using UnityEngine;

namespace RedSilver2.Framework.UI {
    public sealed class ClickQuitApplicationEvent : UISelectionButtonOnClickEvent
    {
        protected sealed override void OnClick() { Application.Quit(); }
    }
}
