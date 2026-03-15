using RedSilver2.Framework.Inputs;
using System.Linq;

namespace RedSilver2.Framework.Inputs
{
    public sealed class PressInputAction : InputAction
    {
        private PressInput[] inputs;


        public PressInputAction(string actionName, PressInput input) : base(actionName)
        {
            this.inputs = new PressInput[1];
            this.inputs[0] = input;

            AddOnEnabledListener(() => {
                if (inputs == null) return;
                foreach (var input in inputs) input?.Enable();
            });


            AddOnDisabledListener(() => {
                if (inputs == null) return;
                foreach (var input in inputs) input?.Disable();
            });
        }

        public PressInputAction(string actionName, PressInput[] inputs) : base(actionName)
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


        public PressInputAction(string actionName, float executionDelay, OverrideablePressInput input) : base(actionName) {
            this.inputs    = new PressInput[1];
            this.inputs[0] = input;

            AddOnEnabledListener(() => {
                if (inputs == null) return;
                foreach(var input in inputs) input?.Enable();    
            });


            AddOnDisabledListener(() => {
                if (inputs == null) return;
                foreach (var input in inputs) input?.Disable();
            });
        }

        public PressInputAction(string actionName, float executionDelay, OverrideablePressInput[] inputs) : base(actionName)
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

        protected override bool CanExecute() {
            if (inputs == null || inputs.Length == 0) return false;
            foreach (var input in inputs) input?.Update();

            return inputs.Where(x => x.Value).Count() == inputs.Length;
        }
    }
}
