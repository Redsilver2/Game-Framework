using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class MovementGroundCheckExtension : StateModule {
        [Space]
        [SerializeField] private float groundCheckRange;
       
        private bool    isGrounded;
        private string  groundTag;
        private Vector3 hitPosition;

        public float  GroundCheckRange => groundCheckRange;
        public bool   IsGrounded       => isGrounded;
        public string GroundTag        => groundTag;
        public Vector3 HitPosition     => hitPosition;

        protected override void Start()
        {
            base.Start();
            stateMachine?.AddOnUpdateListener(OnUpdate);
        }

        protected sealed override void OnDisable()
        {
            base.OnDisable();
            stateMachine?.RemoveOnUpdateListener(OnUpdate); 
        }

        protected sealed override void OnEnable() {
            base.OnEnable();
            stateMachine?.AddOnUpdateListener(OnUpdate);
        }

        private void OnUpdate()
        {
            OnUpdate(groundCheckRange, ref isGrounded, ref groundTag, ref hitPosition);
        }

        protected sealed override StateMachineController GetStateMachineStateMachine(StateMachineController controller)
        {
            if (controller is not MovementStateMachineController) return null;
            return base.GetStateMachineStateMachine(controller);
        }

        protected sealed override UnityAction<State> GetOnStateAddedAction() { return null; }
        protected sealed override UnityAction<State> GetOnStateRemovedAction() { return null; }
           



        protected sealed override string GetModuleName() {
            return "Ground Check";
        }

        protected sealed override bool CanAddOrRemoveState(State state)
        {
            return state is MovementState;
        }

        protected abstract void OnUpdate(float groundCheckRange, ref bool isGrounded, ref string groundTag, ref Vector3 hitPosition);

    }
}
