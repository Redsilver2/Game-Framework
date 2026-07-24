
using UnityEngine;

namespace RedSilver2.Framework.UI
{
    public abstract class BoolSettingUI : UIntSettingUI {

        [SerializeField] private bool canDisplaySettingName;

        protected bool GetValue() { return Index == 1; }

        protected string GetDisplayedValue() {
            return GetDisplayedValue(GetValue());
        }

        protected string GetDisplayedValue(bool isEnabled) {
            string settingName = canDisplaySettingName ? GetSettingName() : string.Empty;
            return $"{settingName} " + (isEnabled ? "Enabled" : "Disabled");
        }

        protected sealed override uint GetMaxIndex() { return 2; }
        protected abstract string GetSettingName();

    }
}
