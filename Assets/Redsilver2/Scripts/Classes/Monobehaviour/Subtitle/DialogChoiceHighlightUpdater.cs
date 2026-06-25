using System;
using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.Dialogs
{
    public sealed class DialogChoiceHighlightUpdater : DialogChoiceSelection {
        [SerializeField] private Color deselectedColor;
        [SerializeField] private Color selectedColor;

        [Space]
        [SerializeField] private float colorUpdateSpeed;

        protected sealed override void UpdateDeselected(DialogChoiceHandler handler, int index) {
            UpdateColor(handler, deselectedColor);
        }

        protected sealed override void UpdateSelected(DialogChoiceHandler handler, int index)
        {
            UpdateColor(handler, selectedColor);
        }

        protected sealed override void UpdateLeftDeselected(DialogChoiceHandler handler, int currentIndex, int seperatedArrayIndex) {
            UpdateDeselected(handler, currentIndex);
        }

        protected sealed override void UpdateRightDeselected(DialogChoiceHandler handler, int currentIndex, int seperatedArrayIndex) {
            UpdateDeselected(handler, currentIndex);
        }


        private void UpdateColor(DialogChoiceHandler handler, Color color) {
            if(handler == null) return;
            UpdateColor(handler.TextDisplayer, color);
        }

        private void UpdateColor(TextMeshProUGUI displayer, Color color) {
            if (displayer == null) return;
            displayer.color = Color.Lerp(displayer.color, color, Time.deltaTime * colorUpdateSpeed);
        }
    }
}
