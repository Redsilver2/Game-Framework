using RedSilver2.Framework.Inputs;
using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine
    {
        [System.Serializable]
        public class HeadbobExtension : MovementMotionExtension
        {
            private float positionXProgress;
            private float positionYProgress;

            private float currentPositionX;
            private float currentPositionY;

            private readonly HeadbobModule module;
            public const string EXTENSION_NAME = "Headbob Extension";

            public HeadbobExtension(PlayerStateMachine owner, Transform transform, HeadbobModule module) : base(owner, transform)
            {
                this.module = module;

                if (module != null) {
                    Vector2 starterPosition = module.StarterPosition;
                    currentPositionX = starterPosition.x;
                    currentPositionY = starterPosition.y;
                }
            }

            protected override string GetExtensionName() => EXTENSION_NAME;

            protected override void OnLateUpdate()
            {
                if (transform != null)
                {
                    transform.localPosition = Vector2.right * currentPositionX + Vector2.up * currentPositionY;
                }
            }

            private float GetPosition(float defaultPosition, float min, float max, float progress)
            {
                float target;

                if     (progress > 0f) target = min;
                else if(progress < 0f) target = max;
                else                   target = defaultPosition;

                return Mathf.Lerp(defaultPosition, target, Mathf.Abs(progress));
            }

            protected sealed override void UpdateMotion(Vector2 inputMotion)
            {
                if (module != null && inputMotion.magnitude > 0)
                {           
                    UpdateMotion(module.StarterPosition, module.MinPosition, module.MaxPosition, module.HeadbobSpeed);
                }
                else
                {
                    ResetMotion();
                }
            }

            private void UpdateMotion(Vector2 starterPosition, Vector2 min, Vector2 max, float headbobSpeed)
            {
                float positionX, positionY;

                positionXProgress = Mathf.Sin(Time.time * headbobSpeed);
                positionYProgress = Mathf.Cos(Time.time * headbobSpeed);

                positionX = GetPosition(starterPosition.x, min.x, max.x, positionXProgress);
                positionY = GetPosition(starterPosition.y, min.y, max.y, positionYProgress);

                currentPositionX = Mathf.Lerp(currentPositionX, positionX, Time.deltaTime);
                currentPositionY = Mathf.Lerp(currentPositionY, positionY, Time.deltaTime);
            }

            protected sealed override void ResetMotion()
            {
                if(module != null)
                {
                    ResetMotion(module.StarterPosition);
                }
            }

            private void ResetMotion(Vector2 starterPosition)
            {
                positionXProgress = Mathf.Lerp(positionXProgress, 0f, Time.deltaTime);
                positionYProgress = Mathf.Lerp(positionYProgress, 0f, Time.deltaTime);

                currentPositionX = Mathf.Lerp(currentPositionX, starterPosition.x, Time.deltaTime);
                currentPositionY = Mathf.Lerp(currentPositionY, starterPosition.y, Time.deltaTime);
            }


            public sealed override bool Compare(PlayerExtension extension)
            {
                if(extension == null) return false;
                return extension is HeadbobExtension;
            }


            protected sealed override string[] GetCompatibleStates()
            {
                return new string[] { WalkState.STATE_NAME, RunState.STATE_NAME, CrouchState.STATE_NAME };
            }

            protected sealed override Vector2Input GetInput()
            {
                return MoveState.GetMovementInput();
            }
        }
    }
}
