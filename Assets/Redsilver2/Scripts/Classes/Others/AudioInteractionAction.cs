using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Actions
{
    public abstract class AudioInteractionAction : InteractionAction
    {
        protected AudioInteractionAction(AudioInteractionModule module, Interaction interaction) : base(module, interaction) {

        }
    }
}
