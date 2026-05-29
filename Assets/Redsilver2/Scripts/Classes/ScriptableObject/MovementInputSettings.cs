using UnityEngine;

namespace RedSilver2.Framework.Inputs.Settings
{
    [CreateAssetMenu(fileName = "New Movement Input Settings", menuName = "Movement Input Settings")]
    public class MovementInputSettings : ScriptableObject
    {
        [SerializeField] private KeyboardVector2InputSettings moveInputSettings;

        [Space]
        [SerializeField] private PressInputSettings pressRunSettings;
        [SerializeField] private HoldInputSettings holdRunInputSettings;

        [Space]
        [SerializeField] private PressInputSettings pressCrouchInputSettings;
        [SerializeField] private HoldInputSettings holdCrouchInputSettings;

        [Space]
        [SerializeField] private PressInputSettings pressJumpInputSettings;

        public void Enable()
        {
            moveInputSettings?.Enable();    

            pressRunSettings?.Enable(); 
            holdRunInputSettings?.Enable();

            pressCrouchInputSettings?.Enable();
            holdCrouchInputSettings?.Enable();

            pressJumpInputSettings?.Enable();   
        }

        public Vector2 GetMoveVector()
        {
            return moveInputSettings == null ? Vector2.zero : moveInputSettings.GetValue();
        }

        public bool IsRunning()
        {
            return holdRunInputSettings == null ? false : holdRunInputSettings.GetValue();
        }
    }
}
