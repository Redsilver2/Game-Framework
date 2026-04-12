using UnityEngine;

namespace RedSilver2.Framework.Inputs.Settings
{
    [CreateAssetMenu(fileName = "New Release Input Settings", menuName = "Input/Settings/Single/Release")]
    public sealed class ReleaseInputSettings : SingleInputSettings
    {
        public sealed override SingleInputType GetInputType()
        {
            return IsOverrideable ? SingleInputType.OverrideableRelease : SingleInputType.Release;
        }
    }
}