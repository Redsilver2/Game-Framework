using UnityEngine;

namespace RedSilver2.Framework.UI
{
    public sealed class ClickUpdateUISelectorEvent : UISelectionButtonOnClickEvent {
        [SerializeField] private UISelector selector;

        [Space]
        [SerializeField] private float activationTime;
        [SerializeField] private bool  canResetIndexes;


#if UNITY_EDITOR
        private void OnValidate() {
           activationTime = Mathf.Clamp(activationTime, 0f, float.MaxValue);   
        }
#endif


        protected override void OnClick()
        {
            if (canResetIndexes) selector?.ResetIndexes();
            selector?.UpdateSelector(activationTime);
        }



    }
}
