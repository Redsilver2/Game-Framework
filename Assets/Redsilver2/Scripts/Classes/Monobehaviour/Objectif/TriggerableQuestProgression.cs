using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Quests.Progressions
{
    public abstract class TriggerableQuestProgression : QuestProgression
    {
        private UnityEvent<string> onTriggered;
        private static readonly UnityEvent<TriggerableQuestProgression> onProgressionTriggered = new UnityEvent<TriggerableQuestProgression>();

        protected TriggerableQuestProgression() : base() {
            onTriggered = new UnityEvent<string>();
            AddOnTriggeredListener(OnTriggered);
        }

        protected TriggerableQuestProgression(bool isOptional) : base(isOptional) {
            onTriggered = new UnityEvent<string>();
            AddOnTriggeredListener(OnTriggered);
        }

        protected TriggerableQuestProgression(string description) : base(description) {
            onTriggered = new UnityEvent<string>();
            AddOnTriggeredListener(OnTriggered);
        }

        protected TriggerableQuestProgression(QuestExpiration expiration) : base(expiration) {
            onTriggered = new UnityEvent<string>();
            AddOnTriggeredListener(OnTriggered);
        }

        protected TriggerableQuestProgression(bool isOptional, QuestExpiration expiration) : base(isOptional, expiration) {
            onTriggered = new UnityEvent<string>();
            AddOnTriggeredListener(OnTriggered);
        }

        protected TriggerableQuestProgression(string description, QuestExpiration expiration) : base(description, expiration) {
            onTriggered = new UnityEvent<string>();
            AddOnTriggeredListener(OnTriggered);
        }

        protected TriggerableQuestProgression(string description, bool isOptional) : base(description, isOptional) {
            onTriggered = new UnityEvent<string>();
            AddOnTriggeredListener(OnTriggered);
        }

        protected TriggerableQuestProgression(string description, bool isOptional, QuestExpiration expiration) : base(description, isOptional, expiration) {
            onTriggered = new UnityEvent<string>();
            AddOnTriggeredListener(OnTriggered);
        }

        public void Trigger(string targetName) 
        {
            if (IsTargetValid(targetName)) onTriggered?.Invoke(targetName);
        }

        protected virtual void OnTriggered(string targetName) {
            onProgressionTriggered?.Invoke(this);
            if(GetProgress() >= 1f) SetState(QuestState.Completed);
        }

        public void AddOnTriggeredListener(UnityAction<string> action)
        {
            if (action != null) onTriggered?.AddListener(action);
        }
        public void RemoveOnTriggeredListener(UnityAction<string> action)
        {
            if (action != null) onTriggered?.RemoveListener(action);
        }

        protected abstract bool IsTargetValid(string targetName);

        public static void AddOnProgressionTriggeredListener(UnityAction<TriggerableQuestProgression> action)
        {
            if (action != null) onProgressionTriggered?.AddListener(action);
        }
        public static void RemoveOnProgressionTriggeredListener(UnityAction<TriggerableQuestProgression> action)
        {
            if (action != null) onProgressionTriggered?.RemoveListener(action);
        }
    }
}
