using UnityEngine;

namespace RedSilver2.Framework.Inputs.Settings
{
    [CreateAssetMenu(fileName = "New Press Input Settings", menuName = "Input/Settings/Single/Press")]
    public sealed class PressInputSettings : SingleInputSettings
    {
        public sealed override SingleInputType GetInputType()
        {
            return IsOverrideable ? SingleInputType.OverrideablePress : SingleInputType.Press;
        }
    }
}
