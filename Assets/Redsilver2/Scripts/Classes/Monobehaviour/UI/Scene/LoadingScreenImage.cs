using UnityEngine;
using UnityEngine.UI;

namespace RedSilver2.Framework.Scenes.UI
{
    public class LoadingScreenImage : LoadingScreenUI
    {
        private Image image;

        protected override void Awake()
        {
            image = GetComponent<Image>();
            base.Awake();
        }

        public sealed override float GetAlpha()
        {
            if (image != null) return image.color.a;
            return 0f;
        }

        public sealed override bool IsTargetedAlpha(float alpha)
        {
            if (image != null) return image.color.a == alpha;
            return true;
        }

        public sealed override void SetAlpha(float alpha)
        {
            if (image != null) {
                var color = image.color;
                color.a = Mathf.Clamp01(alpha);
                image.color = color;
            }
        }
    }
}
