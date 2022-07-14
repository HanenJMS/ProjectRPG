using RPG.Attributes;
using RPG.Core;
using RPG.Saving;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] GameObject target;
        [SerializeField] float maxSpeed = 6.0f;
        NavMeshAgent agent;
        Animator animator;
        Health health;

        private void Awake()
        {
            agent = this.gameObject.GetComponent<NavMeshAgent>();
            animator = this.gameObject.GetComponent<Animator>();
            health = this.gameObject.GetComponent<Health>();
        }
        void Start()
        {
        }
        // Update is called once per frame
        void Update()
        {
            agent.enabled = !health.IsDead();
            UpdateAnimator();
        }


        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            agent.SetDestination(destination);
            agent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            agent.isStopped = false;
        }
        public void Cancel()
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

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 sz = (SerializableVector3)state;
            agent.enabled = false;
            this.transform.position = sz.GetVector3();
            agent.enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
