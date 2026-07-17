using UnityEngine;

namespace RedSilver2.Framework.Quests.Targets {
    [System.Serializable]
    public abstract class QuestTarget
    {
        [SerializeField] private string targetName;
        public string TargetName => targetName;

        protected QuestTarget(string targetName) { this.targetName = targetName.ToLower(); }
        public bool Validate(string targetName) {
            if (string.IsNullOrEmpty(targetName) || string.IsNullOrEmpty(this.targetName))
                return false;

            return this.targetName.ToLower().Equals(targetName.ToLower());
        }
    }
}
