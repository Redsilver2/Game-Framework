using RedSilver2.Framework.StateMachines;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

public class CrouchZone : MonoBehaviour {

    private MovementStateType[] disabledStates;

    private void Awake()
    {
        SetDisabledStates(ref disabledStates);
    }


    private void OnTriggerEnter(Collider other)
    {
        MovementStateMachine stateMachine = MovementStateMachine.GetInstance(other);
        SetDisabledStates(stateMachine, false);

        stateMachine?.ChangeState(MovementStateType.Crouch, true);
    }

    private void OnTriggerExit(Collider other)
    {
        SetDisabledStates(MovementStateMachine.GetInstance(other), true);
    }

    private void SetDisabledStates(ref MovementStateType[] stateTypes) {
        stateTypes = new MovementStateType[] { MovementStateType.Walk, MovementStateType.Run, MovementStateType.Jump,
            MovementStateType.Idol
        };
    }

    private void SetDisabledStates(MovementStateMachine stateMachine, bool isEnabled)
    {
        foreach(MovementStateType stateType in disabledStates)  {
            if (isEnabled) stateMachine?.EnableState(stateType);
            else           stateMachine?.DisableState(stateType);
        }
    }
}
