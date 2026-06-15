namespace RedSilver2.Framework.Interactions.Actions {
    public sealed class PlayPreviousAudioAction : AudioInteractionAction
    {
        public PlayPreviousAudioAction(SelecteableAudioInteractionModule module, Interaction interaction) : base(module, interaction)
        {
            interaction?.AddOnInteractedListener(handler => { module?.PlayNext(); });
        }
    }
}
