using UnityEngine.Events;

namespace RedSilver2.Framework.Dialogs {
    public sealed class DialogChoice {
        public  readonly string Name;
        public  readonly string Description;

        private readonly Dialog     dialog;
        private readonly UnityEvent onChoosed;

        public DialogChoice(string name, string description) {
            this.Name = name.ToUpper();
            this.Description = description;

            this.onChoosed = new UnityEvent();
            AddOnChoosedListener(() => { GameManager.DialogManager?.Play(this.dialog); });
        }

        public DialogChoice(string name, string description, Dialog dialog)
        {
            this.Name = name.ToUpper();
            this.Description = description;

            this.dialog = dialog;
            this.onChoosed = new UnityEvent();

            AddOnChoosedListener(() => { GameManager.DialogManager?.Play(this.dialog); });
        }

        public void AddOnChoosedListener(UnityAction action) {
            if(action != null) onChoosed?.AddListener(action);  
        }
        public void RemoveOnChoosedListener(UnityAction action)
        {
            if (action != null) onChoosed?.AddListener(action);
        }
        public void Choose() { onChoosed?.Invoke(); }
    }
}
