using UnityEngine;
using UnityEngine.UI;

namespace RedSilver2.Framework.UI {
    public sealed class UISelectionImageColorUpdater : UISelectionColorUpdater {
        [Space]
        [SerializeField] private Image image;

        protected sealed override Color GetCurrentColor()
        {
            if(image == null) return Color.white;
            return image.color;
        }

        protected sealed override void UpdateColor(Color color)
        {
            if (image == null) return;
            image.color = color;
        }
    }
}
