using RedSilver2.Framework.Inputs;
using UnityEngine;
using UnityEngine.Windows;


namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine
    {
        public abstract class TiltMotionExtension : MovementMotionExtension
        {
            private float rotationTrackerX;
            private float rotationTrackerY;

            private float desiredTiltY;
            private float desiredTiltX;

            protected readonly TiltMotionModule module;

            protected TiltMotionExtension(PlayerStateMachine owner, Transform transform, TiltMotionModule  module) : base(owner, transform)
            {
                this.module = module;
            }

            protected sealed override void OnLateUpdate()
            {
                if(transform != null)
                {
                    transform.localEulerAngles = Vector3.right * rotationTrackerY + Vector3.forward * rotationTrackerX;
                }
            }

            protected sealed override void ResetMotion()
            {
                ResetMotionX();
                ResetMotionY();
            }

            protected void ResetMotionX()
            {
                rotationTrackerX = Mathf.Lerp(rotationTrackerX, 0f, Time.deltaTime);
                desiredTiltX = Mathf.Lerp(desiredTiltX, 0f, Time.deltaTime);
            }

            protected void ResetMotionY()
            {
                rotationTrackerY = Mathf.Lerp(rotationTrackerY, 0f, Time.deltaTime);
                desiredTiltY = Mathf.Lerp(desiredTiltY, 0f, Time.deltaTime);
            }

            protected void RotateX(Vector2 inputValue)
            {
                if (module != null && inputValue.x != 0f)
                {
                    Vector2 min = module.MinRotation, max = module.MaxRotation;

                    rotationTrackerX -= Time.deltaTime * inputValue.x * GetRotationSpeedX();
                    rotationTrackerX = Mathf.Clamp(rotationTrackerX, min.x, max.x);
                    desiredTiltX     = Mathf.Lerp(desiredTiltX, rotationTrackerX, Time.deltaTime);
                }
                else { ResetMotionX(); }
            }

            protected void RotateY(Vector2 inputValue)
            {
                if (module != null && inputValue.y != 0f)
                {
                    Vector2 min = module.MinRotation, max = module.MaxRotation;

                    rotationTrackerY += Time.deltaTime * inputValue.y * GetRotationSpeedY();
                    rotationTrackerY = Mathf.Clamp(rotationTrackerY, min.y, max.y);
                    desiredTiltY = Mathf.Lerp(desiredTiltY, rotationTrackerY, Time.deltaTime);
                }
                else{ ResetMotionY(); }
            }

            protected void Rotate(Vector2 inputValue)
            {
                RotateX(inputValue);
                RotateY(inputValue);
            }

            protected sealed override void UpdateMotion(Vector2 inputMotion)
            {
                if (inputMotion.magnitude > 0f)
                    Rotate(inputMotion);
                else
                    ResetMotion();
            }

            protected sealed override string[] GetCompatibleStates()
            {
                return null;
            }

            protected abstract float GetRotationSpeedX();
            protected abstract float GetRotationSpeedY();
        }
    }
}
