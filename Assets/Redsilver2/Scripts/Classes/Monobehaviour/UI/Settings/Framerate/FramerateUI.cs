using RedSilver2.Framework.UI;
using UnityEngine;

namespace RedSilver2.Framework.Settings
{
    public abstract class FramerateUI : IntSettingUI
    {
        protected readonly FramerateSetting framerateSetting = FramerateSetting.Instance;

        public override void Apply() {
            Debug.Log(framerateSetting);
            if(framerateSetting != null) framerateSetting?.SetFrameRateLimit((int)Index);
            base.Apply();         
        }

        protected sealed override uint GetMaxIndex() {
            if (framerateSetting == null) return 0;
            return (uint)framerateSetting.FramerateLimits.Length;
        }
    }
}
