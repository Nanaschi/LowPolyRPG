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

        float timeSinceLastSawPlayer = Mathf.Infinity;
        [SerializeField] float suspicionTime = 3f;

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
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();

            }
            else if (!GettingDistanceToThePlayer() && timeSinceLastSawPlayer <= suspicionTime)
            {
                SuspicionBehaviour();
            }
            else

            {
                GuardBehaviour();
            }
            timeSinceLastSawPlayer += Time.deltaTime;
            
        }

        private void GuardBehaviour()
        {
            mover.StartMoveAction(guardPosition);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
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
