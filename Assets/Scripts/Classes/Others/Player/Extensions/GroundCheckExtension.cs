using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public sealed class GroundCheckExtension : PlayerExtension
        {
            private bool isGrounded;
            private string groundTag;

            private bool wasUpdateEventAdded;

            private CharacterController character;
            public const string EXTENSION_NAME = "Ground Check Extension";

            public bool IsGrounded => isGrounded;
            public string GroundTag => groundTag;

            public GroundCheckExtension(PlayerStateMachine owner) : base(owner)
            {
                this.isGrounded = false;
                this.groundTag  = string.Empty;

                this.wasUpdateEventAdded = false;
                this.character  = owner.character;
                
            }

            protected override void OnEnable()
            {
                base.OnEnable();
                AddListener();
            }

            protected override void OnDisable()
            {
                base.OnDisable();
                RemoveListener();
            }

            protected override void OnStateAdded(PlayerState state) 
            {
                base.OnStateAdded(state);
                if (state != null) AddListener();
            }

            protected override void OnStateRemoved(PlayerState state) 
            {
                base.OnStateRemoved(state);
                if (state != null) RemoveListener();
            }

            private void OnUpdate()
            {
                if (owner != null)
                {
                    isGrounded = GroundCheck(owner.GroundCheckRange, out groundTag);
                    Debug.DrawRay(character.transform.position, -character.transform.up, IsGrounded ? Color.green : Color.red);
                }
            }

            private void AddListener()
            {
                if (owner != null && States.Length > 0)
                {
                    if (!wasUpdateEventAdded)
                    {
                        wasUpdateEventAdded = true;
                        owner.AddOnUpdateListener(OnUpdate);
                    }
                }
            }

            private void RemoveListener()
            {
                if(owner != null && States.Length == 0)
                {
                    if (wasUpdateEventAdded)
                    {
                        wasUpdateEventAdded = false;
                        owner.RemoveOnUpdateListener(OnUpdate);
                    }
                }
            }

            private bool GroundCheck(float groundCheckRange, out string groundTag)
            {
                return GroundCheck(GetHitInfo(groundCheckRange), out groundTag);
            }

            private bool GroundCheck(RaycastHit hitInfo, out string groundTag)
            {
                return GroundCheck(hitInfo.collider, out groundTag);
            }

            private bool GroundCheck(Collider collider, out string groundTag)
            {
                groundTag = string.Empty;
                if(collider == null) return false;

                if (collider.gameObject.layer == LayerMask.NameToLayer(GameManager.GROUND_LAYER_NAME))
                {
                    groundTag = collider.tag;
                    return true;
                }

                return false;
            }

            private RaycastHit GetHitInfo(float groundCheckRange)
            {
                Physics.Raycast(character.transform.position, -character.transform.up, out RaycastHit hit, groundCheckRange, ~LayerMask.NameToLayer(PlayerController.PLAYER_LAYER_NAME));
                return hit;
            }


            protected sealed override string GetExtensionName() => EXTENSION_NAME;

            public static void Enable(PlayerStateMachine controller)
            {
                GroundCheckExtension extension = Get(controller);
                if (extension != null) extension.Enable();
            }

            public static void Disable(PlayerStateMachine controller)
            {
                GroundCheckExtension extension = Get(controller);
                if (extension != null) extension.Disable();
            }


            public static GroundCheckExtension Get(PlayerStateMachine controller)
            {
                if (controller == null) return null;
                return controller.GetExtension(EXTENSION_NAME) as GroundCheckExtension;
            }

            public static void Instantiate(PlayerStateMachine controller)
            {
                if (controller != null)
                {
                    if (Get(controller) == null)
                    {
                        controller.AddExtension(new GroundCheckExtension(controller));
                    }
                }
            }
        }
    }
}
