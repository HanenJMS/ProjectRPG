using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    //[System.Serializable]
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;
        [SerializeField] UnityEvent takeDamage;
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
                takeDamage.Invoke();
                if (hp.value <= 0)
                {
                    Die();
                    AwardExperience(instigator);
                }
            }
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
            return hp;
        }
        public void RestoreState(object state)
        {
            hp.value = (float)state;
            this.hp.value = hp.value;
            if (hp.value <= 0)
            {
                Die();
            }
        }
    }
}
