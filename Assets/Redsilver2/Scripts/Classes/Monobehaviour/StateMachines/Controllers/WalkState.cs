using RedSilver2.Framework.StateMachines.Extensions;
using UnityEditor;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public abstract class WalkState : MovementState
    {
        [Space]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float moveTransitionSpeed;

        public float MoveSpeed => moveSpeed;
        public float MoveTransitionSpeed => moveTransitionSpeed;

        private const string SOUND_EVENT = "Walk Sound";
        public const MovementStateType TYPE = MovementStateType.Walk;

        public WalkState(MovementStateMachine stateMachine) : base(stateMachine) {
           
        }


#if UNITY_EDITOR
        private bool showSoundExtension;

        public override void Validate()
        {
            base.Validate();
            moveSpeed = Mathf.Clamp(moveSpeed, 0f, float.MaxValue);
            
            moveTransitionSpeed = Mathf.Clamp(moveTransitionSpeed, 0f, float.MaxValue);
            if (!ContainsEvent(SOUND_EVENT)) AddEvent(new MovementWalkSound(SOUND_EVENT, this));
        }

        protected sealed override void DisplayBaseSettings(Color foldoutColor, Color fieldColor) {
            EditorExtension.Space(2.5f);


            SetWalkSpeed(EditorExtension.DisplayFloatSlider("Walk Speed 💨", moveSpeed, 0f, 1000f, fieldColor));
            SetTransitionSpeed(EditorExtension.DisplayFloatSlider("Walk Transition Speed", moveTransitionSpeed, 0f, 1000f, fieldColor));
        }

        protected override void DisplayExenstions(Color foldoutColor, Color fieldColor)
        {
            if (EditorExtension.DisplayFoldout("Sound", ref showSoundExtension, foldoutColor)){
                if (!ContainsEvent("Walk Sound")){
                    EditorExtension.DisplayButton("Add Sound", () => { AddEvent(new MovementWalkSound("Walk Sound", this)); });
                }
                else
                {
                    EditorExtension.DisplayButton("Remove Sound", () => { RemoveEvent("Walk Sound"); });
                }
            }
        }

#endif

        public sealed override bool CanTransition() {
            MovementStateMachine movementStateMachine = GetMovementStateMachine(this);
            if (movementStateMachine == null) return false;

            return movementStateMachine.IsMoving && movementStateMachine.IsGrounded &&
                   !RunState.GetIsRunning(movementStateMachine) && !CrouchState.GetIsCrouching(movementStateMachine);
        }

        protected sealed override void OnUpdate() {
            base.OnUpdate();
            GetMovementStateMachine(this)?.SetMoveSpeed(moveSpeed, moveTransitionSpeed);
        }

        protected sealed override void SetMovementStateType(ref MovementStateType type) {
            type = TYPE;
        }

        public void SetWalkSpeed(float walkSpeed) {
            this.moveSpeed = Mathf.Clamp(walkSpeed, 0f, float.MaxValue);
        }

        public void SetTransitionSpeed(float transitionSpeed) {
            this.moveTransitionSpeed = Mathf.Clamp(transitionSpeed, 0f, float.MaxValue);
        }

        public static WalkState GetState(MovementStateMachine stateMachine) { 
            if(stateMachine == null) return null;
            return stateMachine.GetState(TYPE) as WalkState;
        }


        protected sealed override MovementStateType[] GetDefaultInvalidTypes()
        {
            return new MovementStateType[] { TYPE, LandState.TYPE };
        }
    }
}
