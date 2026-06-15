using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.Interactions.Actions
{

    public sealed class PlayNextAudioAction : AudioInteractionAction
    {
        public PlayNextAudioAction(SelecteableAudioInteractionModule module, Interaction interaction) : base(module, interaction) {
            interaction?.AddOnInteractedListener(handler => { module?.PlayNext(); });
        }
    }
}
