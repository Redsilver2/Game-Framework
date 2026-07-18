using RedSilver2.Framework.UI;
using UnityEngine;

namespace RedSilver2.Framework.Settings
{
    public abstract class FramerateSettingUI : UIntSettingUI
    {
        private readonly uint[] framerateValues = new uint[] { 30, 60, 75, 100, 120, 144, 180, 240, 360, uint.MaxValue };
        private readonly string FRAMERATE_KEY = "FRAMERATE";

        public override void Apply() {
            base.Apply();
            Application.targetFrameRate = (int)CurrentFramerate();
        }


        protected uint CurrentFramerate()
        {
            return framerateValues[Index];
        }

        protected sealed override uint GetMaxIndex() {
            if (framerateValues == null) return 0;
            return (uint)framerateValues.Length;  
        }

        protected sealed override string GetDataName()
        {
            return FRAMERATE_KEY;
        }
    }
}
