using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    //[System.Serializable]
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float hp = -1f;
        bool isDead = false;
        private void Start()
        {
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
                print(hp);
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
            float hpMax = GetComponent<BaseStats>().GetStat(Stat.Health);
            return (hp / hpMax) * 100;
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
    }
}
