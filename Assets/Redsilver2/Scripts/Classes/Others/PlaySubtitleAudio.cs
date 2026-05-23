
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Actions
{
    public class PlaySubtitleAudio : PlayAudio
    {
        // Subtitle stuff over here

        protected override UnityAction<InteractionHandler> GetBaseEvent(AudioInteractionModule module)
        {
            var _base = base.GetBaseEvent(module);
            if (_base == null || module == null) return null;

            return handler => {
                // GameManager.SubtitleManager?.GetSubtitle();
                _base?.Invoke(handler);
                

            };
        }
    }
}
