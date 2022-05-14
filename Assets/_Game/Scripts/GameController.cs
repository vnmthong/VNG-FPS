using PYDFramework;
using PYDFramework.MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VNGFPS
{
    public class GameController : MVCBase<GameApp>
    {
        private void Awake()
        {
            Singleton<GameController>.Set(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Singleton<GameController>.Unset(this);
        }
        [SerializeField] Transform parentObject;
        public Map map;
        public GameObject meleePrefab;
        public GameObject archerPrefab;

        public List<Troop> foods = new List<Troop>();
        public List<Troop> enemies = new List<Troop>();


        public Action onStageStart;
        public Action<bool> onStageEnd;

        private bool _isGameRunning = false;

        

        public void SpawnEnemies(int level)
        {
            DestroyAllTroops(ref enemies);
            var cfg = app.configs.levelConfigs.GetConfig(level);

            foreach (var data in cfg.levelDatas)
            {
                var troopCfg = app.configs.troopConfigs.GetConfigStat(data.id);
                var troop = new TroopGameModel(troopCfg.id, troopCfg.level, data.slotId, troopCfg.hp, TeamType.Devil);
                SpawnTroop(troop);
            }
        }

        public void LoadStage(int level)
        {
            _isGameRunning = false;
            SpawnEnemies(level);
        }

        public void StartBattle()
        {
            _isGameRunning = true;

            foreach (var troop in foods)
            {
                troop.StartBattle();
            }

            foreach (var devil in enemies)
            {
                devil.StartBattle();
            }
        }

        private void DestroyAllTroops(ref List<Troop> troops)
        {
            foreach (var troop in troops)
            {
                Destroy(troop.gameObject);
            }

            troops.Clear();
        }


        private int GetEmptySlot(TeamType teamType)
        {
            if (!IsCanSpawn())
                return -1;
            var troops = GetTroopsInTeam(teamType);
            var cards = troops.Select(e => e.model.SlotId).ToList();
            cards.Sort();

            var avaiableSlot = 0;
            var maximumSlot = 15;
            for (avaiableSlot = 0; avaiableSlot < maximumSlot; ++avaiableSlot)
            {
                if (avaiableSlot >= cards.Count) break;
                if (avaiableSlot < cards[avaiableSlot]) break;
            }

            return avaiableSlot;
        }

        private bool IsCanSpawn()
        {
            return foods.Count < 15;
        }

        public GameObject GetTroopPrefab(string id)
        {
            switch (id)
            {
                case "BM":
                case "M": return meleePrefab;
                case "BR":
                case "R": return archerPrefab;
                default: return null;
            }
        }

        private Troop SpawnTroop(TroopGameModel foodModel)
        {
            var prefab = GetTroopPrefab(foodModel.Id);
            if (!prefab)
                Debug.LogError($"Null prefab for: {foodModel.Id}");

            var foodGo = Instantiate(prefab);
            var food = foodGo.GetOrAddComponent<Troop>();
            foodGo.transform.SetParent(parentObject, false);
            food.Init(foodModel);
            food.onTroopDie += OnTroopDie;

            if (foodModel.team == TeamType.Hero)
                foods.Add(food);
            else
                enemies.Add(food);

            return food;
        }

        public List<Troop> GetTroopsInTeam(TeamType teamType)
        {
            switch (teamType)
            {
                case TeamType.Hero: return foods;
                case TeamType.Devil: return enemies;
                default: return null;
            }
        }

        public void DestroyTroop(Troop food)
        {
            RemoveTroop(food);
            Destroy(food.gameObject);
        }

        public void RemoveTroop(Troop food)
        {
            var team = food.model.team;
            if (team == TeamType.Hero)
            {
                foods.Remove(food);
            }
            else
            {
                enemies.Remove(food);
            }
        }

        private void Merge(TroopGameModel pickupFood, TroopGameModel targetFood)
        {
            if (pickupFood.Level == targetFood.Level)
            {
                pickupFood.Level++;
            }
        }

        private Troop GetNearestFood(Vector3 pos)
        {
            var listFood = foods;

            var minIndex = -1;
            var min = float.MaxValue;
            for (int i = 0; i < listFood.Count; i++)
            {
                var posTouch = pos;
                var posFood = listFood[i].transform.position;
                var distance = Vector3.Distance(posTouch, posFood);
                if (distance <= 0.5f && distance < min)
                {
                    min = distance;
                    minIndex = i;
                }
            }

            if (minIndex < 0)
                return null;

            return listFood[minIndex];
        }

        private void OnTroopDie(Troop troop)
        {
            var team = troop.model.team;
            var troops = GetTroopsInTeam(team);
            if (troops.Count != 0)
                return;

            var isWin = team == TeamType.Devil;
            onStageEnd?.Invoke(isWin);
        }
    }
}