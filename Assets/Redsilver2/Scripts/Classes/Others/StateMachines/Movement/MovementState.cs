using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class MovementState : UpdatableState
    {
        private MovementStateMachine stateMachine;

        private MovementStateType[] incompatibleTransitionStates;
        private MovementStateType   type;
    
        public MovementStateType Type => type;

#if UNITY_EDITOR

        protected virtual void OnValidate()
        {

        }

#endif

        protected override void Awake() {
            base.Awake();
            SetMovementStateType(ref type);
          
            Initialize();

        }

        protected void Start() {
            OnEnabled();
        }

        private void Initialize()
        {
            List<MovementStateType> results = incompatibleTransitionStates != null ? incompatibleTransitionStates.ToList() : new List<MovementStateType>();

            foreach (MovementStateType stateType in GetDefaultInvalidTypes()) {
                if (!results.Contains(stateType)) results?.Add(stateType);
            }

            foreach (MovementStateType stateType in GetRequiredTypes()){
                if (results.Contains(stateType)) results?.Remove(stateType);
            }

            incompatibleTransitionStates = results.ToArray();
        }

        protected abstract MovementStateType[] GetDefaultInvalidTypes();
        protected virtual MovementStateType[] GetRequiredTypes()
        {
            return new MovementStateType[0];
        }


        protected override void OnEnabled() {
            base.OnEnabled();
            OnEnabled(stateMachine);
        }

        protected override void OnDisabled() {
            base.OnDisabled();
            OnDisabled(stateMachine);   
        }

        protected override void OnEntered()
        {
            base.OnEntered();
            OnEntered(stateMachine);
        }

        protected virtual void OnEnabled(MovementStateMachine stateMachine) {
            if (stateMachine == null || stateMachine.ContainsState(this)) return;
            stateMachine?.AddOnMovementStateAddedListener(OnStateAdded);
            stateMachine?.AddOnMovementStateRemovedListener(OnStateRemoved);
            stateMachine?.AddState(this);
        }

        protected virtual void OnDisabled(MovementStateMachine stateMachine)
        {
            if (stateMachine == null || !stateMachine.ContainsState(this)) return;
            stateMachine?.RemoveState(this);
            stateMachine?.RemoveOnMovementStateAddedListener(OnStateAdded);
            stateMachine?.RemoveOnMovementStateRemovedListener(OnStateRemoved);
        }

        protected sealed override bool CanAddTransitionState(UpdatableState state)
        {
            if(state != null) return CanAddTransitionState(state as MovementState); 
            return false;
        }

        private bool CanAddTransitionState(MovementState state) {
            if (state == null || incompatibleTransitionStates == null) return false;
            return !incompatibleTransitionStates.Contains(state.Type);
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

        protected virtual void OnStateAdded(MovementState state) {
            if (stateMachine == null || state == null) return;


            if (state == this) {
                foreach (MovementState _state in stateMachine.States)
                    AddTransitionState(_state);
            }
            else { AddTransitionState(state); }
        }

        protected virtual void OnStateRemoved(MovementState state) {
            if(stateMachine == null || state == null) return;

            if(state == this) {
                foreach (MovementState _state in stateMachine.States)
                    RemoveTransitionState(_state);
            }
            else { RemoveTransitionState(state); }
        }

        public sealed override string GetStateName() {
            return type.ToString();
        }

        public sealed override bool CanTransition() {
            return base.CanTransition() && CanTransition(stateMachine);
        }

        protected virtual bool CanTransition(MovementStateMachine stateMachine) {
            if (stateMachine == null || stateMachine.IsCurrentState(type)) return false;
            return true;
        }

        protected virtual void OnEntered(MovementStateMachine stateMachine) {

        }

        protected virtual void OnUpdate(MovementStateMachine stateMachine) {

        }

        protected abstract void SetMovementStateType(ref MovementStateType type);
    }
}
