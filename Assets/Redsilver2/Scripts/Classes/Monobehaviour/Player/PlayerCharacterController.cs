using RedSilver2.Framework.StateMachines.States.Movement;
using Unity.VisualScripting;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    [RequireComponent(typeof(CharacterController))]

    public class PlayerCharacterController : PlayerController
    {
        [SerializeField] private bool dontDestroyOnLoad;

        private CharacterController character;
        public CharacterController Character => character;


        protected override void Awake() {
            character = gameObject.GetOrAddComponent<CharacterController>();
            if (character != null) character.skinWidth = Mathf.Epsilon;
            base.Awake();

            if (dontDestroyOnLoad) DontDestroyOnLoad(this);
        }

        protected sealed override PlayerMovementHandler GetPlayerMovementHandler() {
            return new PlayerCharacterControllerMovementHandler(this);
        }
    }
}
