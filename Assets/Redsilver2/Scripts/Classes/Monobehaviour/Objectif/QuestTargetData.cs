using RedSilver2.Framework.Quests.Targets;
using UnityEngine;

namespace RedSilver2.Framework.Quests.Datas
{
    public abstract class QuestTargetData : ScriptableObject  {
        public abstract QuestTarget GetTarget();
    }
}
