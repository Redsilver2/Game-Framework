using UnityEngine;

namespace RedSilver2.Framework.Scenes.UI
{
    public sealed class CustomLoadingIcon : LoadingSpinningIcon
    {
        protected override Vector3 GetDesiredRotation()
        {
            return Vector3.forward;
        }
    }
}
