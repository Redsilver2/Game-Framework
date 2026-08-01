using RedSilver2.Framework.StateMachines;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

public class JumpPad : MonoBehaviour {
    [SerializeField] private float jumpForce;

    private async void OnTriggerEnter(Collider other) {
        if(other.tag.ToLower() == "player") {
            if(other.TryGetComponent(out MovementStateMachine stateMachine)) {
                JumpState state = JumpState.GetState(stateMachine);
                state?.SetJumpForce(jumpForce);
                stateMachine?.ChangeState(state, false);
            }
        }
    }
}
