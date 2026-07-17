using RedSilver2.Framework.Quests.Targets;
using UnityEngine;

namespace RedSilver2.Framework.Quests.Datas
{
    [CreateAssetMenu(menuName = "Quest/Target/UInt", fileName = "New UInt Quest Target Data")]
    public sealed class UIntQuestTargetData : QuestTargetData
    {
        [SerializeField] private UIntQuestTarget target;
        public sealed override QuestTarget GetTarget() { return target; }
        public UIntQuestTarget GetUIntTarget() { return target; }
    }
}