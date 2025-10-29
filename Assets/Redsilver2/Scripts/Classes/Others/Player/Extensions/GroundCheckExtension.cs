using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public sealed class GroundCheckExtension : PlayerExtension
        {
            private bool isGrounded;
            private string groundTag;

            private GroundCheckModule   module;
            private Transform transform;

            public const string EXTENSION_NAME = "Ground Check Extension";

            public bool IsGrounded => isGrounded;
            public string GroundTag => groundTag;

            public GroundCheckExtension(PlayerStateMachine owner, Transform transform, GroundCheckModule module) : base(owner)
            {
                this.isGrounded = false;
                this.groundTag  = string.Empty;
               
                this.transform  = transform;           
                this.module = module;
            }

            protected override void OnEnable()
            {
                base.OnEnable();
                if(owner != null) owner.AddOnUpdateListener(OnUpdate);
            }

            protected override void OnDisable()
            {
                base.OnDisable();
                if (owner != null) owner.RemoveOnUpdateListener(OnUpdate);
            }

            private void OnUpdate()
            {
                if (module != null && transform != null)
                {
                    isGrounded = GroundCheck(module.GroundCheckRange, out groundTag);
                    Debug.DrawRay(transform.position, -transform.up, IsGrounded ? Color.green : Color.red);
                }
            }

            protected override void OnStateEnter(PlayerState state) { }

            public sealed override bool Compare(PlayerExtension extension)
            {
                if(extension == null) return false;
                return extension is GroundCheckExtension;
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
                Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, groundCheckRange, ~LayerMask.NameToLayer(PlayerController.PLAYER_LAYER_NAME));
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

            protected override string[] GetCompatibleStates()
            {
                return new string[] { };
            }
        }
    }
}
