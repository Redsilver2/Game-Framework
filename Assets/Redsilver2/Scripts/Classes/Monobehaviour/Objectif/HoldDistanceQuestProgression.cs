using RedSilver2.Framework.Quests.Targets;
using UnityEngine;

namespace RedSilver2.Framework.Quests.Progressions
{
    [System.Serializable]
    public class HoldDistanceQuestProgression : DistanceQuestProgression
    {
        [SerializeField] private float distanceHoldDuration;
        private float currentDistanceHoldDuration;

        private bool canResetProgress;

        public HoldDistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance) : base(targets, reachDistance)
        {
            currentDistanceHoldDuration = 0f;
        }

        public HoldDistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, DistanceProgressionType type) : base(targets, reachDistance, type)
        {
            currentDistanceHoldDuration = 0f;
        }

        public HoldDistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, bool isOptional) : base(targets, reachDistance, isOptional)
        {
            currentDistanceHoldDuration = 0f;
        }

        public HoldDistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, DistanceProgressionType type, bool isOptional) : base(targets, reachDistance, type, isOptional)
        {
            currentDistanceHoldDuration = 0f;
        }

        public HoldDistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, string description) : base(targets, reachDistance, description)
        {
            currentDistanceHoldDuration = 0f;
        }

        public HoldDistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, DistanceProgressionType type, string description) : base(targets, reachDistance, type, description)
        {
            currentDistanceHoldDuration = 0f;
        }

        public HoldDistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, QuestExpiration expiration) : base(targets, reachDistance, expiration)
        {
            currentDistanceHoldDuration = 0f;
        }

        public HoldDistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, DistanceProgressionType type, QuestExpiration expiration) : base(targets, reachDistance, type, expiration)
        {
            currentDistanceHoldDuration = 0f;
        }

        public HoldDistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, bool isOptional, QuestExpiration expiration) : base(targets, reachDistance, isOptional, expiration)
        {
            currentDistanceHoldDuration = 0f;
        }

        public HoldDistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, DistanceProgressionType type, bool isOptional, QuestExpiration expiration) : base(targets, reachDistance, type, isOptional, expiration)
        {
            currentDistanceHoldDuration = 0f;
        }

        public HoldDistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, string description, QuestExpiration expiration) : base(targets, reachDistance, description, expiration)
        {
            currentDistanceHoldDuration = 0f;
        }

        public HoldDistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, DistanceProgressionType type, string description, QuestExpiration expiration) : base(targets, reachDistance, type, description, expiration)
        {
            currentDistanceHoldDuration = 0f;
        }

        public HoldDistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, string description, bool isOptional) : base(targets, reachDistance, description, isOptional)
        {
            currentDistanceHoldDuration = 0f;
        }

        public HoldDistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, DistanceProgressionType type, string description, bool isOptional) : base(targets, reachDistance, type, description, isOptional)
        {
            currentDistanceHoldDuration = 0f;
        }

        public HoldDistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, string description, bool isOptional, QuestExpiration expiration) : base(targets, reachDistance, description, isOptional, expiration)
        {
            currentDistanceHoldDuration = 0f;
        }

        public HoldDistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, DistanceProgressionType type, string description, bool isOptional, QuestExpiration expiration) : base(targets, reachDistance, type, description, isOptional, expiration)
        {
            currentDistanceHoldDuration = 0f;
        }

        public float DistanceHoldDuration => distanceHoldDuration;



        public void SetDistanceHoldDuration(float value) { distanceHoldDuration = Mathf.Clamp(value, 0f, float.MaxValue); }
        public void SetCanResetProgress(bool canResetProgress) { this.canResetProgress = canResetProgress; }

        public override void Validate()
        {
            distanceHoldDuration = Mathf.Clamp(distanceHoldDuration, Mathf.Epsilon, float.MaxValue);
            base.Validate();
        }

        public override void Reset() {
            currentDistanceHoldDuration = 0f;
            base.Reset();
        }

        protected override void OnUpdate()
        {
            if (State != QuestState.Progressing) return;

            if (IsDistanceValid())     { currentDistanceHoldDuration += Time.deltaTime; }
            else if (canResetProgress) { currentDistanceHoldDuration -= Time.deltaTime;  }

            currentDistanceHoldDuration = Mathf.Clamp(currentDistanceHoldDuration, 0f, distanceHoldDuration);
            base.OnUpdate();
        }

        public override float GetProgress()
        {
            if(distanceHoldDuration <= 0f) return 1f;
            return Mathf.Clamp01(currentDistanceHoldDuration / distanceHoldDuration);
        }
    }
}
