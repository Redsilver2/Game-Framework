using UnityEngine;
using UnityEngine.AI;
using RedSilver2.Framework.StateMachines.States.Configurations;
using System.Linq;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class AIMovementController : MovementStateMachineController
    {
        [Space]
        [SerializeField] private int defaultSettingIndex;
        [SerializeField] private MovementStateSettings[] defaultSettings;

        private Transform    target;
        private NavMeshAgent agent;
        private Transform[]  waypoints;


        protected override void Awake() {
            base.Awake();   

            agent = GetComponent<NavMeshAgent>();
            SetStateMachine(GetMovementStateMachine());

            SetDefaultConfigurations();
            (GetStateMachine() as MovementStateMachine)?.AddOnUpdateListener(OnUpdate);

            if (agent != null) agent.updateRotation = false;
        }

        public sealed override void SetStateMachine(StateMachine stateMachine)
        {
            if(stateMachine is AIMovementStateMachine)
                 base.SetStateMachine(stateMachine);
        }

        public void SetTarget(Transform target) {
            this.target = target;
        }

        protected virtual void OnUpdate()
        {
            UpdateDestination();
            if(agent == null) return;

            if (agent.velocity.sqrMagnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,  1000f * Time.deltaTime);
            }
        }

        private void UpdateDestination() {
            if(target == null) {
                agent.destination = transform.position;
                return;
            }

            agent.destination = target.position;
        }

        public void SetSpeed(float speed)
        {
            (GetStateMachine() as MovementStateMachine)?.SetMovementSpeed(speed);
        }

        public void SetAgentSpeed(float speed) {
            if(agent != null) {
                agent.speed = speed;
            }
        }

        public void SetNextPosition(Vector3 nextPosition)
        {
            if(agent != null)
                agent.nextPosition = nextPosition;
        }

        public void SetWaypoints(Transform[] waypoints) {
            this.waypoints = waypoints;
        }

        public Vector3 GetVelocity()
        {
            return agent == null ? Vector3.zero : agent.velocity; 
        }

        private void SetDefaultConfigurations() {
            foreach (var settings in defaultSettings) {
                if (settings == null) continue;
                settings.Register(StateMachine);
            }

            if (defaultSettingIndex >= 0 && defaultSettingIndex < defaultSettings.Length - 1) {
                StateMachine?.ChangeState(defaultSettings[defaultSettingIndex].GetBaseConfiguration(StateMachine));
            }
        }

        public bool IsCloseToTarget() {
            if(agent == null) return false;
            else if(target == null) return true;
           
            return Vector3.Distance(transform.position, target.position) <= agent.stoppingDistance;
        }

        public bool IsTargetPlayer(out PlayerController controller) {
            PlayerController current = PlayerController.Current;
            controller = null;

            if (current == null || target == null) return false;
          
            if (current.transform.Equals(target)) {
                controller = current;
                return true;
            }

            return false;
        }

        public Transform[] GetWaypoints() {
            if(waypoints == null) return new Transform[0];
            return waypoints;
        }

        public Transform GetRandomWaypoint() {
            if (waypoints == null || waypoints.Length == 0) return null;
            var results = waypoints.Where(x => !x.Equals(target)).ToArray();
            return results.Count() > 0 ? results[Random.Range(0, results.Length)] : null;
        }
         
        public abstract AIMovementStateMachine GetMovementStateMachine();
    }
}
