using UnityEngine;

namespace RedSilver2.Framework.Interactions.Actions.Setups {
    public abstract class AudioInteractionActionSetup : InteractionActionSetup
    {
        protected sealed override void SetInteractionModule(InteractionModule module) {
            SetInteractionModule(module as AudioInteractionModule);
        }

        protected abstract void SetInteractionModule(AudioInteractionModule module);
    }
}
