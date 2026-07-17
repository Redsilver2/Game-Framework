using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace RedSilver2.Framework.UI
{
    public abstract class UISelectionUpdater : UISelectionEvent
    {
        private IEnumerator updater;

        protected sealed override void SetEvents(UISelection selection, bool isAddingEvent)
        {
            if (isAddingEvent)
            {
                selection?.AddOnDeselectListener(OnDeSelected);
                selection?.AddOnSelectListener(OnSelected);
            }
            else
            {
                selection?.RemoveOnDeselectListener(OnDeSelected);
                selection?.RemoveOnSelectListener(OnSelected);
            }
        }

        private void OnSelected()   { StartSizeUpdate(true); }
        private void OnDeSelected() { StartSizeUpdate(false); }

        private void StartSizeUpdate(bool isSelected)
        {
            if (updater != null)
            {
                StopCoroutine(updater);
                updater = null;
            }

            updater = UpdateUISelection(isSelected);
            StartCoroutine(updater);
        }

        protected abstract IEnumerator UpdateUISelection(bool isSelected);
        
    }
}
