using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public class JumpMovementStateCondition : MovementStateCondition
    {
        private JumpStateInitializer initializer;

        protected override void Awake()
        {
            base.Awake();
            initializer = transform.root.GetComponentInChildren<JumpStateInitializer>();
        }

        public sealed override bool GetTransitionState()
        {
            if (initializer == null) return false;
            return initializer.TransitionState;
        }

        protected sealed override string GetModuleName()
        {
            return "Jump Movement State Condition";
        }

        protected sealed override void OnStateModuleAdded(StateModule module)
        {
            if(module is  JumpStateInitializer) initializer = module as JumpStateInitializer;
        }
    }
}