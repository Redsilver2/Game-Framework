using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Actions
{
    [CreateAssetMenu(fileName = "New Increase Volume Audio Interaction Action", menuName = "Interaction/Audio/Increase Volume")]
    public class IncreaseVolume : AudioInteractionAction
    {
        public sealed override string GetInteractionName()
        {
            return "Increase Volume";
        }

        protected sealed override UnityAction<InteractionHandler> GetBaseEvent(AudioInteractionModule module)
        {
            if (module == null) return null;
            return handler => { module?.IncreaseVolume(); };
        }
    }
}
