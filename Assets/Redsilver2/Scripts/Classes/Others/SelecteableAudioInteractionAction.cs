using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Actions
{
    public abstract class SelecteableAudioInteractionAction : AudioInteractionAction
    {
        protected sealed override UnityAction<InteractionHandler> GetBaseEvent(AudioInteractionModule module) {
            return GetBaseEvent(module as SelecteableAudioInteractionModule);
        }

        protected abstract UnityAction<InteractionHandler> GetBaseEvent(SelecteableAudioInteractionModule module);
    }
}
