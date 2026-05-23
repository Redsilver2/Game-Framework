using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Actions
{
    [CreateAssetMenu(fileName = "New Stop Audio", menuName = "Interaction/Audio/Stop")]
    public class StopAudio : AudioInteractionAction
    {
        public sealed override string GetInteractionName() {
            return "Stop";
        }

        protected sealed override UnityAction<InteractionHandler> GetBaseEvent(AudioInteractionModule module)
        {
            if (module == null) return null;
            return handler => { module?.Stop(); };
        }
    }
}
