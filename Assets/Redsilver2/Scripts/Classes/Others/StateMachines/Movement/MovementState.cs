using RedSilver2.Framework.StateMachines.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class MovementState : UpdatableState
    {
        [Space]
        [SerializeField] private MovementStateType[] incompatibleTransitionStates;

        private MovementStateType   type;
        private MovementStateMachine stateMachine;


        private List<MovementState> transitionStates;
        public MovementStateType Type => type;
        protected MovementStateMachine StateMachine => stateMachine;

#if UNITY_EDITOR

        protected virtual void OnValidate()
        {
            ValidateIncompatibleTransitionStates(ref incompatibleTransitionStates);
        }

        protected abstract void ValidateIncompatibleTransitionStates(ref MovementStateType[] stateTypes);
#endif

        protected override void Awake() {
            base.Awake();

            stateMachine = GetComponent<MovementStateMachine>();
            transitionStates = new List<MovementState>();

            SetMovementStateType(ref type);    
            incompatibleTransitionStates = incompatibleTransitionStates.Distinct().ToArray();

            AddOnUpdatedListener(OnUpdate);
            AddOnDisabledListener(OnDisabled);

            AddOnEnabledListener(OnEnabled);
        }

        private void Start()
        {
            OnEnabled();
        }

        protected virtual void OnEnabled()
        {
            stateMachine?.AddOnStateAddedListener(OnStateAdded);
            stateMachine?.AddOnStateRemovedListener(OnStateRemoved);
            stateMachine?.AddState(this);
        }

        protected virtual void OnDisabled()
        {
            stateMachine?.RemoveState(this);
            stateMachine?.RemoveOnStateAddedListener(OnStateAdded);
            stateMachine?.RemoveOnStateRemovedListener(OnStateRemoved);
        }

        protected virtual void OnUpdate() {
            if (transitionStates != null) {
                foreach(MovementState state in transitionStates) {
                    if (state == null || !state.CanTransition()) continue;
                    stateMachine?.ChangeState(state.type);   
                    break;
                }
            }

            OnUpdate(stateMachine);
        }

        protected virtual void OnStateAdded(MovementState state) {
            if (stateMachine == null || transitionStates == null) return;
            MovementState[] states = stateMachine.States;

            for (int i = 0; i < states.Length; i++) {
                if (states[i] == this || states[i] == null) continue;
                else if (IsValidTransitionState(states[i]) && !transitionStates.Contains(states[i])) transitionStates.Add(states[i]);
            }
        }

        protected virtual void OnStateRemoved(MovementState state) {
            if(stateMachine == null) return;
            MovementState[] states = stateMachine.States;

            for (int i = 0; i < states.Length; i++) {
                if (transitionStates.Contains(states[i])) transitionStates.Remove(states[i]);
            }
        }

        private bool CanTransition() {
            if (enabled == false) return false;
            return CanTransition(stateMachine); 
        }


        protected bool IsValidTransitionState(MovementStateType stateType)  {
            if (incompatibleTransitionStates == null) return false;
            return !incompatibleTransitionStates.Contains(stateType);
        }

        protected sealed override bool IsValidTransitionState(State state) {
            return IsValidTransitionState(state as MovementState);
        }

        protected bool IsValidTransitionState(MovementState state) {
            if  (state == null || incompatibleTransitionStates == null) return false;
            return IsValidTransitionState(state.type);
        }

        public sealed override string GetStateName() {
            return type.ToString();
        }

        public static MovementStateType[] GetStateTypes(){
            return ((MovementStateType[])Enum.GetValues(typeof(MovementStateType)));
        }

        public static MovementStateType[] GetExcludedStateTypes(MovementStateType[] ignoredStates) {
            if(ignoredStates == null || ignoredStates.Length == 0) return GetStateTypes();
            MovementStateType[] results = GetStateTypes();

            foreach(MovementStateType type in ignoredStates) 
                results = results.Where(x => x != type).Distinct().ToArray();

            return results;
        }

        public static MovementStateType[] GetIncludedStateTypes(MovementStateType[] includedStates)
        {
            if (includedStates == null || includedStates.Length == 0) return GetStateTypes();
            MovementStateType[] results = GetStateTypes();

            foreach (MovementStateType type in includedStates) results = results.Where(x => x == type).Distinct().ToArray();
            return results;
        }

        protected abstract void SetMovementStateType(ref MovementStateType type);
        protected abstract bool CanTransition(MovementStateMachine stateMachine);
        protected abstract void OnUpdate(MovementStateMachine stateMachine);
    }
}
