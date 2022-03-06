using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 3f;
        [SerializeField] float suspicionTime = 5f;
        Vector3 guardPosition;
        Fighter fighter;
        Health health;
        UnitCore player;
        Mover mover;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        private void Start()
        {
            guardPosition = this.gameObject.transform.position;
            fighter = this.gameObject.GetComponent<Fighter>();
            health = this.gameObject.GetComponent<Health>();
            mover = this.gameObject.GetComponent<Mover>();
            setPlayer();
        }
        private void Update()
        {
            if (health.IsDead()) return;
            else
                AttackingPlayer();
            
        }

        private void AttackingPlayer()
        {
            if (IsWithinDistance() && fighter.CanAttack(player.gameObject))
            {
                timeSinceLastSawPlayer = 0f;
                AttackBehaviour();

            }
            else if(!IsWithinDistance() && timeSinceLastSawPlayer <= suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                GuardBehaviour();
            }
            timeSinceLastSawPlayer += Time.deltaTime;

        }

        private void GuardBehaviour()
        {
            mover.StartMoveAction(guardPosition);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player.gameObject);
        }

        private bool IsWithinDistance()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer <= chaseDistance;
        }

        private void setPlayer()
        {
            UnitCore[] unitCores = FindObjectsOfType<UnitCore>();
            foreach (UnitCore unitCore in unitCores)
            {
                if (unitCore == null)
                {
                    continue;
                }
                if (unitCore.GetFaction() == "Player")
                {
                    this.player = unitCore;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, chaseDistance);
        }
    }
}

