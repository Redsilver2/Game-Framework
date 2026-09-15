using RedSilver2.Framework.Interactions;
using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Events
{
    public class PlayerStateMachineInteraction : UpdatableStateMachineEvent
    {
        [SerializeField, SerializeReference, HideInInspector] private InteractionHandler interactionHandler;
        public InteractionHandler InteractionHandler => interactionHandler;

        private const string EVENT_NAME = "Player Interaction";

        protected PlayerStateMachineInteraction(string name, PlayerMovementStateMachine stateMachine, InteractionHandler interactionHandler) : base(name, stateMachine) { 
           this.interactionHandler = interactionHandler;
        }

        public void SetInteractionHandler(InteractionHandler handler) { interactionHandler = handler; }

        protected sealed override void Disable(UpdatableStateMachine stateMachine) {
            stateMachine?.RemoveOnUpdateListener(OnUpdate);
        }

        protected sealed override void Enable(UpdatableStateMachine stateMachine)
        {
            stateMachine?.AddOnUpdateListener(OnUpdate);
        }

        private void OnUpdate() {
            interactionHandler?.Update();
        }

        public static void Create(PlayerMovementStateMachine stateMachine)
        {
            Create(stateMachine, null);
        }

        public static void Create(PlayerMovementStateMachine stateMachine, InteractionHandler controller)
        {
            if (stateMachine == null || Get(stateMachine) != null) return;
            stateMachine?.AddEvent(new PlayerStateMachineInteraction(EVENT_NAME, stateMachine, controller));
        }

        public static PlayerStateMachineInteraction Get(StateMachine stateMachine) {
            if (stateMachine == null) return null;
            return stateMachine.GetEvent(EVENT_NAME) as PlayerStateMachineInteraction;
        }
    }
}
