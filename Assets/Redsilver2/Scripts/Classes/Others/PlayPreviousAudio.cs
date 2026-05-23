using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Actions
{
    [CreateAssetMenu(fileName = "New Play Previous Audio Interaction Action", menuName = "Interaction/Audio/Selecteable/Play (Previous)")]
    public sealed class PlayPreviousAudio : SelecteableAudioInteractionAction
    {
        public sealed override string GetInteractionName()
        {
            return "Play Previous";
        }

        protected override UnityAction<InteractionHandler> GetBaseEvent(SelecteableAudioInteractionModule module)
        {
            if(module == null) return null;
            return handle => { module?.PlayPrevious(); };
        }
    }
}
