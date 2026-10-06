using UnityEngine;

namespace RedSilver2.Framework.Subtitles
{
    public class ScreenSpaceSubtitlePositionHandler : SubtitlePositionHandler
    {
        [Space]
        [SerializeField] private Transform parent;

        protected sealed override void UpdateSubtitleHandler(SubtitleDisplayer handler, int index, ref float previousHeight)
        {
            if (handler == null) return;
            handler.transform.localRotation = Quaternion.identity;
            base.UpdateSubtitleHandler(handler, index, ref previousHeight);
        }

        protected sealed override Transform GetParent(SubtitleDisplayer handler) {
            return parent;
        }

        protected sealed override void SetDefaultEvents(SubtitleManager manager, bool isAddingEvents)
        {
       
        }
    }
}
