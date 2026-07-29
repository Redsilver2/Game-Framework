
using RedSilver2.Framework.Inputs.Settings;
using Unity.VisualScripting;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Controllers
{

    [RequireComponent(typeof(PlayerCharacterControllerStateMachine))]
    public sealed class PlayerCharacterController : PlayerMovementController
    {
        protected sealed override void Awake() {
            gameObject.GetOrAddComponent<PlayerCharacterControllerStateMachine>();
            base.Awake();
        }
    }
}
