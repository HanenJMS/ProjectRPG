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
        bool isAttacking = false;
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        
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
                isAttacking = true;
                if (isAttacking)
                {
                    GetComponent<Mover>().Cancel();
                    AttackBehavior();
                }
            }
        }
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetHealth = combatTarget.GetComponent<Health>();
            return targetHealth != null && !targetHealth.IsDead() && targetHealth != this.gameObject.GetComponent<Health>();
        }
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }
        public void Cancel()
        {
            StopAttackAnimation();
            target = null;
        }
        private void FaceTarget()
        {
            this.gameObject.transform.LookAt(target.transform);
        }
        private void MovingInRangeOfWeapon()
        {
            GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            print("moving in range");
        }
        private void DoDamage()
        {
            target.TakeDamage(weaponDamage);
        }
        private void AttackBehavior()
        {
            FaceTarget();
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                StartAttackAnimation();
                timeSinceLastAttack = 0f;
            }
            isAttacking = false;
        }
        private void StartAttackAnimation()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }
        private void StopAttackAnimation()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }
        //Animation Event
        //animation event is not referenced but triggered with the animation
        void Hit()
        {
            if (target == null) return;
            DoDamage();
        }
    }
}
