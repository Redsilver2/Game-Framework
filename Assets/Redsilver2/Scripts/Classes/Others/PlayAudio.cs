using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Actions
{
    [System.Serializable]
    public class PlayAudio : AudioInteractionAction
    {
        [SerializeField] private AudioClip clip;

        public PlayAudio(AudioInteractionModule module, Interaction interaction) : base(module, interaction) {
            interaction?.AddOnInteractedListener(handler => {
                module?.Play(clip);
            });
        }
    }
}
