using NUnit.Framework;
using RedSilver2.Framework.Quests.Progressions;
using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Quests.Datas
{
    [CreateAssetMenu(menuName = "Quest/Data", fileName = "New Quest Data")]
    public class QuestData : ScriptableObject
    {
        [SerializeField]                private string questName;
        [SerializeField][TextArea(3,3)] private string questDescription;

        [Space]
        [SerializeField] private bool isRepeatable;

        [Space]
        [SerializeField] private QuestProgressionData[] datas;

        private Quest quest;
        public Quest Quest => quest;

        public void Instantiate() {
            quest = new Quest(questName, questDescription, isRepeatable, GetProgressions());
        }

        private QuestProgression[] GetProgressions() {
            List<QuestProgression> results = new List<QuestProgression>();
            if (datas == null) return results.ToArray();
            
            foreach(QuestProgressionData data in datas) {
                QuestProgression progression = data != null ? data.GetProgression() : null;
                if (progression == null || results.Contains(progression)) continue;
                results?.Add(progression);
            }

            return results.ToArray();
        }
    }
}
