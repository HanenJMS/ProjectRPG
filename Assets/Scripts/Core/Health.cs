using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float hp = 100f;
        bool isDead = false;
        public void TakeDamage(float damage)
        {
            //if death trigger has not been triggered.
            if(!isDead)
            {
                hp = Mathf.Max(hp - damage, 0);
                print(hp);
                if(hp <= 0)
                {
                    Dying();
                }
            }
        }

        private void Dying()
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

        public bool IsDead()
        {
            return this.isDead;
        }
    }
}
