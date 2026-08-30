using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public abstract class MovementState : UpdatableState
    {
        [SerializeField][HideInInspector] protected MovementStateMachine MovementStateMachine;
        private MovementStateType type;
        public MovementStateType Type => type;

        protected MovementState() : base() {
            SetMovementStateType(ref type);
            SetStateName(type.ToString());
        }

#if UNITY_EDITOR
        public virtual void Validate(MovementStateMachine movementStateMachine)
        {
            SetStateMachine(movementStateMachine);
            Validate();
        }
#endif

        protected override void SetIncompatibleTransitionStates(ref string[] incompatibleStates)
        {
            List<string> results = new List<string>();

            foreach (MovementStateType stateType in GetDefaultInvalidTypes()) {
                string typeName = stateType.ToString();
                if (!results.Contains(typeName)) results?.Add(typeName);
            }

            foreach (MovementStateType stateType in GetRequiredTypes()) {
                string typeName = stateType.ToString();
                if (results.Contains(typeName)) results?.Remove(typeName);
            }

            incompatibleStates = results.ToArray();
            base.SetIncompatibleTransitionStates(ref incompatibleStates);
        }



        protected abstract MovementStateType[] GetDefaultInvalidTypes();
        protected virtual MovementStateType[] GetRequiredTypes()
        {
            return new MovementStateType[0];
        }

        private void SetStateMachine(MovementStateMachine stateMachine)
        {
            this.MovementStateMachine = stateMachine;
            SetStateMachine(stateMachine as UpdatableStateMachine);
        }


        protected sealed override bool CanAddTransitionState(State state)
        {
            return base.CanAddTransitionState(state) && CanAddTransitionState(state as MovementState); 
        }

        private bool CanAddTransitionState(MovementState state) {
            return state != null ? true : false;
        }

        protected abstract void SetMovementStateType(ref MovementStateType type);
    }
}
