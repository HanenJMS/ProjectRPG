using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    //[System.Serializable]
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }
        LazyValue<float> hp;
        bool isDead = false;
        private void Awake()
        {
            hp = new LazyValue<float>(GetInitialHp);
        }
        float GetInitialHp()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        private void Start()
        {
            hp.ForceInit();
            
        }
        private void OnEnable()
        {
            GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
        }
        private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelUp -= RegenerateHealth;
        }
        //Modifiers In
        public float GetCurrentHealth()
        {
            return hp.value;
        }
        public float GetLevelMaxHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        public float GetPercentage()
        {
            return (hp.value / GetComponent<BaseStats>().GetStat(Stat.Health)) * 100;
        }
        public bool IsDead()
        {
            return this.isDead;
        }
        //Modifiers Out
        public void TakeDamage(GameObject instigator, float damage)
        {
            //if death trigger has not been triggered.
            if (!isDead)
            {
                hp.value = Mathf.Max(hp.value - damage, 0);
                print($"{gameObject.name} took damage: {damage}");
                takeDamage.Invoke(damage);
                if (hp.value <= 0)
                {
                    onDie.Invoke();
                    Die();
                    AwardExperience(instigator);
                }
            }
        }
        public void Heal(float healthToRestore)
        {
            hp.value = Mathf.Min(hp.value + healthToRestore, GetLevelMaxHealth());
        }
        private void AwardExperience(GameObject instigator)
        {
            Experience exp = instigator.GetComponent<Experience>();
            if (exp == null) return;
            exp.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }
        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            hp.value = Mathf.Max(hp.value, regenHealthPoints);
        }
        //helper Methods
        private void Die()
        {
            if (isDead) return;

            PlayDeathAnimation();
            isDead = !isDead;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        //Visual Effects
        private void PlayDeathAnimation()
        {
            this.gameObject.GetComponent<Animator>().SetTrigger("die");
        }
        //Saving System
        public object CaptureState()
        {
            return hp.value;
        }
        public void RestoreState(object state)
        {
            hp.value = (float)state;
            if (hp.value <= 0)
            {
                Die();
            }
            else
            {
                isDead = false;
                GetComponent<Animator>().Play("Locomotion");
            }
        }
    }
}
