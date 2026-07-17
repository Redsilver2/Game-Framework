using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Quests.Targets
{
    [System.Serializable]
    public abstract class ValueQuestTarget : QuestTarget {
        [SerializeField] private bool isIncrementing;
        public bool IsIncrementing => isIncrementing;

        protected ValueQuestTarget(string targetName) : base(targetName) {
            this.isIncrementing = true;
        }

        protected ValueQuestTarget(string targetName, bool isIncrementing) : base(targetName) {
            this.isIncrementing = isIncrementing;
        }

        public void SetIsIncrementing(bool isIncrementing) {
            this.isIncrementing = isIncrementing;
        }
    }
}
