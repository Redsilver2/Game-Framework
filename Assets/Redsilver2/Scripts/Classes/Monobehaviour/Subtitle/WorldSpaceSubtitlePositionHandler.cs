using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.Dialogs {
    public sealed class WorldSpaceSubtitlePositionHandler : SubtitlePositionHandler
    {
        [Space]
        [SerializeField] private float subtitleLookSpeed;

        protected override void UpdateSubtitleHandler(SubtitleHandler handler, int index, ref float previousHeight){
            PlayerController current = PlayerController.Current;
            if (handler == null || current == null) return;

             
            base.UpdateSubtitleHandler(handler, index, ref previousHeight);
        }

        protected sealed override Transform GetParent(SubtitleHandler handler) {
            return handler == null ? null : handler.Parent;
        }

        protected sealed override void SetDefaultEvents(DialogManager manager, bool isAddingEvents) {
            if (isAddingEvents) manager?.AddOnWorldSpaceSubtitleUpdateListener(UpdateSubtitleHandlers);
            else                manager?.RemoveOnWorldSpaceSubtitleUpdateListener(UpdateSubtitleHandlers);
        }
    }
}
