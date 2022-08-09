using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {

        [SerializeField] float timeBetweenAttacks = 1f;

        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        bool isAttacking = false;
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;
        private void Awake()
        {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetUpDefaultWeapon);
        }
        private void Start()
        {
            currentWeapon.ForceInit();
        }
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
        //Initialization methods
        private Weapon SetUpDefaultWeapon()
        {
            
            return AttachWeapon(defaultWeapon);
        }
        //Combat Interaction
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position)) return false;
            Health targetHealth = combatTarget.GetComponent<Health>();
            return targetHealth != null && !targetHealth.IsDead() && targetHealth != this.gameObject.GetComponent<Health>();
        }
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }
        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeaponConfig.GetRange();
        }
        public void Cancel()
        {
            StopAttackAnimation();
            target = null;
        }
        //Combat Modifiers
        public IEnumerable<float> GetAdditiveModifer(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetDamageOutput();
            }
        }
        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPercentageBonus();
            }
        }
        //Weapon Interaction
        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }
        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }
        //Fighter Behaviour
        private void FaceTarget()
        {
            this.gameObject.transform.LookAt(target.transform);
        }
        private void MovingInRangeOfWeapon()
        {
            GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            print("moving in range");
        }
        private void DoDamage(float damage)
        {
            target.TakeDamage(gameObject, damage);
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
        //Fighter animator behaviour
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

        //Animation Event
        //animation event is not referenced but triggered with the animation
        void Hit()
        {
            if (target == null) return;
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if(currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }
            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                DoDamage(damage);
            }
        }
        void Shoot()
        {
            Hit();
        }
        //Saving Interface
        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }
        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }


    }
}
