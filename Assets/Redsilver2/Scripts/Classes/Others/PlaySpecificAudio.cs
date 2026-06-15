using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Actions {
    public class PlaySpecificAudio : AudioInteractionAction
    {
        [Space]
        [SerializeField] private uint clipIndex;

        public PlaySpecificAudio(SelecteableAudioInteractionModule module, Interaction interaction) : base(module, interaction) {
            interaction?.AddOnInteractedListener(handler => { module?.Play(clipIndex); });
        }
    }
}
