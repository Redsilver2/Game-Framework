using UnityEngine;

namespace RedSilver2.Framework.Dialogs
{
    public class ScreenSpaceSubtitlePositionHandler : SubtitlePositionHandler
    {
        [Space]
        [SerializeField] private Transform parent;

        protected sealed override void UpdateSubtitleHandler(SubtitleHandler handler, int index, ref float previousHeight)
        {
            if (handler == null) return;
            handler.transform.localRotation = Quaternion.identity;
            base.UpdateSubtitleHandler(handler, index, ref previousHeight);
        }

        protected sealed override Transform GetParent(SubtitleHandler handler) {
            return parent;
        }

        protected sealed override void SetDefaultEvents(DialogManager manager, bool isAddingEvents)
        {
            if (isAddingEvents) { manager?.AddOnScreenSpaceSubtitleUpdateListener(UpdateSubtitleHandlers); }
            else                { manager?.RemoveOnScreenSpaceSubtitleUpdateListener(UpdateSubtitleHandlers); }
        }
    }
}
