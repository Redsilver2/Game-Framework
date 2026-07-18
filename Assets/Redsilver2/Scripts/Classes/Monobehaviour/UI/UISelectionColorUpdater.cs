using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework.UI
{
    public abstract class UISelectionColorUpdater : UISelectionUpdater
    {
        [SerializeField] private float colorUpdateDuration = 0.1f;

        [Space]
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color deselectedColor;

        protected override IEnumerator UpdateUISelection(bool isSelected) {
            Color nextColor    = isSelected ? selectedColor : deselectedColor;
            Color currentColor = GetCurrentColor();
            float t = 0f;

            while(t < colorUpdateDuration) {
                UpdateColor(Color.Lerp(currentColor, nextColor, Mathf.Clamp01(t/colorUpdateDuration)));
                t += Time.deltaTime;
                yield return null;
            }

            UpdateColor(nextColor);
        }


        protected abstract void UpdateColor(Color color);
        protected abstract Color GetCurrentColor();
    }
}
