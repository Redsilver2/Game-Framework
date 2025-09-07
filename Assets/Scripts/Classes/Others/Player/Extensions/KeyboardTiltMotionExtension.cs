using RedSilver2.Framework.Inputs;
using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine
    {
        public sealed class KeyboardTiltMotionExtension : TiltMotionExtension
        {
            public const float  MIN_ROTATION_SPEED = 0.2f;
            public const string EXTENSION_NAME     = "Keyboard Tilt Motion Extension";

            public KeyboardTiltMotionExtension(PlayerStateMachine owner, Transform transform, KeyboardTiltMotionModule module) : base(owner, transform, module)
            {

            }


            protected sealed override float GetRotationSpeedX()
            {
                if (module != null) return (module as KeyboardTiltMotionModule).RotationSpeed;
                return 1f;
            }

            protected sealed override float GetRotationSpeedY()
            {
                if (module != null) return (module as KeyboardTiltMotionModule).RotationSpeed;
                return 1f;
            }

            protected sealed override string GetExtensionName()
            {
                return EXTENSION_NAME;
            }

            protected sealed override Vector2Input GetInput()
            {
                return MoveState.GetMovementInput(); 
            }

            public sealed override bool Compare(PlayerExtension extension)
            {
                if(extension == null) return false;
                return extension is KeyboardTiltMotionExtension;    
            }
        }
    }
}
