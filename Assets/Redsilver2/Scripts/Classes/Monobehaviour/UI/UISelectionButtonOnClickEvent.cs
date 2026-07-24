using RedSilver2.Framework.UI;

namespace RedSilver2.Framework.UI
{

    public abstract class UISelectionButtonOnClickEvent : UISelectionButtonEvent
    {
        protected sealed override void SetEvents(UISelectionButton selection, bool isAddingEvent) {
            if (isAddingEvent) selection?.AddOnClickListener(OnClick);
            else selection?.RemoveOnClickListener(OnClick);
        }
        protected abstract void OnClick();
    }
}
