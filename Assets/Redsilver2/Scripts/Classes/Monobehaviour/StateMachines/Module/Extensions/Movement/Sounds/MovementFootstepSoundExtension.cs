using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class MovementFootstepSoundExtension : MovementStateExtension
    {
        [Space]
        [SerializeField] private float footstepSoundPlayDelay;  
        private float currentPlayTime;

        private MovementGroundCheckExtension groundCheckExtension;

        protected override void Awake()
        {
            base.Awake();
            groundCheckExtension = transform.root.GetComponentInChildren<MovementGroundCheckExtension>();

            stateMachine?.AddOnStateModuleAddedListener(OnStateExtensionAdded);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            stateMachine?.RemoveOnStateModuleAddedListener(OnStateExtensionAdded);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            stateMachine?.AddOnStateModuleAddedListener(OnStateExtensionAdded);
        }

        private void OnStateExtensionAdded(StateModule module)
        {

        }

        private void UpdateCurrenTime(string groundTag)
        {
            currentPlayTime += Time.deltaTime;

            if (currentPlayTime >= footstepSoundPlayDelay) {
                currentPlayTime = 0f;
            }
        }

        protected virtual void OnStateUpdated() {
            if (stateMachine == null || groundCheckExtension == null) return;
            if (groundCheckExtension.IsGrounded) { UpdateCurrenTime(groundCheckExtension.GroundTag);   }
            else                                 { currentPlayTime = 0f; }
        }

        protected sealed override void OnStateAdded(MovementState state) {
            state?.AddOnUpdateListener(OnStateUpdated);
        }

        protected sealed override void OnStateRemoved(MovementState state) {
            state?.AddOnUpdateListener(OnStateUpdated);
        }
    }
}
