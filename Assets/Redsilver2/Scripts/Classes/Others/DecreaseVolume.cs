using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Actions
{
    [CreateAssetMenu(fileName = "New Decrease Volume Audio Interaction Action", menuName = "Interaction/Audio/Decrease Volume")]
    public class DecreaseVolume : AudioInteractionAction
    {
        public sealed override string GetInteractionName()
        {
            return "Decrease Volume";
        }

        protected sealed override UnityAction<InteractionHandler> GetBaseEvent(AudioInteractionModule module)
        {
            if (module == null) return null;
            return handler => { module?.DecreaseVolume(); };        
        }
    }
}
