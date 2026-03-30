using UnityEngine;

namespace RedSilver2.Framework.Inputs.Settings
{
    public abstract class InputSettings : ScriptableObject {
        public  string InputName;
        public  bool   IsEnabled => false;
        public  bool   IsInitialized => false;

        public abstract void Enable();
        public abstract void Disable();
    }
}
