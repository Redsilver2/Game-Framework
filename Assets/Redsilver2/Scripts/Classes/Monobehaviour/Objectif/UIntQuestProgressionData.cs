using RedSilver2.Framework.Quests.Progressions;
using RedSilver2.Framework.Quests.Targets;
using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Quests.Datas
{
    [CreateAssetMenu(menuName = "Quest/Progression Data/UInt", fileName = "New UInt Quest Progression Data")]
    public sealed class UIntQuestProgressionData : QuestProgressionData
    {
        [Space]
        [SerializeField][Range(1, 9999)] private uint maxCount;

        [Space]
        [SerializeField] private UIntQuestTargetData[] targetDatas;

        protected sealed override QuestProgression GetProgression(string description, bool isOptional) {
            return new UIntQuestProgression(GetTargets(), maxCount, description, isOptional);
        }

        private UIntQuestTarget[] GetTargets()
        {
            List<UIntQuestTarget> results = new List<UIntQuestTarget>();
            if (targetDatas == null) return results.ToArray();

            foreach(UIntQuestTargetData targetData in targetDatas) {
                UIntQuestTarget target = targetData != null ? targetData.GetUIntTarget() : null;
                if(target == null || results.Contains(target)) continue;

                results?.Add(target);
            }

            return results.ToArray();
        }
    }
}
