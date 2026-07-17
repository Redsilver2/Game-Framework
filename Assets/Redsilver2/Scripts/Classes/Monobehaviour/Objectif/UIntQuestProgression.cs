using RedSilver2.Framework.Quests.Targets;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Quests.Progressions
{
    [System.Serializable]
    public class UIntQuestProgression : TriggerableQuestProgression
    {
        [SerializeField] private uint maxCount;
        private uint currentCount;
        private readonly UIntQuestTarget[] targets;

        public uint MaxCount => maxCount;
        public uint CurrentCount => currentCount;

        public UIntQuestProgression(UIntQuestTarget[] targets) : base()
        {
            currentCount = 0;
            maxCount = 1;

            this.targets = targets != null ? targets : new UIntQuestTarget[0];
        }
        public UIntQuestProgression(UIntQuestTarget[] targets, uint maxCount) : base()
        {
            currentCount = 0;
            this.maxCount = (uint)Mathf.Clamp(maxCount, 1, uint.MaxValue);

            this.targets = targets != null ? targets : new UIntQuestTarget[0];
        }
       
        public UIntQuestProgression(UIntQuestTarget[] targets, bool isOptional) : base(isOptional)
        {
            currentCount = 0;
            maxCount = 1;

            this.targets = targets != null ? targets : new UIntQuestTarget[0];
        }
        public UIntQuestProgression(UIntQuestTarget[] targets, uint maxCount, bool isOptional) : base(isOptional)
        {
            currentCount = 0;
            this.maxCount = (uint)Mathf.Clamp(maxCount, 1, uint.MaxValue);

            this.targets = targets != null ? targets : new UIntQuestTarget[0];
        }

        public UIntQuestProgression(UIntQuestTarget[] targets, string description) : base(description)
        {
            currentCount = 0;
            this.maxCount = 1;

            this.targets = targets != null ? targets : new UIntQuestTarget[0];
        }
        public UIntQuestProgression(UIntQuestTarget[] targets, uint maxCount, string description) : base(description)
        {
            currentCount = 0;
            this.maxCount = (uint)Mathf.Clamp(maxCount, 1, uint.MaxValue);

            this.targets = targets != null ? targets : new UIntQuestTarget[0];
        }

        public UIntQuestProgression(UIntQuestTarget[] targets, QuestExpiration expiration) : base(expiration)
        {
            currentCount = 0;
            this.maxCount = 1;

            this.targets = targets != null ? targets : new UIntQuestTarget[0];
        }
        public UIntQuestProgression(UIntQuestTarget[] targets, uint maxCount, QuestExpiration expiration) : base(expiration)
        {
            currentCount = 0;
            this.maxCount = (uint)Mathf.Clamp(maxCount, 1, uint.MaxValue);

            this.targets = targets != null ? targets : new UIntQuestTarget[0];
        }

        public UIntQuestProgression(UIntQuestTarget[] targets, bool isOptional, QuestExpiration expiration) : base(isOptional, expiration)
        {
            currentCount = 0;
            this.maxCount = 1;

            this.targets = targets != null ? targets : new UIntQuestTarget[0];
        }
        public UIntQuestProgression(UIntQuestTarget[] targets, uint maxCount, bool isOptional, QuestExpiration expiration) : base(isOptional, expiration)
        {
            currentCount = 0;
            this.maxCount = (uint)Mathf.Clamp(maxCount, 1, uint.MaxValue);


            this.targets = targets != null ? targets : new UIntQuestTarget[0];
        }

        public UIntQuestProgression(UIntQuestTarget[] targets, string description, QuestExpiration expiration) : base(description, expiration)
        {
            currentCount = 0;
            this.maxCount = 1;
            this.targets = targets != null ? targets : new UIntQuestTarget[0];
        }
        public UIntQuestProgression(UIntQuestTarget[] targets, uint maxCount, string description, QuestExpiration expiration) : base(description, expiration)
        {
            currentCount = 0; 
            this.maxCount = (uint)Mathf.Clamp(maxCount, 1, uint.MaxValue);

            this.targets = targets != null ? targets : new UIntQuestTarget[0];
        }

        public UIntQuestProgression(UIntQuestTarget[] targets, string description, bool isOptional) : base(description)
        {
            this.currentCount = 0;
            this.maxCount = 1;


            this.targets = targets != null ? targets : new UIntQuestTarget[0];
        }
        public UIntQuestProgression(UIntQuestTarget[] targets, uint maxCount, string description, bool isOptional) : base(description, isOptional)
        {
            currentCount = 0;
            this.maxCount = (uint)Mathf.Clamp(maxCount, 1, uint.MaxValue);
            this.targets = targets != null ? targets : new UIntQuestTarget[0];
        }

        public UIntQuestProgression(UIntQuestTarget[] targets, string description, bool isOptional, QuestExpiration expiration) : base(description, isOptional, expiration)
        {
            currentCount = 0;
            this.maxCount = 1;


            this.targets = targets != null ? targets : new UIntQuestTarget[0];
        }
        public UIntQuestProgression(UIntQuestTarget[] targets, uint maxCount, string description, bool isOptional, QuestExpiration expiration) : base(description, isOptional, expiration)
        {
            currentCount = 0;
            this.maxCount = (uint)Mathf.Clamp(maxCount, 1, uint.MaxValue);

            this.targets = targets != null ? targets : new UIntQuestTarget[0];
        }

        public void SetMaxCount(int maxCount) {
            this.maxCount = (uint)Mathf.Clamp(maxCount, 1, uint.MaxValue);
        }

        public override void Reset() {
            currentCount = 0;
            base.Reset();
        }

        protected override void OnTriggered(string targetName) {
            GetTarget(targetName)?.Update(ref currentCount, maxCount);
            base.OnTriggered(targetName);
        }

        public override float GetProgress() {
            if (maxCount <= 0) return 1f;
            return Mathf.Clamp01(currentCount/maxCount);
        }

        public override bool IsActive() { return true; }

#if UNITY_EDITOR
        public override void Validate() {
            this.maxCount = (uint)Mathf.Clamp(maxCount, 1, uint.MaxValue);
        }
#endif

        protected override string GetFormattedDescription(string description) {
            if(string.IsNullOrEmpty(description)) return string.Empty;
            return description.Replace("{count}"   , currentCount.ToString() , System.StringComparison.OrdinalIgnoreCase)
                              .Replace("{maxcount}", maxCount.ToString()     , System.StringComparison.OrdinalIgnoreCase);
        }

        private UIntQuestTarget GetTarget(string targetName) {
            if (targets == null) return null;

            foreach (UIntQuestTarget target in targets) {
                if (target == null || !target.Validate(targetName)) continue;
                return target;
            }

            return null;
        }

        protected override bool IsTargetValid(string targetName) {
              if(targets == null) return false;
              
              foreach(UIntQuestTarget target in targets) {
                 if (target == null || !target.Validate(targetName)) continue;
                 return true;
              }

              return false;
        }
    }
}
