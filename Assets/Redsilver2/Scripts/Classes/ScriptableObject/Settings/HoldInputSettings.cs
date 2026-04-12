using UnityEngine;


namespace RedSilver2.Framework.Inputs.Settings
{
    [CreateAssetMenu(fileName = "New Hold Input Settings", menuName = "Input/Settings/Single/Hold")]
    public sealed class HoldInputSettings : SingleInputSettings
    {
        public override SingleInputType GetInputType()
        {
            return IsOverrideable ? SingleInputType.OverrideableHold : SingleInputType.Hold;
        }
    }
}
