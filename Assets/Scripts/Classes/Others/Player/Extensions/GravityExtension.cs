using UnityEngine;
namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public abstract class GravityExtension : PlayerExtension
        {
            protected readonly GroundCheckExtension groundCheckExtension;
            private bool wasLateUpdateEventAdded;

            public const string GRAVITY_SETTING_NAME = "Gravity";

            protected static float gravity;
            public static float Gravity => gravity;


            protected GravityExtension(PlayerStateMachine owner) : base(owner)
            {
                SetGroundCheckExtension(ref groundCheckExtension);
                gravity = 0f;
            }

            protected GravityExtension(PlayerStateMachine owner, float defaultGravity) : base(owner)
            {
                SetGroundCheckExtension(ref groundCheckExtension);
                gravity = defaultGravity;
                wasLateUpdateEventAdded = false;
            }


            public float GetGravity()
            {
                return gravity;
            }

            protected void SetGravity(float gravity)
            {
                if (gravity < 0f) gravity = 0f;
                GravityExtension.gravity = gravity;
            }

            protected float GetDesiredGravity()
            {
                if (groundCheckExtension == null || owner == null) return 0f;
                return groundCheckExtension.IsGrounded ? 5f : 10f;
            }


            private void OnLateUpdate()
            {
                if (owner != null)
                {
                    CharacterController character = owner.character;
                    character.Move(Time.deltaTime * -character.transform.up * GetGravity());
                }
            }

            protected virtual void AddListeners()
            {
                if (owner != null && States.Length > 0)
                {
                    if (!wasLateUpdateEventAdded)
                    {
                        wasLateUpdateEventAdded = true;
                        owner.AddOnLateUpdateListener(OnLateUpdate);
                    }
                }
            }

            protected virtual void RemoveListeners()
            {
                if (owner != null && States.Length == 0)
                {
                    if (wasLateUpdateEventAdded)
                    {
                        wasLateUpdateEventAdded = false;
                        owner.RemoveOnLateUpdateListener(OnLateUpdate);
                    }
                }
            }

            private void SetGroundCheckExtension(ref GroundCheckExtension groundCheckExtension)
            {
                if (owner != null)
                {
                    GroundCheckExtension.Instantiate(owner);
                    groundCheckExtension = GroundCheckExtension.Get(owner);
                }
            }

            protected sealed override void OnStateAdded(PlayerState state)
            {
                if (state != null && !ContainsState(state))
                {
                    base.OnStateAdded(state);
                    AddListeners();
                }
            }

            protected sealed override void OnStateRemoved(PlayerState state)
            {
                if (state != null && ContainsState(state))
                {
                    base.OnStateRemoved(state);
                    RemoveListeners();
                }
            }

            protected sealed override void OnEnable()
            {
                base.OnEnable();
                AddListeners();
            }

            protected sealed override void OnDisable()
            {
                base.OnDisable();
                RemoveListeners();
            }
        }
    }

}
