using System.Linq;
using UnityEngine;


namespace RedSilver2.Framework.Inputs
{
    public class HoldInputAction : InputAction
    {
        private HoldInput[] inputs;

        public HoldInputAction(string actionName, HoldInput input) : base(actionName)
        {
            this.inputs = new HoldInput[1];
            this.inputs[0] = input;

            AddOnEnabledListener(() => {
                if (inputs == null) return;
                foreach (var input in inputs)
                    input?.Enable();
            });

            AddOnDisabledListener(() => {
                if (inputs == null) return;

                foreach (var input in inputs)
                    input?.Disable();
            });
        }

        public HoldInputAction(string actionName, HoldInput[] inputs) : base(actionName)
        {
            this.inputs = inputs;

            AddOnEnabledListener(() => {
                if (inputs == null) return;
                foreach (var input in inputs) input?.Enable();
            });


            AddOnDisabledListener(() => {
                if (inputs == null) return;
                foreach (var input in inputs) input?.Disable();
            });
        }

        public HoldInputAction(string actionName, OverrideableHoldInput input) : base(actionName)
        {
            this.inputs       = new HoldInput[1];
            this.inputs[0]    = input;

            AddOnEnabledListener(() => {
                if (inputs == null) return;
                foreach (var input in inputs)
                    input?.Enable();
            });

            AddOnDisabledListener(() => {
                if (inputs == null) return;

                foreach (var input in inputs)
                    input?.Disable();
            });
        }

        public HoldInputAction(string actionName, OverrideableHoldInput[] inputs) : base(actionName)
        {
            this.inputs = inputs;

            AddOnEnabledListener(() => {
                if (inputs == null) return;
                foreach (var input in inputs) input?.Enable();
            });


            AddOnDisabledListener(() => {
                if (inputs == null) return;
                foreach (var input in inputs) input?.Disable();
            });
        }

        protected override bool CanExecute()
        {
            if(!IsEnabled || inputs == null || inputs.Length == 0) return false;
            foreach (var input in inputs) input?.Update();

            return inputs.Where(x => x.Value).Count() == inputs.Length;
        }
    }
}
