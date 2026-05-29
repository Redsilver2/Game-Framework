using UnityEngine;

namespace RedSilver2.Framework.Inputs.Settings
{
    public abstract class InputSettings : ScriptableObject {
        public  string InputName;

        [Space]
        public bool IsOverrideable;
        public abstract void Enable();
        public abstract void Disable();
    }
}
