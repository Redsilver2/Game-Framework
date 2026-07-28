using RedSilver2.Framework.Interactions;
using UnityEngine.Events;

namespace RedSilver2.Framework.Items
{
    public class TakeItemAction : ItemAction
    {
        protected override void SetInteractionEvent(Item item, Interaction interaction, bool isAddingEvent)
        {
            base.SetInteractionEvent(item, interaction, isAddingEvent);

            if (item == null) return;
            else if (isAddingEvent) {
                item?.AddOnAddedListener(OnAdded(item));
                item?.AddOnRemovedListener(OnRemoved(item));
                if (!item.IsInInventory()) item?.AddInteractionAction(this);
            }
            else {
                item?.RemoveOnAddedListener(OnAdded(item));
                item?.RemoveOnRemovedListener(OnRemoved(item));
                item?.RemoveInteractionAction(this);
            }
        }


        public sealed override UnityAction<InteractionHandler> GetOnInteractedEvent(Item item) {
            return handler => {
                if (item != null && handler != null) {       
                    if (!item.IsInInventory()) item?.Take(handler.Inventory);
                }
            };
        }

        private UnityAction OnAdded(Item item) {
            return () => {
                item?.RemoveInteractionAction(this);
            };
        }

        private UnityAction OnRemoved(Item item) {
            return () => {
                item?.AddInteractionAction(this);
            };
        }
    }
}