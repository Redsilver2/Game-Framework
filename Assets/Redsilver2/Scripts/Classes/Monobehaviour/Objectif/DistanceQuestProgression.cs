using RedSilver2.Framework.Quests.Targets;
using RedSilver2.Framework.StateMachines.Controllers;
using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Quests.Progressions
{
    [System.Serializable]
    public class DistanceQuestProgression : UpdatableQuestProgression
    {
        [SerializeField] private float reachDistance = 0f;
        [SerializeField] private DistanceProgressionType progressionType;

        private QuestTransformTarget[] targets;
        private Transform[] targetTransforms;

        public float ReachDistance => reachDistance;
        public DistanceProgressionType ProgressionType => progressionType;

        public DistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance) : base()
        {
            this.progressionType = DistanceProgressionType.ReachTarget;
            SetReachDistance(reachDistance);
            
            AddOnUpdateListener(OnUpdate);
            this.targets = targets != null ? targets : new QuestTransformTarget[0];
        }
        public DistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, DistanceProgressionType type) : base() {

            this.progressionType = type;
            SetReachDistance(reachDistance);
          
            AddOnUpdateListener(OnUpdate);
            this.targets = targets != null ? targets : new QuestTransformTarget[0];
        }

        public DistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, bool isOptional) : base(isOptional)
        {
            this.progressionType = DistanceProgressionType.ReachTarget;
            SetReachDistance(reachDistance);
          
            AddOnUpdateListener(OnUpdate);
            this.targets = targets != null ? targets : new QuestTransformTarget[0];
        }
        public DistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, DistanceProgressionType type, bool isOptional) : base(isOptional) {
            this.progressionType = type;
            SetReachDistance(reachDistance);
           
            AddOnUpdateListener(OnUpdate);
            this.targets = targets != null ? targets : new QuestTransformTarget[0];
        }

        public DistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, string description) : base()
        {
            this.progressionType = DistanceProgressionType.ReachTarget;
            SetReachDistance(reachDistance);
           
            AddOnUpdateListener(OnUpdate);
            this.targets = targets != null ? targets : new QuestTransformTarget[0];
        }
        public DistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, DistanceProgressionType type, string description) : base(description) {
            this.progressionType = type;
            SetReachDistance(reachDistance);
          
            AddOnUpdateListener(OnUpdate);
            this.targets = targets != null ? targets : new QuestTransformTarget[0];
        }

        public DistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, QuestExpiration expiration) : base(expiration)
        {
            this.progressionType = DistanceProgressionType.ReachTarget;
            SetReachDistance(reachDistance);
           
            AddOnUpdateListener(OnUpdate);
            this.targets = targets != null ? targets : new QuestTransformTarget[0];
        }
        public DistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, DistanceProgressionType type, QuestExpiration expiration) : base(expiration) {
            this.progressionType = type;
            SetReachDistance(reachDistance);
           
            AddOnUpdateListener(OnUpdate);
            this.targets = targets != null ? targets : new QuestTransformTarget[0];
        }

        public DistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, bool isOptional, QuestExpiration expiration) : base(isOptional, expiration)
        {
            this.progressionType = DistanceProgressionType.ReachTarget;
            SetReachDistance(reachDistance);
           
            AddOnUpdateListener(OnUpdate);
            this.targets = targets != null ? targets : new QuestTransformTarget[0];
        }
        public DistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, DistanceProgressionType type, bool isOptional, QuestExpiration expiration) : base(isOptional, expiration) {
            this.progressionType = type;
            SetReachDistance(reachDistance);
           
            AddOnUpdateListener(OnUpdate);
            this.targets = targets != null ? targets : new QuestTransformTarget[0];
        }

        public DistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, string description, QuestExpiration expiration) : base(description, expiration)
        {
            this.progressionType = DistanceProgressionType.ReachTarget;
            SetReachDistance(reachDistance);
           
            AddOnUpdateListener(OnUpdate);
            this.targets = targets != null ? targets : new QuestTransformTarget[0];
        }
        public DistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, DistanceProgressionType type, string description, QuestExpiration expiration) : base(description, expiration) {
            this.progressionType = type;
            SetReachDistance(reachDistance);
           
            AddOnUpdateListener(OnUpdate);
            this.targets = targets != null ? targets : new QuestTransformTarget[0];
        }

        public DistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, string description, bool isOptional) : base(description, isOptional)
        {
            this.progressionType = DistanceProgressionType.ReachTarget;
            SetReachDistance(reachDistance);
           
            AddOnUpdateListener(OnUpdate);
            this.targets = targets != null ? targets : new QuestTransformTarget[0];
        }
        public DistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, DistanceProgressionType type, string description, bool isOptional) : base(description, isOptional) {
            this.progressionType = type;
            SetReachDistance(reachDistance);
            
            AddOnUpdateListener(OnUpdate);
            this.targets = targets != null ? targets : new QuestTransformTarget[0];
        }

        public DistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, string description, bool isOptional, QuestExpiration expiration) : base(description, isOptional, expiration)
        {
            this.progressionType = DistanceProgressionType.ReachTarget;
            SetReachDistance(reachDistance);
           
            AddOnUpdateListener(OnUpdate);
            this.targets = targets != null ? targets : new QuestTransformTarget[0];
        }
        public DistanceQuestProgression(QuestTransformTarget[] targets, float reachDistance, DistanceProgressionType type, string description, bool isOptional, QuestExpiration expiration) : base(description, isOptional, expiration) {
            this.progressionType = type;
            SetReachDistance(reachDistance);
           
            AddOnUpdateListener(OnUpdate);
            this.targets = targets != null ? targets : new QuestTransformTarget[0];
        }

        public void SetReachDistance(float reachDistance) {
            this.reachDistance = Mathf.Clamp(reachDistance, 0f, float.MaxValue);
        }

        public void SetProgressionType(DistanceProgressionType type) {
            this.progressionType = type;
        }

        public override void Reset() {
            targetTransforms = null;
            base.Reset();
        }

        protected override void OnUpdate() {
            if (State != QuestState.Progressing) return;
            targetTransforms = GetTargets();
            base.OnUpdate();
        }

        protected Transform GetClosestTransformFromPlayer() {
            List<float> results = new List<float>();
            float closestDistance = float.MaxValue;
           
            Transform result = null;
            if (targetTransforms == null) return null;
        
            foreach (Transform transform in targetTransforms) {
                float distance = GetDistanceFromPlayer(transform);

                if(distance < closestDistance) {
                    closestDistance = distance;
                    result = transform;
                }
            }

            return result;
        }

        protected float GetDistanceFromPlayer(Transform transform) {
            PlayerController controller = PlayerController.Current;
            
            if(transform == null || controller == null) {
                if (progressionType == DistanceProgressionType.LeaveTarget) return float.MinValue;
                else                                                        return float.MaxValue;
            }

            return Vector3.Distance(transform.position, controller.transform.position);
        }

        protected float[] GetDistancesFromPlayer() {
            List<float> results = new List<float>();
            if (targetTransforms == null) return results.ToArray();
            
            foreach (Transform target in targetTransforms)
                results?.Add(GetDistanceFromPlayer(target));

            return results.ToArray(); 
        }

        protected bool IsDistanceValid() {
            float distance = GetDistanceFromPlayer(GetClosestTransformFromPlayer());

            if (distance <= reachDistance && progressionType == DistanceProgressionType.ReachTarget) return true;
            else if (distance >= reachDistance && progressionType == DistanceProgressionType.LeaveTarget) return true;

            return false;
        }

        private Transform[] GetTargets()
        {
            List<Transform> results = new List<Transform>();
            if (targets == null) return results.ToArray();

            foreach (QuestTransformTarget target in targets) {
                if (target == null) continue;

                foreach(Transform transform in target.GetTransforms()) {
                    if (transform == null) continue;
                    results?.Add(transform);
                }
            }

            return results.ToArray();
        }

#if UNITY_EDITOR
        public override void Validate() {
            reachDistance = Mathf.Clamp(reachDistance, 0f, float.MaxValue);
        }
#endif

        protected override string GetFormattedDescription(string description)
        {
           return description;
        }

        public override float GetProgress() {
            Debug.Log(IsDistanceValid());
            return IsDistanceValid() ? 1f : 0f;
        }

        public sealed override bool IsActive() { return true; }
    }
}
