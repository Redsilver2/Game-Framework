using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace RedSilver2.Framework.UI
{
    public sealed class UISelectionTextColorUpdater : UISelectionColorUpdater
    {
        [Space]
        [SerializeField] private TextMeshProUGUI displayer;

        protected sealed override Color GetCurrentColor()
        {
            if (displayer == null) return Color.white;
            return displayer.color;
        }

        protected sealed override void UpdateColor(Color color)
        {
            if (displayer == null) return;
            displayer.color = color;
        }
    }
}
