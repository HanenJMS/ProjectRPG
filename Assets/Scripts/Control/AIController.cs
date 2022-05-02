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
        [SerializeField] float chaseDistance = 8f;
        [SerializeField] float suspicionTime = 5f;
        Vector3 guardPosition;
        Fighter fighter;
        Health health;
        UnitCore player;
        Mover mover;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        PatrolPath patrolPath;
        private void Start()
        {
            guardPosition = this.gameObject.transform.position;
            fighter = this.gameObject.GetComponent<Fighter>();
            health = this.gameObject.GetComponent<Health>();
            mover = this.gameObject.GetComponent<Mover>();
            patrolPath = this.gameObject.GetComponentInChildren<PatrolPath>();
            setPlayer();
        }

        bool isIdle = false;
        private void Update()
        {
            if (health.IsDead()) return;
            else
                AiBehaviour();
        }

        private void AiBehaviour()
        {
            if (IsWithinDistance() && fighter.CanAttack(player.gameObject))
            {
                timeSinceLastSawPlayer = 0f;
                AttackBehaviour();
                print(gameObject.name + " is attacking : " + player.gameObject.name);
            }
            else if(!IsWithinDistance() && timeSinceLastSawPlayer <= suspicionTime)
            {
                SuspicionBehaviour();
                print(gameObject.name + ": I saw something");
            }
            else
            {
                GuardBehaviour();
            }
            timeSinceLastSawPlayer += Time.deltaTime;

        }
        void PatrolBehaviour()
        {
            List<Waypoint> waypoints = patrolPath.GetWaypoints();
            foreach (Waypoint wp in waypoints)
            {
                mover.StartMoveAction(wp.gameObject.transform.position);
            }
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, chaseDistance);
        }
    }
}

