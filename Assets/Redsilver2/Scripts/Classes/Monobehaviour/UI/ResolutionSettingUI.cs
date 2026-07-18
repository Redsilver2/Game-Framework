using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.UI
{
    public abstract class ResolutionSettingUI : UIntSettingUI
    {
        private Resolution[] resolutions;
        private const string RESOLUTION_KEY = "RESOLUTION";

        protected override void Awake()
        {
            resolutions = Screen.resolutions.Distinct().Reverse().ToArray();
            base.Awake();
        }

        public override void Apply()
        {
            base.Apply();
            Resolution resolution = GetResolution();
            Screen.SetResolution(resolution.height, resolution.width, Screen.fullScreen);
        }

        protected Resolution GetResolution() {
            if (resolutions == null) return default;
            return resolutions[(int)Index];
        }

        protected sealed override uint GetMaxIndex()
        {
            if (resolutions == null) return 0;
            return (uint)resolutions.Length;
        }

        protected sealed override string GetDataName()
        {
            return RESOLUTION_KEY;
        }
    }
}
