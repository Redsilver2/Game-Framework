using RedSilver2.Framework.Quests.Progressions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Quests
{
    [System.Serializable]
    public class Quest
    {
        [SerializeField] private string name;
        [SerializeField] private string description;

        private bool isRepeatable;
        private QuestState state;

        public readonly QuestExpiration Expiration;
        private List<QuestProgression> progressions;

        public string Name                => name;
        public string Description         => GetFormattedDescription(description); 
        public QuestState State           => state;

        private UnityEvent onStarted, onUpdate;
        private UnityEvent<QuestState> onStateChanged;

        private UnityEvent<QuestProgression> onProgressionAdded;
        private UnityEvent<QuestProgression> onProgressionRemoved;


        private static readonly UnityEvent<Quest> onQuestStarted      = new UnityEvent<Quest>();
        private static readonly UnityEvent<Quest> onQuestStateChanged = new UnityEvent<Quest>();

        private static readonly UnityEvent<Quest, QuestProgression> onQuestProgressionUpdate  = new UnityEvent<Quest, QuestProgression>();
        private static readonly UnityEvent<Quest, QuestProgression> onQuestProgressionAdded   = new UnityEvent<Quest, QuestProgression>();
        private static readonly UnityEvent<Quest, QuestProgression> onQuestProgressionRemoved = new UnityEvent<Quest, QuestProgression>();

        private static readonly Dictionary<string, Quest> instances = new Dictionary<string, Quest>();

        public Quest()
        {
            this.name        = string.Empty;
            this.description = string.Empty;

            progressions = new List<QuestProgression>();
            SetDefaultEvents();

            this.state      = QuestState.Pending;
            this.Expiration = null;

            AddQuestInstance(this);        
        }

        public Quest(string name) {
            this.name = name;
            this.description = string.Empty;

            progressions = new List<QuestProgression>();
            SetDefaultEvents();

            this.state = QuestState.Pending;
            isRepeatable = false;

            this.Expiration = null;
            AddQuestInstance(this);
        }

        public Quest(string name, QuestExpiration expiration)
        {
            this.name = name;
            this.description = string.Empty;

            progressions = new List<QuestProgression>();
            SetDefaultEvents();

            this.state = QuestState.Pending;
            this.isRepeatable = false;

            this.Expiration = expiration;
            AddQuestInstance(this);
        }

        public Quest(string name, bool isRepeatable)
        {
            this.name = name;
            this.description = string.Empty;

            progressions = new List<QuestProgression>();
            SetDefaultEvents();

            this.state = QuestState.Pending;
            this.isRepeatable = isRepeatable;

            this.Expiration = null;
            AddQuestInstance(this);
        }


        public Quest(string name, bool isRepeatable, QuestExpiration expiration)
        {
            this.name = name;
            this.description = string.Empty;

            progressions = new List<QuestProgression>();
            SetDefaultEvents();

            this.state = QuestState.Pending;
            this.isRepeatable = isRepeatable;

            this.Expiration = expiration;
            AddQuestInstance(this);
        }





        public Quest(string name, string description) {
            this.name = name;
            this.description = description;

            progressions = new List<QuestProgression>();
            SetDefaultEvents();

            this.state   = QuestState.Pending;
            isRepeatable = false;

            this.Expiration = null;
            AddQuestInstance(this);
        }

        public Quest(string name, string description, QuestExpiration expiration)
        {
            this.name = name;
            this.description = description;

            progressions = new List<QuestProgression>();
            SetDefaultEvents();

            this.state = QuestState.Pending;
            this.isRepeatable = false;

            this.Expiration = expiration;
            AddQuestInstance(this);
        }

        public Quest(string name, string description, bool isRepeatable, QuestExpiration expiration)
        {
            this.name = name;
            this.description = description;

            progressions = new List<QuestProgression>();
            SetDefaultEvents();

            this.state = QuestState.Pending;
            this.isRepeatable = isRepeatable;

            this.Expiration = expiration;
            AddQuestInstance(this);
        }


        public Quest(string name, bool isRepeatable, List<QuestProgression> progressions)
        {
            this.name = name;
            this.description = string.Empty;

            this.progressions = progressions != null ? progressions : new List<QuestProgression>();
            SetDefaultEvents();

            this.state      = QuestState.Pending;
            this.Expiration = null;

            this.isRepeatable = false;
            AddQuestInstance(this);
        }

        public Quest(string name, bool isRepeatable, QuestExpiration expiration, List<QuestProgression> progressions)
        {
            this.name = name;
            this.description = string.Empty;

            this.progressions = progressions != null ? progressions : new List<QuestProgression>();
            SetDefaultEvents();

            this.state = QuestState.Pending;
            this.Expiration = expiration;

            this.isRepeatable = false;
            AddQuestInstance(this);
        }


        public Quest(string name, string description, List<QuestProgression> progressions)
        {
            this.name = name;
            this.description = description;

            this.progressions = progressions != null ? progressions : new List<QuestProgression>();
            SetDefaultEvents();

            this.state      = QuestState.Pending;
            this.Expiration = null;

            AddQuestInstance(this);
        }


        public Quest(string name, string description, QuestExpiration expiration, List<QuestProgression> progressions)
        {
            this.name = name;
            this.description = description;

            this.progressions = progressions != null ? progressions : new List<QuestProgression>();
            SetDefaultEvents();

            this.state = QuestState.Pending;
            this.isRepeatable = false;

            this.Expiration = expiration;
            AddQuestInstance(this);
        }

        public Quest(string name, string description, bool isRepeatable, List<QuestProgression> progressions)
        {
            this.name = name;
            this.description = description;

            this.progressions = progressions != null ? progressions : new List<QuestProgression>();
            SetDefaultEvents();

            this.state = QuestState.Pending;
            this.isRepeatable = isRepeatable;

            this.Expiration = null;
            AddQuestInstance(this);
        }

        public Quest(string name, string description, bool isRepeatable, QuestExpiration expiration, List<QuestProgression> progressions)
        {
            this.name = name;
            this.description = description;

            this.progressions = progressions != null ? progressions : new List<QuestProgression>();
            SetDefaultEvents();

            this.state        = QuestState.Pending;
            this.isRepeatable = isRepeatable;

            this.Expiration = expiration;
            AddQuestInstance(this);
        }


        public Quest(string name, string description, QuestProgression[] progressions)
        {
            this.name = name;
            this.description = description;

            this.progressions = progressions != null ? progressions.ToList() : new List<QuestProgression>();
            SetDefaultEvents();

            this.state = QuestState.Pending;
            this.isRepeatable = false;

            this.Expiration = null;
            AddQuestInstance(this);
        }

        public Quest(string name, string description, QuestExpiration expiration, QuestProgression[] progressions)
        {
            this.name = name;
            this.description = description;

            this.progressions = progressions != null ? progressions.ToList() : new List<QuestProgression>();
            SetDefaultEvents();

            this.state = QuestState.Pending;
            this.isRepeatable = false;

            this.Expiration = expiration;
            AddQuestInstance(this);
        }

        public Quest(string name, bool isRepeatable, QuestProgression[] progressions)
        {
            this.name        = name;
            this.description = string.Empty;

            this.progressions = progressions != null ? progressions.ToList() : new List<QuestProgression>();
            SetDefaultEvents();

            this.state = QuestState.Pending;
            this.isRepeatable = isRepeatable;

            this.Expiration = null;
            AddQuestInstance(this);
        }

        public Quest(string name, bool isRepeatable, QuestExpiration expiration, QuestProgression[] progressions)
        {
            this.name = name;
            this.description = string.Empty;

            this.progressions = progressions != null ? progressions.ToList() : new List<QuestProgression>();
            SetDefaultEvents();

            this.state = QuestState.Pending;
            this.isRepeatable = isRepeatable;

            this.Expiration = expiration;
            AddQuestInstance(this);
        }

        public Quest(string name, string description, bool isRepeatable, QuestProgression[] progressions)
        {
            this.name = name;
            this.description = description;

            this.progressions = progressions != null ? progressions.ToList() : new List<QuestProgression>();
            SetDefaultEvents();

            this.state = QuestState.Pending;
            this.isRepeatable = isRepeatable;

            this.Expiration = null;
            AddQuestInstance(this);
        }

        public Quest(string name, string description, bool isRepeatable, QuestExpiration expiration, QuestProgression[] progressions)
        {
            this.name = name;
            this.description = description;

            this.progressions = progressions != null ? progressions.ToList() : new List<QuestProgression>();
            SetDefaultEvents();

            this.state = QuestState.Pending;
            this.isRepeatable = isRepeatable;

            this.Expiration = expiration;
            AddQuestInstance(this);
        }

        private void SetDefaultEvents()
        {
            onStarted = new UnityEvent();
            onUpdate  = new UnityEvent();

            onStateChanged      = new UnityEvent<QuestState>();
            onProgressionAdded   = new UnityEvent<QuestProgression>();
           
            onProgressionRemoved = new UnityEvent<QuestProgression>();

            AddOnStartedlistener(() => {
                SetState(QuestState.Progressing);
                onQuestStarted?.Invoke(this); 
            });

            AddOnStateChangedlistener(OnStateChanged);
            AddOnProgressionAddedlistener(progression => {
                progression?.Reset();
                if (state == QuestState.Progressing) progression?.Enable();
                onQuestProgressionAdded?.Invoke(this, progression); 
            });

            AddOnProgressionRemovedlistener(progression => {
                progression?.Disable();
                onQuestProgressionRemoved?.Invoke(this, progression); 
            });
        }



        public void AddProgression(QuestProgression progression)
        {
            if (progression == null || progressions == null || progressions.Contains(progression))
                return;

            progressions?.Add(progression);
            onProgressionAdded?.Invoke(progression);
        }

        public void RemoveProgression(QuestProgression progression)
        {
            if (progression == null || progressions == null || !progressions.Contains(progression))
                return;

            progressions?.Remove(progression);
            onProgressionRemoved?.Invoke(progression);
        }

        protected virtual void OnStateChanged(QuestState state) {
            foreach (QuestProgression progression in progressions) {
                if (state == QuestState.Completed || state == QuestState.Completed) progression?.Disable();
                else if (state == QuestState.Progressing) progression?.Enable();
                else progression?.Reset();
            }

            onQuestStateChanged?.Invoke(this);
        }

        protected void SetState(QuestState state) {
            if(this.state != state) {
                this.state = state;
                onStateChanged?.Invoke(state);
            }
        }

        public void Start() {
            if (isRepeatable && (state == QuestState.Completed || state == QuestState.Failed))
                Reset();

            if (state == QuestState.Pending) onStarted?.Invoke();
        }

        public virtual void Update() {
            if (state != QuestState.Progressing) return;
            UpdateProgressions();
            UpdateExpiration(Expiration, this);
        }

        private void UpdateProgressions() {
            if (progressions == null) {
                SetState(QuestState.Pending);
                return;
            }
               
            bool isFailedQuest = false;

            foreach (QuestProgression progression in progressions) {
                if (progression == null) continue;
                (progression as UpdatableQuestProgression)?.Update();

                if (!progression.IsOptional && progression.State == QuestState.Failed) isFailedQuest = true;
                UpdateExpiration(progression.Expiration, progression);
            }

            if (isFailedQuest) SetState(QuestState.Failed);
            else if (GetProgress() >= 1f) SetState(QuestState.Completed);
        }

        private void  UpdateExpiration(QuestExpiration expiration) {
            if(expiration == null || state != QuestState.Progressing) return;
            expiration?.Update();
        }

        private void UpdateExpiration(QuestExpiration expiration, Quest quest) {
            if (expiration == null) return;
            UpdateExpiration(expiration);
            if(expiration.IsDone()) quest?.SetState(QuestState.Failed);
        }

        private void UpdateExpiration(QuestExpiration expiration, QuestProgression progression) {
            if (expiration == null) return;
            UpdateExpiration(expiration);
            if (expiration.IsDone()) progression?.Fail();
        }

        private void Reset() { 
            SetState(QuestState.Pending);
            Expiration?.Reset();
        }

        public float GetProgress()
        {
            int count = GetProgressionCount();
            return count <= 0f ? 1f : Mathf.Clamp01(GetProgressionCompletedCount()/count);
        }
        public bool ContainsProgression(QuestProgression progression)
        {
            if (progression == null || progressions == null) return false;
            return progressions.Contains(progression);
        }

        protected virtual string GetFormattedDescription(string description) {
            if (string.IsNullOrEmpty(description)) return string.Empty;
            return description;
        }

        public void AddOnStartedlistener(UnityAction action) {
            if (action != null) onStarted?.AddListener(action);
        }
        public void RemoveOnStartedlistener(UnityAction action){
            if (action != null) onStarted?.AddListener(action);
        }

        public void AddOnStateChangedlistener(UnityAction<QuestState> action)
        {
            if (action != null) onStateChanged?.AddListener(action);
        }
        public void RemoveOnStateChangedListener(UnityAction<QuestState> action)
        {
            if (action != null) onStateChanged?.RemoveListener(action);
        }

        public void AddOnProgressionAddedlistener(UnityAction<QuestProgression> action)
        {
            if (action != null) onProgressionAdded?.AddListener(action);
        }
        public void RemoveOnProgressionAddedlistener(UnityAction<QuestProgression> action)
        {
            if (action != null) onProgressionAdded?.RemoveListener(action);
        }

        public void AddOnProgressionRemovedlistener(UnityAction<QuestProgression> action)
        {
            if (action != null) onProgressionRemoved?.AddListener(action);
        }
        public void RemoveOnProgressionRemovedlistener(UnityAction<QuestProgression> action)
        {
            if (action != null) onProgressionRemoved?.RemoveListener(action);
        }

        public int GetProgressionCompletedCount() {
            if (progressions == null) return 0;
            return progressions.Where(x => x != null)
                        .Where(x => !x.IsOptional)
                        .Where(x => x.State == QuestState.Completed).Count();
        }

        public int GetProgressionCount() {
            if(progressions == null) return 0;
            return progressions.Where(x => x != null)
                               .Where(x => !x.IsOptional).Count();
        }

        public QuestProgression[] GetProgressions()
        {
            if (progressions == null) return new QuestProgression[0];
            return progressions.ToArray();
        }

        private static void AddQuestInstance(Quest quest)
        {
            if (quest == null) return;
            string name = quest.Name;

            if (!string.IsNullOrEmpty(name) && instances != null)
                if (!instances.ContainsKey(name.ToLower()))
                    instances?.Add(name.ToLower(), quest);
        }

        public static void AddOnQuestStartedListener(UnityAction<Quest> action)
        {
            if (action != null) onQuestStarted?.AddListener(action);
        }
        public static void RemoveOnQuestStartedListener(UnityAction<Quest> action)
        {
            if (action != null) onQuestStarted?.RemoveListener(action);
        }

        public static void AddOnQuestStateChangedListener(UnityAction<Quest> action)
        {
            if (action != null) onQuestStateChanged?.AddListener(action);
        }
        public static void RemoveOnQuestStateChangedListener(UnityAction<Quest> action)
        {
            if (action != null) onQuestStateChanged?.RemoveListener(action);
        }

        public static void AddOnQuestProgressionAddedListener(UnityAction<Quest, QuestProgression> action)
        {
            if (action != null) onQuestProgressionAdded?.AddListener(action);
        }
        public static void RemoveOnQuestProgressionAddedListener(UnityAction<Quest, QuestProgression> action) {
            if (action != null) onQuestProgressionAdded?.RemoveListener(action);
        }

        public static void AddOnQuestProgressionRemovedListener(UnityAction<Quest, QuestProgression> action)
        {
            if (action != null) onQuestProgressionRemoved?.AddListener(action);
        }
        public static void RemoveOnQuestProgressionRemovedListener(UnityAction<Quest, QuestProgression> action)
        {
            if (action != null) onQuestProgressionRemoved?.RemoveListener(action);
        }

        public static void TriggerProgression(string name)  {
            if (string.IsNullOrEmpty(name)) return;

            foreach(TriggerableQuestProgression progression in GetActifProgressions())
                progression?.Trigger(name);
        }

        public static Quest[] GetQuests() {
            if (instances == null) return new Quest[0];
            return instances.Values.ToArray();
        }

        public static Quest GetQuest(string questName) {
            if (string.IsNullOrEmpty(questName) || instances == null) return null;

            if (instances.ContainsKey(questName.ToLower()))
                return instances[questName.ToLower()];

            return null;
        }

        public static Quest[] GetQuests(List<string> questNames) {
            if(questNames == null) return new Quest[0];
            return GetQuests(questNames.ToArray());
        }


        public static Quest[] GetQuests(string[] questNames) {
            List<Quest> results = new List<Quest>();
            if(questNames == null) return results.ToArray();

            foreach(string questName in questNames) {
                Quest quest = GetQuest(questName);
                if (quest == null || results.Contains(quest)) continue;

                results?.Add(quest);
            }

            return results.ToArray();
        }

        public static Quest GetQuest(QuestProgression progression) {
            if (progression == null) return null;

            foreach (Quest quest in GetQuests()) {
                if (quest == null) continue;
                else if (quest.ContainsProgression(progression)) return quest;
            }

            return null;
        }

        public static Quest[] GetQuests(List<QuestProgression> progressions) {
            if(progressions == null) return new Quest[0];
            return GetQuests(progressions.ToArray());
        }

        public static Quest[] GetQuests(QuestProgression[] progressions) {
            List< Quest> results = new List<Quest>();
            if(progressions == null) return results.ToArray();

            foreach(QuestProgression progression in progressions) {
                Quest quest = GetQuest(progression);
                if (quest == null || results.Contains(quest)) continue;

                results?.Add(quest);
            }

            return results.ToArray();
        }

        public static Quest[] GetActifQuests() {
            List<Quest> results = new List<Quest>();

            foreach (Quest instance in GetQuests()) {
                if (instance == null || instance.State != QuestState.Progressing) continue;
                results?.Add(instance);
            }

            return results.ToArray();
        }

        private static TriggerableQuestProgression[] GetActifProgressions()
        {
            List<TriggerableQuestProgression> results = new List<TriggerableQuestProgression>();

            foreach (Quest objectif in GetActifQuests()) {
                if (objectif == null || objectif.progressions == null) continue;

                foreach (QuestProgression progression in objectif.progressions) {
                    TriggerableQuestProgression triggerable = progression as TriggerableQuestProgression;
                    if (triggerable == null) continue;
                    results?.Add(triggerable);
                }
            }

            return results.ToArray();
        }

        public static TriggerableQuestProgression[] GetTriggerableQuestProgressions(){
            List<TriggerableQuestProgression> results = new List<TriggerableQuestProgression>();

            foreach(Quest quest in GetQuests()) {
                if(quest == null) continue;

                foreach(QuestProgression progression in quest.GetProgressions()) {
                    TriggerableQuestProgression triggerable = progression as TriggerableQuestProgression;
                    if (triggerable == null || results.Contains(triggerable)) continue;
                    results?.Add(triggerable);
                }
            }

            return results.ToArray();
        }
    }
}
