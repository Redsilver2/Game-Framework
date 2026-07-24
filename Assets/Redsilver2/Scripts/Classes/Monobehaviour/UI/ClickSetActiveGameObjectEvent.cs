using UnityEngine;

namespace RedSilver2.Framework.UI
{
    public class ClickSetActiveGameObjectEvent : UISelectionButtonOnClickEvent
    {
        [SerializeField] private GameObject target;
        [SerializeField] private bool isEnabled;
        protected sealed override void OnClick() { target?.SetActive(isEnabled); }
    }
}
