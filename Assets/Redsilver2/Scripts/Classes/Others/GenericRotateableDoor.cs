using UnityEngine;

namespace RedSilver2.Framework.Interactions {
    public class GenericRotateableDoor : RotateableDoor
    {
        protected sealed override Quaternion GetOpenRotation(float openRotationAngle) {
            return Quaternion.Euler(OriginalTarget.x, openRotationAngle, OriginalTarget.z);
        }
    }
}