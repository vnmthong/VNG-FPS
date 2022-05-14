using PYDFramework;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace VNGFPS
{
    public class TroopMelee : Troop
    {
        private Vector3 destination;
        private Troop targetBuilding => target as Troop;
        public override void LogicUpdate(float deltaTime)
        {
            base.LogicUpdate(deltaTime);
            if (!isAlive)
                return;

            switch (currentState)
            {
                case TroopState.setup:
                    {
                        UpdateSetup(deltaTime);
                        break;
                    }
                case TroopState.idle:
                    {
                        UpdateIdle(deltaTime);
                        break;
                    }
                case TroopState.move:
                    {
                        UpdateMove(deltaTime);
                        break;
                    }
                case TroopState.attack:
                    {
                        UpdateAttack(deltaTime);
                        break;
                    }
            }
        }

        private void UpdateSetup(float deltaTime)
        {
            return;
        }    

        private void UpdateIdle(float deltaTime)
        {
            if (targetBuilding == null || !target.isAlive)
                target = GetNearestEnermy();

            if (target == null)
                return;

            NavMesh.Raycast(transform.position, targetBuilding.transform.position, out NavMeshHit hit, NavMesh.AllAreas);

            navMeshAgent.isStopped = false;
            destination = hit.position;
            navMeshAgent.SetDestination(destination);
            animator?.Play("Move", 0, 0.1f);
            currentState = TroopState.move;
        }

        private void UpdateMove(float deltaTime)
        {
            if (targetBuilding == null || !target.isAlive)
            {
                Stop();
                currentState = TroopState.idle;
                return;
            }

            var distance = Vector3.Distance(transform.position, targetBuilding.transform.position);
            if (distance <= stat.range.Value)
            {
                Stop();
                transform.LookAt(targetBuilding.transform.position);
                currentState = TroopState.attack;
            }
        }

        private void UpdateAttack(float deltaTime)
        {
            attackCooldown.Update(deltaTime);

            if (targetBuilding == null || !target.isAlive)
            {
                currentState = TroopState.idle;
                return;
            }

            var distance = Vector3.Distance(transform.position, targetBuilding.transform.position);
            if (distance > stat.range.Value)
            {
                currentState = TroopState.idle;
                return;
            }

            if (attackCooldown.isFinished)
            {
                attackCooldown.Restart(stat.atkSpeedPerSecond);
                Attack();
            }
        }

        private void Attack()
        {
            var clips = animator.runtimeAnimatorController.animationClips;
            var attacks = clips.Where(e => e.name.Contains("Attack")).ToList();
            var attack = attacks.RandomElement().name;
            animator?.Play(attack, 0, 0.1f);
            target.TakeDamage(stat.atk.Value);
        }

        private Troop GetNearestEnermy()
        {
            var min = float.MaxValue;
            Troop nearestBuilding = null;
            var enemies = gameController.GetTroopsInTeam(model.enermyTeam);
            var buildings = enemies.Where(e => e.isAlive).ToList();
            foreach (var building in buildings)
            {
                var distance = Vector3.Distance(transform.position, building.transform.position);
                if (distance < min)
                {
                    min = distance;
                    nearestBuilding = building;
                }
            }

            return nearestBuilding;
        }

        public override void PhysicUpdate(float deltaTime)
        {
            base.PhysicUpdate(deltaTime);
        }
    }
}
