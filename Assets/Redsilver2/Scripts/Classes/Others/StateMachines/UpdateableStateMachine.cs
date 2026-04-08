using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using RedSilver2.Framework.StateMachines.States.Configurations;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines
{
    public class UpdateableStateMachine : StateMachine
    {
        private readonly UnityEvent onUpdate;
        private readonly UnityEvent onLateUpdate;

        public UpdateableStateMachine(UpdateableStateMachineController controller) : base(controller) {
            onUpdate     = new UnityEvent();
            onLateUpdate = new UnityEvent();
        }

        public void Update()
        {
            onUpdate?.Invoke();
        }

        public void LateUpdate()
        {
            onLateUpdate?.Invoke();
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

        public override void AddStateConfiguration(StateConfiguration configuration)
        {
            if(configuration is UpdateableStateConfiguration)
               base.AddStateConfiguration(configuration);
        }
    }
}
