
namespace RedSilver2.Framework.Interactions.Actions
{
    public class DecreaseAudioVolumeAction   : AudioInteractionAction
    {
        public DecreaseAudioVolumeAction(AudioInteractionModule module, Interaction interaction) : base(module, interaction) {
            interaction?.AddOnInteractedListener(handler => { module?.DecreaseVolume(); });
        }
    }
}
