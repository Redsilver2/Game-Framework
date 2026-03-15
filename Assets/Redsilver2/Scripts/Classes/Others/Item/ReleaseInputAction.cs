using System.Linq;

namespace RedSilver2.Framework.Inputs
{
    public class ReleaseInputAction : InputAction
    {
        private ReleaseInput[] inputs;

        public ReleaseInputAction(string actionName, ReleaseInput input) : base(actionName)
        {
            this.inputs = new ReleaseInput[1];
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

        public ReleaseInputAction(string actionName, ReleaseInput[] inputs) : base(actionName)
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


        public ReleaseInputAction(string actionName, float executionDelay, float holdTriggerTime, OverrideableReleaseInput input) : base(actionName)
        {
            this.inputs = new ReleaseInput[1];
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

        public ReleaseInputAction(string actionName, float executionDelay, float triggerTime, OverrideableReleaseInput[] inputs) : base(actionName)
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
            if(inputs == null || inputs.Length == 0) return false;
            foreach (var input in inputs) input?.Update();

            return inputs.Where(x => x.Value).Count() == inputs.Length;
        }
    }
}
