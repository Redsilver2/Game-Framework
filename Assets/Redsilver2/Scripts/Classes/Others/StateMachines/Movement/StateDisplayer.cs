using RedSilver2.Framework.StateMachines.States;
using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Events
{
    public class StateDisplayer : StateMachineEvent
    {
        [SerializeField] private TextMeshProUGUI displayer;

        public StateDisplayer(string name, StateMachine stateMachine) : base(name, stateMachine) {

        }

        protected sealed override void Disable(StateMachine stateMachine)
        {
            stateMachine?.RemoveOnStateEnteredListener(OnStateEntered);
            stateMachine?.RemoveOnStateExitedListener(OnStateExited);
        }

        protected sealed override void Enable(StateMachine stateMachine)
        {
            stateMachine?.AddOnStateEnteredListener(OnStateEntered);
            stateMachine?.AddOnStateExitedListener(OnStateExited);  
        }

        private void OnStateEntered(State state) { if(displayer != null) displayer.text = state != null ?  state.Name : string.Empty; }
        private void OnStateExited(State state) { if (displayer != null) displayer.text = "None"; }
    }
}