using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace RedSilver2.Framework.UI
{
    public class UISelectionTextColorUpdater : UISelectionUpdater
    {
        [SerializeField] private float colorUpdateDuration = 0.1f;

        [Space]
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color deselectedColor;

        [Space]
        [SerializeField] private TextMeshProUGUI displayer;

        protected sealed override IEnumerator UpdateUISelection(bool isSelected)
        {
            yield return StartCoroutine(UpdateColor(isSelected ? selectedColor : deselectedColor));
        }

        private IEnumerator UpdateColor(Color color)
        {
            Color currentColor = displayer != null ? displayer.color : Color.white;
            float t = 0f;

            while (true)
            {
                float progress = Mathf.Clamp01(t / colorUpdateDuration);

                if (displayer != null)
                    displayer.color = progress < 1f ? Color.Lerp(currentColor, color, progress) : color;

                if (progress >= 1f) break;
                t += Time.deltaTime;
                yield return null;
            }
        }
    }
}
