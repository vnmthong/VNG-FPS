using PYDFramework;
using PYDFramework.MVC;
using PYDFramework.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace VNGFPS
{
    public enum TroopState
    {
        setup,
        idle,
        move, 
        attack,
    }

    public interface IObjectBattle
    {
        bool isFullHp { get; }
        bool isAlive { get; }
        ObjectBattleType GetObjectBattleType();
        void TakeDamage(int dmg);
        void StopGame();
    }

    public enum ObjectBattleType
    {
        troop,
        //building
    }

    public class StatBase
    {
        public StatInt atk = new StatInt();
        public StatFloat moveSpeed = new StatFloat();
        public StatFloat range = new StatFloat();

        public float speedPerSecond => 10 / moveSpeed.Value;

        public StatBase(int atk, float moveSpeed, float range)
        {
            this.atk.BaseValue = atk;
            this.moveSpeed.BaseValue = moveSpeed;
            this.range.BaseValue = range;
        }
    }

    public class TroopStat: StatBase
    {
        public StatFloat atkSpeed = new StatFloat();

        public float atkSpeedPerSecond => atkSpeed.Value;

        public TroopStat(TroopConfig.TroopStats troopStats) : base(troopStats.atk, troopStats.moveSpeed, troopStats.range)
        {
            atkSpeed.BaseValue = troopStats.atkSpeed;
        }
    }

    public abstract class Troop : View<GameApp>, IObjectBattle
    {
        [SerializeField] protected Animator animator;
        [SerializeField] Transform cavas;
        [SerializeField] Transform trfModel;
        [SerializeField] Slider sldHp;
        [SerializeField] TMP_Text txtLv;
        [SerializeField] Transform bombPosition;

        private GameObject model3D = null;

        public NavMeshAgent navMeshAgent;
        public TroopGameModel model { get; private set; }
        public IObjectBattle target { get; protected set; }
        public TroopStat stat { get; private set; }
        public TroopState currentState; //{ get; protected set; }
        public bool isAlive => model.Hp > 0;
        public bool isFullHp => model.Hp >= model.MaxHp;
        public bool isInitialize { get; private set; } = false;
        public Cooldown attackCooldown { get; protected set; } = new Cooldown();
        public Cooldown pauseCooldown { get; protected set; } = new Cooldown();

        private bool isStop;
        public StateMachine smFood;
        public States states;
        public Action<Troop> onTroopDie;
        public Action<Troop, int> onTroopLossHp;
        public bool isMerge => smFood.currentState == states.idle;

        protected GameController gameController => Singleton<GameController>.instance;
        public TroopConfig.TroopStats cfg => app.configs.troopConfigs.GetConfigStat(model.Id, model.Level);
        public bool Pickup()
        {
            if (smFood.currentState != states.idle)
                return false;

            smFood.ChangeState(states.pickup);
            return true;
        }

        public bool Placedown()
        {
            if (smFood.currentState != states.pickup)
                return false;

            smFood.ChangeState(states.idle);
            return true;
        }

        public class States
        {
            public STFoodSpawn spawn;
            public STFoodIdle idle;
            public STFoodPickup pickup;

            public States(Troop agent, StateMachine sm)
            {
                spawn = new STFoodSpawn(agent, sm);
                idle = new STFoodIdle(agent, sm);
                pickup = new STFoodPickup(agent, sm);
            }
        }

        //protected BattleController battleController => Singleton<BattleController>.instance;
        //protected BuilderController builderController => Singleton<BuilderController>.instance;
        //protected MapController mapController => Singleton<MapController>.instance;

        protected override void OnViewInit()
        {
            AddDataKeyListen("sldHp-value", sldHp, (control, e) =>
            {
                if (model == null)
                    return;

                var value = (float)model.Hp / model.MaxHp;
                control.value = value;
                control.gameObject.SetActive(value != 1 && value != 0);
            });

            ListenDataChanged(TroopGameModel.dataChangedEvent, nameof(TroopGameModel.Hp), e =>
             {
                 if (e.sender != model)
                     return;

                 NotifyDataChanged("sldHp-value");
             });

            AddDataKeyListen("txtLevel-text", txtLv, (control, e) =>
            {
                if (model == null)
                    return;

                UpdateStat();
                control.text = $"{model.Level + 1}";
            });

            ListenDataChanged(TroopGameModel.dataChangedEvent, nameof(TroopGameModel.Level), e =>
            {
                if (e.sender != model)
                    return;

                NotifyDataChanged("txtLevel-text");
            });
        }

        private void UpdateStat()
        {
            stat = new TroopStat(cfg);
            model.ResetData(cfg);
            UpdateSpeed();
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (model3D != null)
                Destroy(model3D);

            var prefab = (GameObject)null; //app.resourceManager.GetTroopModel(model.Id, model.Level);
            model3D = Instantiate(prefab, trfModel);
            var anim = model3D.GetComponent<Animator>();
            animator = anim;
        }

        public void Init(TroopGameModel model)
        {
            this.model = model;
            UpdateStat();

            if (smFood == null)
            {
                smFood = new StateMachine();
                states = new States(this, smFood);
                smFood.Init(states.spawn);
            }
            else
                smFood.ChangeState(states.spawn);

            Setup();
            isInitialize = true;
        }

        public void StartBattle()
        {
            currentState = TroopState.idle;
        }

        public void TakeDamage(int dmg)
        {
            if (!isAlive)
                return;

            var lossHp = AddHp(-dmg);
            onTroopLossHp?.Invoke(this, lossHp);
            if (model.Hp == 0)
                Die();
        }

        protected void Die()
        {
            gameController.DestroyTroop(this);
            //animator?.Play("Die", 0, 0.1f);
            //navMeshAgent.enabled = false;
            onTroopDie?.Invoke(this);
        }

        private int AddHp(int amount)
        {
            var remain = model.Hp + amount;
            var newValue = Mathf.Clamp(remain, 0, model.MaxHp);
            var oldValue = model.Hp;
            model.Hp = newValue;
            return newValue - oldValue;
        }

        public void UpdateSpeed()
        {
            navMeshAgent.speed = stat.moveSpeed.Value;
        }    

        public void Stop()
        {
            animator?.Play("Idle");
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
        }

        public void Setup()
        {
            Stop();
            currentState = TroopState.setup;
        }

        private void Update()
        {
            if (!isInitialize)
                return;

            if (isStop)
                return;

            LogicUpdate(Time.deltaTime);
        }

        private void LateUpdate()
        {
            var cam = Camera.main;
            cavas.transform.LookAt(cavas.transform.position + cam.transform.forward);
        }

        private void FixedUpdate()
        {
            if (!isInitialize)
                return;

            if (isStop)
                return;

            PhysicUpdate(Time.fixedDeltaTime);
        }

        //public Bullet SpawnBullet(BulletType bulletType, BulletExploseType exploseType)
        //{
        //    var bomb = Instantiate(gameController.bulletPrefab, gameController.map.parentObject);
        //    bomb.transform.position = bombPosition.position;
        //    //var cfg = app.configs.troopConfigs.GetConfigStat(model.Id, model.Level);
        //    bomb.Init(this, new StatBase(stat.atk.Value, 20, 1), target, exploseType);
        //    return bomb;
        //}

        public virtual void LogicUpdate(float deltaTime)
        {
            var isNavMeshEnable = currentState != TroopState.setup;
            navMeshAgent.enabled = isNavMeshEnable;
        }

        public virtual void PhysicUpdate(float deltaTime)
        {

        } 

        public void StopGame()
        {
            Stop();
            isStop = true;
        }

        public ObjectBattleType GetObjectBattleType()
        {
            return ObjectBattleType.troop;
        }
    }
}
