using RedSilver2.Framework.StateMachines.States.Conditions;
using RedSilver2.Framework.StateMachines.States.Configurations;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class UpdateableStateConfiguration : StateConfiguration
    {
        private UnityEvent onUpdate;
        private UnityEvent onLateUpdate;

        protected UpdateableStateConfiguration(UpdateableStateMachine stateMachine) : base(stateMachine) {
            onUpdate     = new UnityEvent();
            onLateUpdate = new UnityEvent();

            AddOnAddedListener(() => {
                UpdateableStateMachine updateableState = StateMachine as UpdateableStateMachine;
                updateableState?.AddOnUpdateListener(UpdateTransition);
            });
            AddOnRemovedListener(() =>
            {
                UpdateableStateMachine updateableState = StateMachine as UpdateableStateMachine;
                updateableState?.RemoveOnUpdateListener(UpdateTransition);
            });

            AddOnEnteredListener(() => {
                UpdateableStateMachine updateableState = StateMachine as UpdateableStateMachine;
                updateableState?.AddOnUpdateListener(Update);
                updateableState?.AddOnLateUpdateListener(LateUpdate); 
            });

            AddOnExitedListener(() => {
                UpdateableStateMachine updateableState = StateMachine as UpdateableStateMachine;
                updateableState?.RemoveOnUpdateListener(Update);
                updateableState?.RemoveOnLateUpdateListener(LateUpdate);  
            });
        }

        private void Update()
        {
            onUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            onLateUpdate?.Invoke();
        }

        private void UpdateTransition()
        {
            if (StateMachine == null) return;

            if (!StateMachine.IsCurrentConfiguration(this) && CanTransition()) {
                StateMachine?.ChangeState(this);
            }
        }


        public void AddOnUpdateListener(UnityAction action)
        {
            if (action != null) onUpdate?.AddListener(action);
        }

        public void RemoveOnUpdateListener(UnityAction action)
        {
            if (action != null) onUpdate?.RemoveListener(action);
        }

        public void AddOnLateUpdateListener(UnityAction action)
        {
            if (action != null) onLateUpdate?.AddListener(action);
        }

        public void RemoveOnLateUpdateListener(UnityAction action)
        {
            if (action != null) onLateUpdate?.RemoveListener(action);
        }
    }
}
