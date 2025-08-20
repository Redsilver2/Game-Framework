using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.Scenes.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public abstract class LoadingTextMeshProUGUI : LoadingProgressUI
    {
        private TextMeshProUGUI displayer;

        protected void Awake()
        {
            displayer = GetComponent<TextMeshProUGUI>();
        }

        protected void SetText(string text)
        {
            if (displayer != null) displayer.text = text;
        }

    }
}
