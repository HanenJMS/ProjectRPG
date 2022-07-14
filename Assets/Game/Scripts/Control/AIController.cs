using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 8f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] float patrolHoldTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerence = 1f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        int currentPosition = 0; 
        Vector3 guardPosition;
        Fighter fighter;
        Health health;
        UnitCore player;
        Mover mover;
        bool hasArrived;

        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private void Awake()
        {
            fighter = this.gameObject.GetComponent<Fighter>();
            health = this.gameObject.GetComponent<Health>();
            mover = this.gameObject.GetComponent<Mover>();
        }
        private void Start()
        {
            guardPosition = this.gameObject.transform.position;
            setPlayer();
        }
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
                
                AttackBehaviour();
                print(gameObject.name + " is attacking : " + player.gameObject.name);
            }
            else if (!IsWithinDistance() && timeSinceLastSawPlayer <= suspicionTime)
            {
                SuspicionBehaviour();
                print(gameObject.name + ": I saw something");
            }
            else
            {
                
                PatrolBehaviour();
            }
            UpdateTimers();

        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if(timeSinceArrivedAtWaypoint > patrolHoldTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
            
            
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerence;
        }
        
        private void CycleWaypoint()
        {
            currentPosition = patrolPath.GetNextIndex(currentPosition);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentPosition);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0f;
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

