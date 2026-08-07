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

        protected sealed override void SetStateMachineEvents(PlayerMovementStateMachine stateMachine, bool isAddingEvents) {
            if (isAddingEvents) {
                stateMachine?.AddOnLateUpdateListener(OnLateUpdate);
                stateMachine?.AddOnMoveInputUpdateListener(OnInputUpdate);
            }
            else {
                stateMachine?.RemoveOnLateUpdateListener(OnLateUpdate);
                stateMachine?.RemoveOnMoveInputUpdateListener(OnInputUpdate);
            }
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


    }
}
