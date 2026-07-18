using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    public class UpdateableStateMachineController : StateMachineController
    {
        public UpdateableStateMachine UpdateableStateMachine => StateMachine as UpdateableStateMachine;

        private void Update() { UpdateableStateMachine?.Update(); }
        private void LateUpdate() { UpdateableStateMachine?.LateUpdate(); }
    }

}
