using RedSilver2.Framework.StateMachines.States;
using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Events
{
    public class StateDisplayer : StateMachineEvent
    {
        [SerializeField] private TextMeshProUGUI displayer;

        protected override void SetStateMachineEvents(StateMachine stateMachine, bool isAddingEvents) {
            if (isAddingEvents) { 
                stateMachine?.AddOnStateEnteredListener(OnStateEntered);
                stateMachine?.AddOnStateExitedListener(OnStateExited);
            }
            else {
                stateMachine?.RemoveOnStateEnteredListener(OnStateEntered);
                stateMachine?.RemoveOnStateExitedListener(OnStateExited);
            }

        }
        private void OnStateEntered(State state) { if(displayer != null) displayer.text = state.GetStateName(); }
        private void OnStateExited(State state) { if (displayer != null) displayer.text = "None"; }
    }
}