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
        Vector3 startingPosition;
        Fighter fighter;
        Health health;
        UnitCore player;
        private void Start()
        {
            startingPosition = this.gameObject.transform.position;
            fighter = this.gameObject.GetComponent<Fighter>();
            health = this.gameObject.GetComponent<Health>();
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
                fighter.Attack(player.gameObject);
            }
            else
            {
                fighter.Cancel();
                GetComponent<Mover>().StartMoveAction(startingPosition);
            }
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
    }
}

