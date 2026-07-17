using UnityEngine;

namespace RedSilver2.Framework.Quests.Targets.Triggers
{
    public class QuestTriggerEnter : UIntQuestTargetTrigger
    {
        private void OnTriggerEnter(Collider other) {
            if (other.tag.ToLower().Equals("player"))
                Trigger(1, true);
        }
        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.tag.ToLower().Equals("player"))
                Trigger(1, true);
        }
    }
}