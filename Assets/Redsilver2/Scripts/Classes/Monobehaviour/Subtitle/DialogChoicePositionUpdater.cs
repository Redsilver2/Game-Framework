using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Dialogs
{
    public abstract class DialogChoicePositionUpdater : DialogChoiceSelection
    {

        [Space]
        [SerializeField] private Vector3 defaultPosition;

        [Space]
        [SerializeField] private float positionUpdateSpeed;

        private Dictionary<DialogChoiceHandler, Vector3> handlersPositions;
        private bool isHandlersPositionsSet = false;

        protected virtual void Awake()
        {
            handlersPositions = new Dictionary<DialogChoiceHandler, Vector3>();
            isHandlersPositionsSet = false;
        }

        protected override void SetDefaultEvents(DialogChoiceManager manager, bool isAddingEvents)
        {
            base.SetDefaultEvents(manager, isAddingEvents);

            if (isAddingEvents) { manager?.AddOnChoiceUpdateFinishedListener(OnChoiceUpdateFinished); }
            else {
                manager?.RemoveOnChoiceUpdateFinishedListener(OnChoiceUpdateFinished);
                isHandlersPositionsSet = false;
            }
        }

        private void OnChoiceUpdateFinished(DialogChoice[] choices)
        {
            isHandlersPositionsSet = false;
        }

        protected sealed override void OnHandlersUpdate(DialogChoiceHandler[] handlers)
        {
            SetHandlerPositions(handlers);
            base.OnHandlersUpdate(handlers);
        }

        private void SetHandlerPositions(DialogChoiceHandler[] handlers)
        {
            if (handlers == null || handlersPositions == null || isHandlersPositionsSet) return;
            handlersPositions?.Clear();

            foreach (var handler in handlers) {
                if (handlersPositions.ContainsKey(handler) || handler == null) continue;
                handlersPositions?.Add(handler, Vector3.zero);
            }

            isHandlersPositionsSet = true;
        }

        protected sealed override void UpdateDeselected(DialogChoiceHandler handler, int index)
        {
            if (handler == null || handlersPositions == null || !handlersPositions.ContainsKey(handler)) return;
            else if (index == 0) { handlersPositions[handler] = defaultPosition; }

            UpdateNextHandlerPosition(index + 1);
            UpdatePosition(handler);
        }

        protected sealed override void UpdateSelected(DialogChoiceHandler handler, int index)
        {
            if (handler == null || handlersPositions == null || !handlersPositions.ContainsKey(handler)) return;
            else if (index == 0) { handlersPositions[handler] = defaultPosition; }

            UpdateNextHandlerPosition(index + 1);
            UpdatePosition(handler);
        }

        protected sealed override void UpdateLeftDeselected(DialogChoiceHandler handler, int currentIndex, int seperatedArrayIndex) {
            UpdateDeselected(handler, currentIndex);
        }

        protected sealed override void UpdateRightDeselected(DialogChoiceHandler handler, int currentIndex, int seperatedArrayIndex) {
            UpdateDeselected(handler, currentIndex);
        }

        protected void UpdatePosition(DialogChoiceHandler handler) {
            if (handler == null || handlersPositions == null || !handlersPositions.ContainsKey(handler)) return;
            UpdatePosition(handler.transform, handlersPositions[handler]);
        }

        private void UpdatePosition(Transform transform, Vector3 position)
        {
            if (transform == null) return;
            transform.localPosition = Vector3.Lerp(transform.localPosition, position, Time.deltaTime * positionUpdateSpeed);
        }

        private void UpdateNextHandlerPosition(int index) {
            UpdateNextHandlerPosition(GetHandler(index), index);
        }

        protected virtual void UpdateNextHandlerPosition(DialogChoiceHandler handler, int index) {
            if (handler == null) return;
            SetCurrentPosition(handler, GetCurrentPosition(GetHandler(index - 1)));
        }


        protected void SetCurrentPosition(DialogChoiceHandler handler, Vector3 position)
        {
            if (handler == null || handlersPositions == null || !handlersPositions.ContainsKey(handler))
                return;

            handlersPositions[handler] = position;
        }



        protected Vector3 GetCurrentPosition(DialogChoiceHandler handler)
        {
            if (handler == null || handlersPositions == null || !handlersPositions.ContainsKey(handler))
                return defaultPosition;

            return handlersPositions[handler];
        }


        protected DialogChoiceHandler GetHandler(int index)
        {
            if (handlersPositions == null) return null;
            var handlers = handlersPositions.Keys.ToArray();

            if (handlers == null || handlers.Length <= 0 || index < 0 || index >= handlers.Length)
                return null;

            return handlers[index];
        }

    }
}
