using UnityEngine;
using UnityEngine.AI;
using RedSilver2.Framework.StateMachines.States.Configurations;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class AIMovementController : MovementStateMachineController
    {
        [SerializeField] private float targetCheckDistance;


        [Space]
        [SerializeField] private int defaultSettingIndex;
        [SerializeField] private MovementStateSettings[] defaultSettings;

        public  Transform    target;
        private NavMeshAgent agent;
        private MovementStateMachineController controller;

        public float        TargetCheckDistance => targetCheckDistance;
        public Transform    Traget => target;
        public NavMeshAgent Agent  => agent;


        protected void Awake() {
            agent = GetComponent<NavMeshAgent>();
            SetStateMachine(GetMovementStateMachine());
            SetDefaultConfigurations();
        }

        public sealed override void SetStateMachine(StateMachine stateMachine)
        {
            if(stateMachine is AIMovementStateMachine)
                 base.SetStateMachine(stateMachine);
        }

        public void SetTarget(Transform target) {
            this.target = target;
        }

        public void UpdateDestination() {
            if(target == null || Vector3.Distance(transform.position, target.position) < targetCheckDistance) {
                agent.destination = transform.position;
                return;
            }

            agent.destination = target.position;
        }

        public void SetSpeed(float speed) {
            if(agent != null) {
                agent.speed = speed;
            }
        }

        public Vector3 GetNextPosition()
        {
            return agent == null ? Vector3.zero : agent.nextPosition;
        }

        public Vector3 GetVelocity()
        {
            return agent == null ? Vector3.zero : agent.velocity; 
        }


        private void SetDefaultConfigurations()
        {
            foreach (var settings in defaultSettings) {
                if (settings == null) continue;
                settings.Register(StateMachine);
            }

            if (defaultSettingIndex >= 0 && defaultSettingIndex < defaultSettings.Length - 1) {
                StateMachine?.ChangeState(defaultSettings[defaultSettingIndex].GetBaseConfiguration(StateMachine));
            }
        }

        public abstract AIMovementStateMachine GetMovementStateMachine();
    }
}
