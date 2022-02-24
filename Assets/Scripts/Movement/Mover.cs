using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] GameObject target;
        NavMeshAgent agent;
        Animator animator;


        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {

            UpdateAnimator();
        }


        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<Fighter>().Cancel();
            MoveTo(destination);
        }
        public void MoveTo(Vector3 destination)
        {
            agent.SetDestination(destination);
            agent.isStopped = false;
        }
        public void Stop()
        {
            agent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }
    }
}
