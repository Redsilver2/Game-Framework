using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public abstract class PlayerMovement : Movement {
        [HideInInspector][SerializeField] protected PlayerMovementStateMachine  stateMachine;

        protected PlayerMovement() : base() { 
        
        }

        protected sealed override void SetType(ref MovementType type)
        {
            type = MovementType.Player;
        }

#if UNITY_EDITOR
        public sealed override void Validate(MovementStateMachine stateMachine)
        {
            base.Validate(stateMachine);
            Validate(stateMachine as PlayerMovementStateMachine);
        }

        protected virtual void Validate(PlayerMovementStateMachine stateMachine) {
            this.stateMachine = stateMachine;
        }
#endif
    }
}
