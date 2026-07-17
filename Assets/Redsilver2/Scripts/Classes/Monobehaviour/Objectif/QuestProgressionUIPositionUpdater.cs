using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.Quests.UI
{
    public class QuestProgressionUIPositionUpdater : QuestUIUpdater
    {
        [SerializeField] private float defaultSpacing;
        [SerializeField] private float updateSpeed;

        protected override void SetEvents(QuestUIHandler handler, bool isAddingEvents) {
            if (isAddingEvents) handler?.AddOnUpdateProgressionUIHandlers(UpdateHandlers);
            else handler?.RemoveOnUpdateProgressionUIHandlers(UpdateHandlers);
        }

        private void UpdateHandlers(QuestProgressionUIHandler[] handlers)
        {
            if (handlers == null) return;
            Vector3 nextPosition = transform.position + Vector3.down * defaultSpacing;

            for(int i = 0; i < handlers.Length; i++) {
                QuestProgressionUIHandler handler = handlers[i];
                if (handler == null) continue;

                UpdateHandler(handler, ref nextPosition);
            }
        }

        private void UpdateHandler(QuestProgressionUIHandler handler, ref Vector3 nextPosition) {
            if(handler == null) return;
            handler.transform.position = Vector3.Lerp(handler.transform.position, nextPosition, Time.deltaTime * updateSpeed);

            TextMeshProUGUI displayer = handler.DescriptionDisplayer;
            if(displayer == null) return;

            nextPosition += Vector3.down * displayer.textInfo.lineCount * defaultSpacing;
        }
    }
}
