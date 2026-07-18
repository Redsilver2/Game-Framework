using System;
using UnityEngine;

namespace RedSilver2.Framework.UI
{
    public abstract class FullScreenModeSettingUI : UIntSettingUI
    {
        private readonly FullScreenMode[] fullScreenModes = Enum.GetValues(typeof(FullScreenMode)) as FullScreenMode[];
        private const string FULLSCREEN_MODE_KEY = "FULLSCREEN_MODE";
        public override void Apply()
        {
            base.Apply();
            Screen.fullScreenMode = GetFullscreenMode();
        }

        protected FullScreenMode GetFullscreenMode()
        {
            if (fullScreenModes == null) return FullScreenMode.ExclusiveFullScreen;
            return fullScreenModes[(int)Index];
        }

        protected sealed override uint GetMaxIndex() {
            if (fullScreenModes == null) return uint.MinValue;
            return (uint)fullScreenModes.Length;
        }

        protected sealed override string GetDataName()
        {
            return FULLSCREEN_MODE_KEY;
        }
    }
}
