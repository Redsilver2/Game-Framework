using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Actions
{
    [CreateAssetMenu(fileName = "New Play Audio Interaction Action", menuName = "Interaction/Audio/Play (Default)")]
    public class PlayAudio : AudioInteractionAction
    {
        [SerializeField] private AudioClip clip;

        public sealed override string GetInteractionName() {
            return "Play";
        }

        protected override UnityAction<InteractionHandler> GetBaseEvent(AudioInteractionModule module)
        {
            if(module == null) return null;
            return handler => {
                module?.Play(clip);
            };
        }
    }
}
