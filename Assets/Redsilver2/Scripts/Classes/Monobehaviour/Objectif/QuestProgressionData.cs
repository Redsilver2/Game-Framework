using RedSilver2.Framework.Quests.Progressions;
using UnityEngine;

namespace RedSilver2.Framework.Quests.Datas
{
    public abstract class QuestProgressionData : ScriptableObject {

        [SerializeField][TextArea(3,3)] private string description;

        [Space]
        [SerializeField] private bool isOptional;

        public QuestProgression GetProgression() { 
            return GetProgression(description, isOptional); 
        }

        protected abstract QuestProgression GetProgression(string description, bool isOptional);
    }
}
