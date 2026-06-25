using UnityEngine;

namespace RedSilver2.Framework.Dialogs
{
    [CreateAssetMenu(fileName = "New Dialog Choice Info", menuName = "Dialog/Choice/Info")]
    public sealed class DialogChoiceInfo : ScriptableObject {
        [SerializeField] private string choiceName;
        [SerializeField] private string choiceDescription;

        [Space]
        [SerializeField] private DialogInfo nextDialogInfo;
        private DialogChoice choice = null;

        public DialogChoice GetChoice() {
            if (choice != null) return choice; 

            if (nextDialogInfo == null) {
                choice = new DialogChoice(choiceName, choiceDescription);
                return choice;
            }

            choice = new DialogChoice(choiceName, choiceDescription, nextDialogInfo.GetDialog());
            return choice;
        }
    }
}
