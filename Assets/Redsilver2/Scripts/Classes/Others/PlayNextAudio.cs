using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.Interactions.Actions
{
    [CreateAssetMenu(fileName = "New Play Next Audio Interaction Action", menuName = "Interaction/Audio/Selecteable/Play (Next)")]
    public sealed class PlayNextAudio : SelecteableAudioInteractionAction
    {
        public sealed override string GetInteractionName() {
            return "Play Next";
        }

        protected sealed override UnityAction<InteractionHandler> GetBaseEvent(SelecteableAudioInteractionModule module)
        {
            if(module == null) return null;
            return handler => { module?.PlayNext(); };
        }
    }
}
