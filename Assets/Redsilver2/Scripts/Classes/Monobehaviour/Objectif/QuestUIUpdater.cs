using UnityEngine;

namespace RedSilver2.Framework.Quests.UI
{
    public abstract class QuestUIUpdater : MonoBehaviour
    {
        private QuestUIHandler handler;

        private void Awake() {
            handler = GetComponent<QuestUIHandler>();
        }
        private void Start() { SetEvents(handler, true); }

        private void OnEnable() { SetEvents(handler, true); }
        private void OnDisable() { SetEvents(handler, false); }

        protected abstract void SetEvents(QuestUIHandler handler, bool isAddingEvents);

    }
}