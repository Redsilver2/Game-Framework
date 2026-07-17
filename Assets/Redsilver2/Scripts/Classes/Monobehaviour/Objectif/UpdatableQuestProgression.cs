using UnityEngine.Events;

namespace RedSilver2.Framework.Quests.Progressions {
    public abstract class UpdatableQuestProgression : QuestProgression
    {
        private readonly UnityEvent onUpdate;
        private static readonly UnityEvent<UpdatableQuestProgression> onProgressionUpdate = new UnityEvent<UpdatableQuestProgression>();

        protected UpdatableQuestProgression() : base() {
            onUpdate = new UnityEvent();
            AddOnUpdateListener(OnUpdate);
        }

        protected UpdatableQuestProgression(bool isOptional) : base(isOptional) {
            onUpdate = new UnityEvent();
            AddOnUpdateListener(OnUpdate);
        }

        protected UpdatableQuestProgression(bool isOptional, QuestExpiration expiration) : base(isOptional, expiration) {
            onUpdate = new UnityEvent();
            AddOnUpdateListener(OnUpdate);
        }

        protected UpdatableQuestProgression(string description) : base(description) {
            onUpdate = new UnityEvent();
            AddOnUpdateListener(OnUpdate);
        }


        protected UpdatableQuestProgression(QuestExpiration expiration) : base(expiration)
        {
            onUpdate = new UnityEvent();
            AddOnUpdateListener(OnUpdate);
        }

        protected UpdatableQuestProgression(string description, QuestExpiration expiration) : base(description, expiration) {
            onUpdate = new UnityEvent();
            AddOnUpdateListener(OnUpdate);
        }

        protected UpdatableQuestProgression(string[] targets, QuestExpiration expiration) : base(expiration) {
            onUpdate = new UnityEvent();
            AddOnUpdateListener(OnUpdate);
        }

        protected UpdatableQuestProgression(string description, bool isOptional) : base(description,  isOptional) {
            onUpdate = new UnityEvent();
            AddOnUpdateListener(OnUpdate);
        }

        protected UpdatableQuestProgression(string description, bool isOptional, QuestExpiration expiration) : base(description, isOptional, expiration) {
            onUpdate = new UnityEvent();
            AddOnUpdateListener(OnUpdate);  
        }

        public override void Reset() {
            base.Reset();
            onUpdate?.Invoke();
        }

        public void Update() { onUpdate?.Invoke(); }
       
        public void AddOnUpdateListener(UnityAction action) {
            if (action != null) onUpdate?.AddListener(action);
        }
        public void RemoveOnUpdateListener(UnityAction action) {
            if (action != null) onUpdate?.RemoveListener(action);
        }

        protected virtual void OnUpdate() {
            if (State != QuestState.Progressing) return;
            else if (GetProgress() >= 1f) SetState(QuestState.Completed);
            
            onProgressionUpdate?.Invoke(this); 

           
        }

        public static void AddOnProgressionUpdateListener(UnityAction<UpdatableQuestProgression> action) {
            if (action != null) onProgressionUpdate?.AddListener(action);
        }
        public static void RemoveOnProgressionUpdateListener(UnityAction<UpdatableQuestProgression> action) {
            if (action != null) onProgressionUpdate?.RemoveListener(action);
        }
    }
}
