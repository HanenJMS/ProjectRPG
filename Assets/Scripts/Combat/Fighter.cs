using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5f;
        Health target;
        float timeSinceLastAttack = 0f;
        
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (target.IsDead()) return;
            if (target != null && !GetIsInRange())
            {
                MovingInRangeOfWeapon();
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }
        public bool CanAttack(CombatTarget combatTarget)
        {
            if(combatTarget == null) return false;
            Health testHealth = combatTarget.GetComponent<Health>();
            return testHealth != null && !testHealth.IsDead();
        }
        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }
        public void Cancel()
        {
            StopAttackAnimation();
            target = null;
        }

        private void StopAttackAnimation()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        private void FaceTarget()
        {
            this.gameObject.transform.LookAt(target.transform);
        }
        private void MovingInRangeOfWeapon()
        {
            GetComponent<Mover>().MoveTo(target.transform.position);
            print("moving in range");
        }
        private void DoDamage()
        {
            target.TakeDamage(weaponDamage);
        }
        private void AttackBehavior()
        {
            FaceTarget();
            if(timeSinceLastAttack > timeBetweenAttacks)
            {
                StartAttackAnimation();
                timeSinceLastAttack = 0f;
            }

        }

        private void StartAttackAnimation()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        //Animation Event
        void Hit()
        {
            if (target == null) return;
            DoDamage();
        }
    }
}
