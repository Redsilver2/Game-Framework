using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines.Events
{
    public sealed class KeyboardMovementTiltMotion : MovementTiltMotion
    {
        [Space]
        [SerializeField] private bool isTiltingContinuously;

        [Space]
        [SerializeField] private float tiltSpeed;

        public KeyboardMovementTiltMotion(string name, PlayerMovementStateMachine stateMachine) : base(name, stateMachine)
        {
        }

        protected sealed override float GetUpdatedRotation(float input, float current, float original, float directionUpdateSpeed, float min, float max)
        {

            if (!isTiltingContinuously) return base.GetUpdatedRotation(input, current, original, directionUpdateSpeed, min, max);
            else if (Mathf.Abs(input) > 0f) {
                float target = Mathf.Lerp(min, max, Mathf.Abs(Mathf.Sin(Time.time * tiltSpeed)));
                return Mathf.Lerp(current, target, Time.deltaTime * directionUpdateSpeed);
            }

            return GetUpdatedRotation(current);
        }

        protected override void Enable(MovementStateMachine stateMachine)
        {
            throw new System.NotImplementedException();
        }

        protected override void Disable(MovementStateMachine stateMachine)
        {
            throw new System.NotImplementedException();
        }
    }
}
