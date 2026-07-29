using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using Unity.VisualScripting;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class AIMovementController : MovementStateMachine
    {
        [Space]
        [SerializeField] private int defaultSettingIndex;

        private Transform    target;
        private NavMeshAgent agent;
        private Transform[]  waypoints;


        protected override void Awake() {
            base.Awake();   

            agent = gameObject.GetOrAddComponent<NavMeshAgent>();
            if (agent != null) agent.updateRotation = false;
        }

        public void SetTarget(Transform target) {
            this.target = target;
        }

        protected override void Move()
        {
            if (agent == null) return;
            else if (agent.velocity.sqrMagnitude > 0.1f) {
                Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1000f * Time.deltaTime);
            }


            if (!IsCloseToTarget()) {
                agent?.SetDestination(target != null ? target.position : transform.position);
                Move(agent.nextPosition);
            }


        }

        public void SetWaypoints(Transform[] waypoints) { this.waypoints = waypoints; }
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
    }
}
