using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class MovementState : UpdatableState
    {
        private MovementStateMachine stateMachine;
        private MovementStateType   type;
    
        public MovementStateType Type => type;

        protected override void Awake() {
            base.Awake();

            SetStateMachine(transform.root != null ? transform.root.GetComponentInChildren<MovementStateMachine>() :
                                                                    GetComponentInChildren<MovementStateMachine>());

            SetMovementStateType(ref type);
            SetStateName(type.ToString());
        }

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


        protected sealed override void OnEnabled(UpdatableStateMachine stateMachine) {
            base.OnEnabled(stateMachine);
            OnEnabled(stateMachine as MovementStateMachine);
        }

        protected sealed override void OnDisabled(UpdatableStateMachine stateMachine) {
            base.OnDisabled(stateMachine);
            OnDisabled(stateMachine as MovementStateMachine);   
        }

        protected sealed override void OnEntered(UpdatableStateMachine stateMachine)
        {
            base.OnEntered(stateMachine);
            OnEntered(stateMachine as MovementStateMachine);
        }

        protected sealed override void OnExited(UpdatableStateMachine stateMachine)
        {
            base.OnExited(stateMachine);
            OnExited(stateMachine as MovementStateMachine);
        }

        protected virtual void OnEntered(MovementStateMachine stateMachine) { }
        protected virtual void OnExited(MovementStateMachine stateMachine) { }

        protected virtual void OnEnabled(MovementStateMachine stateMachine) { }
        protected virtual void OnDisabled(MovementStateMachine stateMachine) { }
        protected virtual void OnUpdate(MovementStateMachine stateMachine) { }

        protected sealed override bool CanAddTransitionState(UpdatableState state)
        {
            if(state != null) return CanAddTransitionState(state as MovementState); 
            return false;
        }

        private bool CanAddTransitionState(MovementState state) {
            return state != null ? true : false;
        }




        protected override void SetStateMachine(StateMachine stateMachine)
        {
            base.SetStateMachine(stateMachine);
            this.stateMachine = stateMachine as MovementStateMachine;
        }


        protected override void OnUpdate() {
            base.OnUpdate();
            OnUpdate(stateMachine);
        }

        public sealed override bool CanTransition(UpdatableStateMachine stateMachine) {
            if(base.CanTransition(stateMachine)) return CanTransition(stateMachine as MovementStateMachine);
            return false;
        }

        public virtual bool CanTransition(MovementStateMachine stateMachine)  {
            if (stateMachine == null || stateMachine.IsCurrentState(type)) return false;
            return true;
        }


        protected abstract void SetMovementStateType(ref MovementStateType type);
    }
}
