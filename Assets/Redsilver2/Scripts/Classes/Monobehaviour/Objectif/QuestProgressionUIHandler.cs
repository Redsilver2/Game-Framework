using RedSilver2.Framework.Quests.Progressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Quests.UI {
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class QuestProgressionUIHandler : MonoBehaviour {
        private TextMeshProUGUI descriptionDisplayer;
        private QuestProgression owner;

        public TextMeshProUGUI DescriptionDisplayer => descriptionDisplayer;
        public QuestProgression Owner => owner;

        private UnityEvent onUpdate;

        private void Awake() 
        { 
            onUpdate = new UnityEvent();
            descriptionDisplayer = GetComponent<TextMeshProUGUI>();
            AddOnUpdateListener(OnUpdate);
        }

        public void SetOwner(QuestProgression progression) {
            this.owner = progression;
        }

        public void AddOnUpdateListener(UnityAction action)
        {
            if (action != null) onUpdate?.AddListener(action);
        }
        public void RemoveOnUpdateListener(UnityAction action) {
            if (action != null) onUpdate?.RemoveListener(action);
        }

        public void UpdateHandler() { onUpdate?.Invoke();  }

        private void OnUpdate() {
            if (owner == null || descriptionDisplayer == null) return;
            descriptionDisplayer.text = owner.Description;
            descriptionDisplayer.color = Color.Lerp(descriptionDisplayer.color, GetColorFromState(), Time.deltaTime * 10f);
        }

        public Color GetColorFromState()
        {
            if(owner == null) return Color.white;
            return GetColorFromState(owner.State);
        }
        private Color GetColorFromState(QuestState state) {
            if      (state == QuestState.Failed) return Color.red;
            else if (state == QuestState.Completed) return Color.green;
            return Color.white;
        }
    }
}
