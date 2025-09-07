using RedSilver2.Framework.Inputs;
using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine
    {
        public sealed class MouseTiltMotionExtension : TiltMotionExtension
        {
            public const string EXTENSION_NAME = "Mouse Tilt Motion Extension";

            public MouseTiltMotionExtension(PlayerStateMachine owner, Transform transform, MouseTiltMotionModule module) : base(owner, transform, module)
            {
               
            }

            protected sealed override float GetRotationSpeedX()
            {
                return CameraController.GetSensitivityX();
            }

            protected sealed override float GetRotationSpeedY()
            {
                return CameraController.GetSensitivityY();
            }

            protected sealed override string GetExtensionName()
            {
                return EXTENSION_NAME; 
            }

            protected sealed override Vector2Input GetInput()
            {
                return PlayerCameraController.GetMouseInput();
            }

            public override bool Compare(PlayerExtension extension)
            {
                if(extension == null) return false;
                return extension is MouseTiltMotionExtension;
            }
        }
    }
}
