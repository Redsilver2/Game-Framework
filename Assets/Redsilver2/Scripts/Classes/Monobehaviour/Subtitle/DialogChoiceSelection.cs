namespace RedSilver2.Framework.Dialogs
{
    public abstract class DialogChoiceSelection : DialogChoiceEventHandler {
        private DialogChoiceHandler selected;
        private int selectedIndex;
       
        protected override void SetDefaultEvents(DialogChoiceManager manager, bool isAddingEvents) {
            if (isAddingEvents) {
                manager?.AddOnChoiceSelectedListener(OnChoiceSelected);
                manager?.AddOnHandlersUpdateListener(OnHandlersUpdate);
            }
            else {
                manager?.RemoveOnChoiceSelectedListener(OnChoiceSelected);
                manager?.RemoveOnHandlersUpdateListener(OnHandlersUpdate);
            }
        }

        private void OnChoiceSelected(DialogChoiceHandler handler) {
            this.selected = handler;
            selectedIndex = -1;
        }

        protected virtual void OnHandlersUpdate(DialogChoiceHandler[] handlers)
        {
            if (handlers == null) return;
            int leftIndex = 0, rightIndex = 0;
            for(int i = 0; i < handlers.Length; i++) OnHandlersUpdate(handlers[i], i, ref leftIndex, ref rightIndex);
        }

        private void OnHandlersUpdate(DialogChoiceHandler handler, int currentIndex, ref int leftIndex, ref int rightIndex) {
            if (handler == null) return;
            else if (handler == selected) {
                selectedIndex = currentIndex;
                UpdateSelected(handler, currentIndex);
            }
            else OnHandlersUpdate(handler, currentIndex, IsSeperatingArray(), ref leftIndex, ref rightIndex);
        }


        private void OnHandlersUpdate(DialogChoiceHandler handler, int currentIndex, bool isSeperatingArray, ref int leftIndex, ref int rightIndex) {
            if(handler == null) return;

            if (isSeperatingArray) {
                if (selectedIndex == -1 || currentIndex < selectedIndex) {
                    UpdateLeftDeselected(handler, currentIndex, leftIndex);
                    leftIndex++;
                }
                else if (currentIndex > selectedIndex) {
                    UpdateRightDeselected(handler, currentIndex, rightIndex);
                    rightIndex++;
                }
            }
            else UpdateDeselected(handler, currentIndex);
        }

        protected bool IsSeperatingArray() {
            return selected == null ? false : true;
        }

        protected bool IsSelectedIndex(int index) { return index == selectedIndex; }
        protected bool IsSelected(DialogChoiceHandler handler) { 
            if(handler == null || selected == null) return false;
            return selected.Equals(handler); 
        }

        protected bool IsLeftArrayElement(int index)
        {
            if(selectedIndex == 1) return true;
            return index < selectedIndex; 
        }

        protected abstract void UpdateSelected(DialogChoiceHandler handler, int index);
        protected abstract void UpdateLeftDeselected(DialogChoiceHandler handler, int currentIndex, int seperatedArrayIndex);
        protected abstract void UpdateRightDeselected(DialogChoiceHandler handler, int currentIndex, int seperatedArrayIndex);
        protected abstract void UpdateDeselected(DialogChoiceHandler handler, int index);
       
    }
}
