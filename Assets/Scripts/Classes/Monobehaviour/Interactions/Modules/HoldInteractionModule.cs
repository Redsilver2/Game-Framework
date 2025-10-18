namespace RedSilver2.Framework.Interactions
{
    public class HoldInteractionModule : SingleInteractionModule
    {
        public override void Interact(InteractionHandler handler) {

            if(handler != null && enabled)
                if (handler.IsInputHeld)
                    Interact();
        }
    }
}
