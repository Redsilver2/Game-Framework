namespace RedSilver2.Framework.Interactions.Actions {
    public class StopAudioAction : AudioInteractionAction
    {
        public StopAudioAction(AudioInteractionModule module, Interaction interaction) : base(module, interaction)
        {
            interaction?.AddOnInteractedListener(handler => { module?.Stop(); });
        }
    }
}
