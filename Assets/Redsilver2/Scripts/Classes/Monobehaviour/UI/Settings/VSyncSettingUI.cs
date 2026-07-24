using UnityEngine;

namespace RedSilver2.Framework.UI
{
    public class VSyncSettingUI : BoolSettingUI {

        private const string VSYNC_KEY = "VSYNC";

        public override void ApplySetting() {
            base.ApplySetting();
            QualitySettings.vSyncCount = (int)Index;
        }

        protected sealed override string GetSettingName() {
            char[] chars = VSYNC_KEY.ToCharArray();
            string results = null;

            if (chars == null) return results;

            for(int i = 0; i < chars.Length; i++) {
                char c = chars[i];
                if (i > 1) c = char.ToLower(c);
                results += c;
            }

            return results;
        }

        protected sealed override string GetDataName() { return VSYNC_KEY; }
    }
}
