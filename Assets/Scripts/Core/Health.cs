using UnityEngine;
using System.Collections;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100;
        bool alreadyDead;

        public bool IsDead()
        {
            return alreadyDead;
        }

        public void TakeDamage (float damage)
        {

            health = Mathf.Max(health - damage, 0);
            print(health);

            //if (health <= 0) return;
            //health -= damage;
            if (health <= 0 && !alreadyDead)
            {
                Die();
            }


        }

        private void Die()
        {
            GetComponent<Animator>().SetTrigger("dead");
            alreadyDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }

}