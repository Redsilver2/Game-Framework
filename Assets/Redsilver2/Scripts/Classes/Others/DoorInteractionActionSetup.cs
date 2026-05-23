using UnityEngine;

namespace RedSilver2.Framework.Interactions.Actions.Setups
{
    public abstract class DoorInteractionActionSetup : InteractionActionSetup
    {
        protected sealed override void SetInteractionModule(InteractionModule module)
        {
            SetInteractionModule(module as Door);
        }

        protected abstract void SetInteractionModule(Door door);

    }
}
