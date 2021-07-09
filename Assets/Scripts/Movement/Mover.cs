
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {

        NavMeshAgent navMesh;

        Health health;
      


        // Start is called before the first frame update
        void Awake()
        {
            navMesh = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            navMesh.enabled = !health.IsDead();
            UpdateAnimation();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
    
            MoveTo(destination);
        }

        public void MoveTo(Vector3 hit)
        {
            navMesh.destination = hit;
            navMesh.isStopped = false;
        }

        public void Cancel()
        {
            navMesh.isStopped = true;
        }


        void UpdateAnimation()
        {
            var velocity = navMesh.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);
            var speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
    }

}