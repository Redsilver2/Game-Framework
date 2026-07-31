using RedSilver2.Framework.StateMachines;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

public class JumpPad : MonoBehaviour {
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpDuration;

    private bool canTrigger = true;

    private async void OnTriggerEnter(Collider other) {
        if(other.tag.ToLower() == "player" && canTrigger) {
            if(other.TryGetComponent(out MovementStateMachine stateMachine)) {
                JumpState state = JumpState.GetState(stateMachine);
                state?.Cancel();

                state?.SetJumpDuration(jumpDuration);
                state?.SetJumpForce(jumpForce);

                stateMachine?.ChangeState(state, false);
                canTrigger = false;

                await WaitJumpReset(jumpDuration);

                state?.ResetJumpDuration();
                state?.ResetJumpForce();

                canTrigger = true;
            }
        }
    }

    private async Awaitable WaitJumpReset(float jumpDuration) {
        float t = 0f;

        while(t < jumpDuration) {
            t += Time.deltaTime;
            await Awaitable.NextFrameAsync();
        }
    }
}
