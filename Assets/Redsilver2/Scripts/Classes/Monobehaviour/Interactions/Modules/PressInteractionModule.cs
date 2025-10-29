namespace RedSilver2.Framework.Interactions
{
    public class PressInteractionModule : SingleInteractionModule
    {
        public sealed override void Interact(InteractionHandler handler)
        {
            if(handler != null && enabled)
                if (handler.IsInputPressed)
                    Interact();
        }
    }
}
