using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Inputs
{
    public class Vector2InputAction : InputAction {
        
        private Vector2Input[] inputs;
        private UnityEvent<Vector2[]> onExecuted;

        public Vector2InputAction(string actionName, Vector2Input input) : base(actionName)
        {
            inputs    = new Vector2Input[1];
            inputs[0] = input;  

            AddOnEnabledListener(EnableInputs);
            AddOnDisabledListener(DisableInputs);
            AddOnExecutedListener(DefaultOnExecutedEvent);
        }

        public Vector2InputAction(string actionName, Vector2Input[] inputs) : base(actionName)
        {
            this.inputs = inputs;
            AddOnEnabledListener(EnableInputs);
            AddOnDisabledListener(DisableInputs);
        }

        public void AddOnExecutedListener(UnityAction<Vector2[]> action) {
            if(action != null) onExecuted?.AddListener(action);
        }

        public void RemoveOnExecutedListener(UnityAction<Vector2[]> action) {
            if (action != null) onExecuted?.RemoveListener(action);
        }

        private async void DefaultOnExecutedEvent() {
            if (inputs == null) return;
            List<Vector2> vectors = new List<Vector2>();

            await Awaitable.BackgroundThreadAsync();

            foreach (Vector2Input input in inputs.Where(x => x != null))
                vectors.Add(input.Value);

            await Awaitable.MainThreadAsync();

            onExecuted?.Invoke(vectors.ToArray());
        }


        private void EnableInputs()
        {
            if (inputs == null) return;

            foreach (Vector2Input input in inputs.Where(x => x != null))
                input.Enable();
        }

        private void DisableInputs()
        {
            if (inputs == null) return;
            foreach (Vector2Input input in inputs.Where(x => x != null))
                input.Disable();
        }
        protected sealed override bool CanExecute()
        {
            if (inputs == null || inputs.Length == 0) return false;
            return inputs.Where(x => x != null).Where(x => x.Value.magnitude > 0).Count() == inputs.Where(x => x != null).Count();
        }


    }
}
