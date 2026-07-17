using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Quests.Progressions
{
    [System.Serializable]
    public abstract class QuestProgression {
        [SerializeField] private string description;
        [SerializeField] private bool   isOptional;
        private QuestState state;

        public  readonly QuestExpiration Expiration;
        private readonly UnityEvent<QuestState> onStateChanged;

        public string     Description => GetFormattedDescription(description);

        public bool IsOptional        => isOptional;
        public QuestState State       => state;

        private static readonly UnityEvent<QuestProgression> onProgressionStateChanged = new UnityEvent<QuestProgression>();

        protected QuestProgression() {
            this.description = string.Empty;

            onStateChanged = new UnityEvent<QuestState>();
            this.state = QuestState.Pending;

            Expiration = null;
            isOptional = false;

    
            AddOnStateChangedListener(OnStateChanged);
        }

        protected QuestProgression(bool isOptional) {
            this.description = string.Empty;
            onStateChanged = new UnityEvent<QuestState>();

            this.state = QuestState.Pending;
            Expiration = null;

            this.isOptional = isOptional;
            AddOnStateChangedListener(OnStateChanged);
        }


        protected QuestProgression(bool isOptional, QuestExpiration expiration)
        {
            this.description = string.Empty;
            onStateChanged   = new UnityEvent<QuestState>();

            this.state = QuestState.Pending;
            Expiration = expiration;
          
            this.isOptional = isOptional;
            AddOnStateChangedListener(OnStateChanged);
        }

        protected QuestProgression(string description) {
            this.description = string.IsNullOrEmpty(description) ? string.Empty : description;

            onStateChanged = new UnityEvent<QuestState>();
            this.state     = QuestState.Pending;

            Expiration     = null;
            isOptional     = false;

            this.isOptional = false;
            AddOnStateChangedListener(OnStateChanged);
        }
        protected QuestProgression(string description, QuestExpiration expiration)
        {
            this.description = string.IsNullOrEmpty(description) ? string.Empty : description;
            onStateChanged   = new UnityEvent<QuestState>();

            this.state = QuestState.Pending;
            Expiration = expiration;
            
            isOptional = false;
            AddOnStateChangedListener(OnStateChanged);
        }

        protected QuestProgression(QuestExpiration expiration) {
            this.description = string.Empty;
            onStateChanged   = new UnityEvent<QuestState>();
          
            this.state   = QuestState.Pending;
            Expiration   = expiration;
           
            isOptional   = false;
            AddOnStateChangedListener(OnStateChanged);
        }

        protected QuestProgression(string description, bool isOptional)
        {
            this.description = string.IsNullOrEmpty(description) ? string.Empty : description;
            onStateChanged = new UnityEvent<QuestState>();
        
            this.state = QuestState.Pending;
            Expiration = null;

            this.isOptional = isOptional;
            AddOnStateChangedListener(OnStateChanged);
        }
        protected QuestProgression(string description, bool isOptional, QuestExpiration expiration) {
            this.description = string.IsNullOrEmpty(description) ? string.Empty : description;
            onStateChanged   = new UnityEvent<QuestState>();

            this.state       = QuestState.Pending;
            Expiration       = expiration;
          
            this.isOptional  = isOptional;
            AddOnStateChangedListener(OnStateChanged);
        }

        protected virtual void OnStateChanged(QuestState state) {
            onProgressionStateChanged?.Invoke(this);
        }

        public void Enable() {
            if (state == QuestState.Pending) { SetState(QuestState.Progressing); }
        }

        public void Disable() {
            if(state != QuestState.Pending) SetState(QuestState.Pending);
        }

        public virtual void Reset() { SetState(QuestState.Pending); }
        public void Fail() { SetState(QuestState.Failed); }


        protected void SetState(QuestState state){
            if (this.state != state) {
                this.state = state;
                onStateChanged?.Invoke(state);
            }
        }

        public void AddOnStateChangedListener(UnityAction<QuestState> action)
        {
            if (action != null) onStateChanged?.AddListener(action);
        }
        public void RemoveOnCompletedListener(UnityAction<QuestState> action) {
            if (action != null) onStateChanged?.RemoveListener(action);
        }


#if UNITY_EDITOR
        public abstract void Validate();
#endif

        protected abstract string GetFormattedDescription(string description);
        public abstract float GetProgress();
        public abstract bool IsActive();

        public static void AddOnProgressionCompletedListener(UnityAction<QuestProgression> action){
            if (action != null) onProgressionStateChanged?.AddListener(action);
        }
        public static void RemoveOnProgressionCompletedListener(UnityAction<QuestProgression> action) {
            if (action != null) onProgressionStateChanged?.RemoveListener(action);
        }
    }
}
