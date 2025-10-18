namespace RedSilver2.Framework.Interactions
{
    public class ReleaseInteractionModule : SingleInteractionModule
    {
        public sealed override void Interact(InteractionHandler handler)
        {
            if(handler != null && enabled)
            {
                if (handler.IsInputReleased) {
                    Interact();
                }
            }
        }
    }
}
