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
        [SerializeField] PatrolPath patrolPath;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        [SerializeField] float suspicionTime = 3f;

        [SerializeField] float waypointTolerance = 1f;

        [SerializeField] float timeSinceLastWaypointArrival = Mathf.Infinity;
        [SerializeField] float waypointDwellTime = 3f;


        Vector3 guardPosition;
        int currentWaypointIndex = 0;
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
                PatrolBehaviour();
            }
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceLastWaypointArrival += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceLastWaypointArrival = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceLastWaypointArrival > waypointDwellTime)
            {

                mover.StartMoveAction(nextPosition);
            }

        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextWaypointIndex(currentWaypointIndex); 
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance; 
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
