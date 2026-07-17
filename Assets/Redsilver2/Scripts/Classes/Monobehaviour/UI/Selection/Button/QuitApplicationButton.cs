using UnityEngine;

namespace RedSilver2.Framework.UI {
    public class QuitApplicationButton : ButtonUISelection {
        protected override void Awake() {
            base.Awake();
            AddOnClickListener(() => { Application.Quit(); });
        }
    }
}
