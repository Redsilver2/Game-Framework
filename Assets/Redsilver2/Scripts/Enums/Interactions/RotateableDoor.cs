using RedSilver2.Framework.Interactions.Actions.Setups;
using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework.Interactions
{
    [RequireComponent(typeof(OpenRotateableDoorSetup))]
    public sealed class RotateableDoor : Door
    {   
        private float   openRotationAngle;
        private Vector3 closeRotation;

        public void SetOpenRotation(float openRotationAngle)
        {
            this.openRotationAngle = openRotationAngle;
        }


        protected sealed override IEnumerator DoorUpdate(Transform anchor)
        {
            if (anchor != null) {
                 yield return StartCoroutine(DoorUpdate(anchor, IsOpen ? GetOpenRotation() :
                                                                          GetCloseRotation()));
            }
        }

        private IEnumerator DoorUpdate(Transform anchor, Quaternion rotation)
        {
            float t = 0;
            Quaternion current = anchor.rotation;

            while (anchor != null) {
                t += Time.deltaTime * DoorSpeed;
                anchor.rotation = Quaternion.Slerp(current, rotation, Mathf.Clamp01(t/1f));

                if (t > 1f) break;
                yield return null;
            }

            if (anchor != null) anchor.rotation = rotation;
        }

        protected sealed override void SetDefaultValue(Transform anchorPoint)
        {
            closeRotation = anchorPoint ? anchorPoint.eulerAngles : Vector3.zero;   
        }

        private Quaternion GetOpenRotation()
        {
            return Quaternion.Euler(closeRotation.x, openRotationAngle, closeRotation.z);
        }

        private Quaternion GetCloseRotation()
        {
            return Quaternion.Euler(closeRotation.x, closeRotation.y, closeRotation.z);
        }


    }
}
