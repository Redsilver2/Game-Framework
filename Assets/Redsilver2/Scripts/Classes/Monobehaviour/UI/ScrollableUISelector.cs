using UnityEngine;
using UnityEngine.UI;


namespace RedSilver2.Framework.UI
{
    public sealed class ScrollableUISelector : UISelector
    {
        [Space]
        [SerializeField] private Transform scrollParent;

        [Space]
        [SerializeField] private Scrollbar scrollbar;

        [Space]
        [SerializeField] private Vector3 minPosition;
        [SerializeField] private Vector3 maxPosition;

        [Space]
        [SerializeField] private float positionUpdateSpeed;

        private Vector3 currentPosition;

        protected sealed override void Awake() {
            base.Awake();

            currentPosition = minPosition;
            if (scrollbar != null) scrollbar.onValueChanged.AddListener(Scroll);
        }

        protected sealed override void UpdateSelections()
        {
            base.UpdateSelections();

            if (GameUIController.GetNavigateDownState(true) || GameUIController.GetNavigateUpState(true)) {
                if(MaxVerticalIndex > 0 && scrollbar != null)
                   scrollbar.value = Mathf.Clamp01(VerticalIndex / MaxVerticalIndex);
            }

            if(scrollParent != null)
              scrollParent.localPosition = Vector3.Lerp(scrollParent.localPosition, currentPosition, Time.deltaTime * positionUpdateSpeed);
        }

        private void Scroll(float value)
        {
            Vector3 nextPosition;

            if(value >= 1f)       { nextPosition = maxPosition; }
            else if (value <= 0f) { nextPosition = minPosition; }
            else                  { nextPosition = Vector3.Lerp(minPosition, maxPosition, Mathf.Clamp01(value)); }

            currentPosition = nextPosition;
        }
    }
}
