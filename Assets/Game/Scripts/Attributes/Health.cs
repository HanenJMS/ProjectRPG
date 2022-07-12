using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using UnityEngine;

namespace RPG.Attributes
{
    //[System.Serializable]
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField]float hp = -1f;
        [SerializeField] float regenerationPercentage = 70;
        bool isDead = false;
        private void Start()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
            if(hp < 0)
            {
                hp = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

    

        public void TakeDamage(GameObject instigator, float damage)
        {
            //if death trigger has not been triggered.
            if(!isDead)
            {
                hp = Mathf.Max(hp - damage, 0);
                if(hp <= 0)
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

        private void Die()
        {
            if (isDead) return;
            
            PlayDeathAnimation();
            isDead = !isDead;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void PlayDeathAnimation()
        {
            this.gameObject.GetComponent<Animator>().SetTrigger("die");
        }
        public float GetPercentage()
        {
            return (hp / GetComponent<BaseStats>().GetStat(Stat.Health)) * 100;
        }
        public bool IsDead()
        {
            return this.isDead;
        }

        public object CaptureState()
        {
            return hp;
        }

        public void RestoreState(object state)
        {
            float hp = (float)state;
            this.hp = hp;
            if(hp <= 0)
            {
                Die();
            }
        }
        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            hp = Mathf.Max(hp, regenHealthPoints);
        }
    }
}
