using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Settings
{
    [CreateAssetMenu(fileName = "New Movement Speed Settings", menuName = "Movement/Settings/Move Speed")]
    public class MovementSpeedSetting : ScriptableObject {
        public float moveSpeed;
        public float transitionSpeed;

        public void SetTransitionSpeed(MovementStateMachine stateMachine) {
            if(stateMachine != null) {
                stateMachine?.SetMovementSpeed(
                    Mathf.Lerp(stateMachine.MoveSpeed, moveSpeed, Time.deltaTime * transitionSpeed));
            }
        }
    }
}
