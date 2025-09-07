using UnityEngine;
namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public abstract class GravityExtension : PlayerExtension
        {
            private bool canApplyGravity = false;

            protected readonly GravityModule gravityModule;
            protected readonly GroundCheckModule groundCheckModule;

            public const string GRAVITY_SETTING_NAME = "Gravity";


            protected GravityExtension(PlayerStateMachine owner) : base(owner)
            {
                canApplyGravity         = false;

                gravityModule     = owner.owner.GetComponentInChildren<GravityModule>();    
                groundCheckModule = owner.owner.GetComponentInChildren<GroundCheckModule>();   
            }

            protected GravityExtension(PlayerStateMachine owner, GroundCheckModule groundCheckModule) : base(owner)
            {
                canApplyGravity = false;

                gravityModule = owner.owner.GetComponentInChildren<GravityModule>();
                this.groundCheckModule =  groundCheckModule;
            }

            protected GravityExtension(PlayerStateMachine owner, GravityModule gravityModule) : base(owner)
            {
                canApplyGravity        = false;
                this.gravityModule     = gravityModule;
                this.groundCheckModule = owner.owner.GetComponentInChildren<GroundCheckModule>();
            }

            protected GravityExtension(PlayerStateMachine owner, GravityModule gravityModule, GroundCheckModule groundCheckModule) : base(owner)
            {
                this.gravityModule     = gravityModule;
                this.groundCheckModule = groundCheckModule;
            }

            protected void SetGravity(float gravity)
            {
                if (gravity < 0f) gravity = 0f;
                if (gravityModule != null) gravityModule.SetGravity(gravity);   
            }

            protected float GetDesiredGravity()
            {
                GroundCheckExtension extension = groundCheckModule.Extension;
                if (extension == null || owner == null) return 0f;
                return extension.IsGrounded ? 15f : 50f;
            }


            private void OnLateUpdate()
            {
                if (owner != null && canApplyGravity)
                {
                    CharacterController character = owner.character;
                    character.Move(Time.deltaTime * -character.transform.up * gravityModule.Gravity);
                }
            }

            protected override void OnDisable()
            {
                base.OnDisable();           
                if (owner != null) owner.RemoveOnLateUpdateListener(OnLateUpdate);
            }

            protected override void OnEnable()
            {
                base.OnEnable();
                if (owner != null) owner.AddOnLateUpdateListener(OnLateUpdate);
            }

            protected sealed override void OnStateEnter(PlayerState state)
            {
                canApplyGravity = IsCompatibleState(state);
                Debug.Log("Can Apply Gravity: " + canApplyGravity);
            }

            protected sealed override string[] GetCompatibleStates()
            {
                return new string[] { WalkState.STATE_NAME, FallState.STATE_NAME, RunState.STATE_NAME, CrouchState.STATE_NAME, IdolState.STATE_NAME, JumpState.STATE_NAME };
            }
        }
    }

}
