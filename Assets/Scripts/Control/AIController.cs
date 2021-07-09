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
        [SerializeField] float alertDistance = 5;
        Health health;
        Fighter fighter;
        GameObject player;
        Mover mover;

        Vector3 guardPosition;

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindGameObjectWithTag("Player");
            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;    

            if (GettingDistanceToThePlayer() && fighter.CanAttack(player)) 
            {
                fighter.Attack(player);

                print(gameObject.name + " starts chasing and the distance between us is ");

            } else 
            
            {
              
                    fighter.Cancel();
                mover.StartMoveAction(guardPosition);


            }
            
        }




        private bool GettingDistanceToThePlayer()
        {
        
            var distanceToPlayer =  Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer <= alertDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, alertDistance);
        }
    }
}
