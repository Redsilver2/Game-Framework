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
        private int previousVerticalIndex;

        protected sealed override void Awake() {
            base.Awake();

            currentPosition       = minPosition;
            previousVerticalIndex = -1;

            if (scrollbar != null) scrollbar.onValueChanged.AddListener(Scroll);
        }


        public override void ResetIndexes()
        {
            base.ResetIndexes();
            previousVerticalIndex = -1;
        }
       

        protected sealed override void UpdateSelections()
        {
            base.UpdateSelections();
            int currentVeriticalIndex = (int)VerticalIndex;


            if (previousVerticalIndex != currentVeriticalIndex) {
                if (MaxVerticalIndex > 0) {
                    float progress = Mathf.Clamp01((float)VerticalIndex / (float)MaxVerticalIndex);
                    if (scrollbar != null) scrollbar.value = progress;
                    else Scroll(progress);
                }

                previousVerticalIndex = currentVeriticalIndex;
            }

            if (scrollParent != null)
              scrollParent.localPosition = Vector3.Lerp(scrollParent.localPosition, currentPosition, Time.deltaTime * positionUpdateSpeed);
        }

        private void Scroll(float value)
        {
            Debug.Log(value);
            currentPosition = Vector3.Lerp(minPosition, maxPosition, Mathf.Clamp01(value));
        }
    }
}
