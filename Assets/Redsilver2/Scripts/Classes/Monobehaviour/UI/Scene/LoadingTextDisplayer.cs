using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.Scenes.UI
{
    public abstract class LoadingTextDisplayer : LoadingScreenCanvasRenderer
    {
        private TextMeshProUGUI displayer;

        protected override void Awake()
        {
            displayer = GetComponent<TextMeshProUGUI>();
            base.Awake();
        }
        public void SetText(string text)
        {
            if (displayer != null) displayer.text = text;
        }
    }
}
