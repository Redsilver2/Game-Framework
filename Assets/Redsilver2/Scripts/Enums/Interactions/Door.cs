using RedSilver2.Framework.StateMachines;
using Unity.VisualScripting;
using UnityEngine;

namespace RedSilver2.Framework.Interactions {

    [RequireComponent(typeof(DoorStateMachine))]
    public sealed class Door : InteractionModule {

        private DoorStateMachine stateMachine;

        protected sealed override void Awake()
        {
            base.Awake();
            stateMachine = gameObject.GetOrAddComponent<DoorStateMachine>();
            SetInteractionType(InteractionType.Door);
        }

        protected sealed override void OnSelectionUpdate(InteractionHandler handler)
        {
            base.OnSelectionUpdate(handler);

            if (stateMachine != null && handler != null) {
                if (handler.IsPressed()) {
                    if (stateMachine.IsOpen) stateMachine?.Close();
                    else stateMachine?.Open();
                }
            }
        }

        public void Open()
        {
            if(stateMachine != null) stateMachine?.Open();
        }

        public void Close()
        {
            if (stateMachine != null) stateMachine?.Close();
        }
    }
}
