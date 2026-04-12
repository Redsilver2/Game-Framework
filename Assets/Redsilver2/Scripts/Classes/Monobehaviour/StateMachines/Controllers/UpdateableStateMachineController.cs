using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    public class UpdateableStateMachineController : StateMachineController
    {
        private void Update() { GetStateMachine()?.Update(); }
        private void LateUpdate() { GetStateMachine()?.LateUpdate(); }

        public override void SetStateMachine(StateMachine stateMachine)
        {
            if(stateMachine is UpdateableStateMachine)
                 base.SetStateMachine(stateMachine);
        }

        public UpdateableStateMachine GetStateMachine() {
            return StateMachine as UpdateableStateMachine;
        }
    }

}
