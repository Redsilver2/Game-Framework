using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Settings
{
    public abstract class MovementConfiguration : ScriptableObject {

        [Space]
        [SerializeField] private bool  isRunningAllowed;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;

        [Space]
        [SerializeField] private bool  isFallingAllowed;
        [SerializeField] private float defaultGravitySpeed;
        [SerializeField] private float fallGravitySpeed;

        [Space]
        [SerializeField] private bool  isJumpingAllowed;
        [SerializeField] private float jumpForce;


        [Space]
        [SerializeField] private bool isCrouchingAllowed;

        [SerializeField] private float standingSpeed;
        [SerializeField] private float standingHeight;

        [SerializeField] private float crouchingSpeed;
        [SerializeField] private float crouchingHeight;

        public float WalkSpeed => walkSpeed;
        public float RunSpeed  => runSpeed;  

        public float DefaultGravitySpeed => defaultGravitySpeed;
        public float FallGravitySpeed => fallGravitySpeed;

        public float JumpForce => jumpForce;
        public float StandingSpeed => standingSpeed;
        public float StandingHeight => standingHeight;

        
        public float CrouchingSpeed    => crouchingSpeed;
        public float CrouchingHeight   => crouchingHeight;

        public bool IsRunningAllowed   => isRunningAllowed;
        public bool IsFallingAllowed   => isFallingAllowed;
        public bool IsJumpingAllowed   => isJumpingAllowed;
        public bool IsCrouchingAllowed => isCrouchingAllowed;
    }
}
