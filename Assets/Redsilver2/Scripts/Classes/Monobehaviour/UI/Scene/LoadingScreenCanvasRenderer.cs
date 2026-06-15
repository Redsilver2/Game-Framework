using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework.Scenes.UI {
    public class LoadingScreenCanvasRenderer : LoadingScreenUI
    {
        private CanvasRenderer _renderer;

        protected override void Awake()
        {
            _renderer = GetComponent<CanvasRenderer>();
            base.Awake();
        }

        public sealed override bool IsTargetedAlpha(float alpha) {
            return _renderer != null ? _renderer.GetAlpha() == alpha : true;
        }

        public sealed override void SetAlpha(float alpha) {
            if (_renderer != null) { _renderer.SetAlpha(Mathf.Clamp01(alpha)); }
        }

        public sealed override float GetAlpha()
        {
            if(_renderer != null) return _renderer.GetAlpha();
            return 0f;
        }
    }
}
