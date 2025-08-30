using UnityEngine.UI;
using UnityEngine;

namespace RedSilver2.Framework.Scenes.UI
{
    [RequireComponent(typeof(Text))]
    public abstract class LoadingText : LoadingProgressUI
    {
        private Text displayer;

        protected void Awake()
        {
            displayer = GetComponent<Text>();   
        }

        public void SetText(string text)
        {
            if(displayer != null) displayer.text = text;
        }
    }
}
