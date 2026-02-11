using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public class GroundMovementStateCondition : MovementStateCondition
    {
        private MovementGroundCheckExtension checkExtension;

        protected override void Awake()
        {
            base.Awake();
            checkExtension = transform.root.GetComponentInChildren<MovementGroundCheckExtension>(); 
        }

        public sealed override bool GetTransitionState()
        {
            if (checkExtension == null) return false;
            return checkExtension.IsGrounded;
        }

        protected override string GetModuleName() {
            return "Ground Movement State Condition";
        }
      
        protected sealed override void OnStateModuleAdded(StateModule module)
        {
           if(module is MovementGroundCheckExtension) { checkExtension = module as MovementGroundCheckExtension; }
        }
    }
}
