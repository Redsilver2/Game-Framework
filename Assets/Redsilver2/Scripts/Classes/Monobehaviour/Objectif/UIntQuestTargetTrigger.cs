using RedSilver2.Framework.Quests.Datas;
using UnityEngine;

namespace RedSilver2.Framework.Quests.Targets.Triggers {
    public abstract class UIntQuestTargetTrigger : MonoBehaviour {
        [SerializeField] private UIntQuestTargetData data;
        protected void Trigger(uint value, bool isIncrementing) {
            UIntQuestTarget target = data != null ? data.GetUIntTarget() : null;
            target?.SetIsIncrementing(isIncrementing);
            target?.SetValue(value);

            Debug.Log(target);
            if (target != null) Quest.TriggerProgression(target.TargetName);
        }
    }
}
