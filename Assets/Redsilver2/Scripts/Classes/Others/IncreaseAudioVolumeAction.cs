namespace RedSilver2.Framework.Interactions.Actions
{
    public class IncreaseAudioVolumeAction : AudioInteractionAction {
        public IncreaseAudioVolumeAction(AudioInteractionModule module, Interaction interaction) : base(module, interaction) {
            interaction?.AddOnInteractedListener(handler => { module?.IncreaseVolume(); });
        }
    }
}
