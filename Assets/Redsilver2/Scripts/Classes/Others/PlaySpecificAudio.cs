using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Actions {
    [CreateAssetMenu(fileName = "New Play Specific Audio Interaction Action", menuName = "Interaction/Audio/Selecteable/Play (Specific)")]
    public class PlaySpecificAudio : SelecteableAudioInteractionAction
    {
        [Space]
        [SerializeField] private int clipIndex;
        [SerializeField] private string clipName;

        public sealed override string GetInteractionName() {
            return "Play " + clipName;
        }

        protected override UnityAction<InteractionHandler> GetBaseEvent(SelecteableAudioInteractionModule module) {
            if (module == null) return null;
            return handler => { module?.Play(clipIndex); };
        }
    }
}
