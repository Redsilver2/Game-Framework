using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.Events;
using Unity.VisualScripting;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerCharacterControllerStateMachine : PlayerMovementStateMachine
    {
        [SerializeField, HideInInspector] private MovementControlType controlType;
        private CharacterController controller;

        protected sealed override void Awake() {
            base.Awake();
            controller = gameObject.GetOrAddComponent<CharacterController>();   
        }

        protected sealed override void OnMoved(Vector3 nextPosition)
        {
            controller?.Move(nextPosition);
        }

        protected sealed override bool GetGroundCheckResult(out string groundTag)
        {
            groundTag = string.Empty;
            if(controller == null || !controller.isGrounded) return false;
            return base.GetGroundCheckResult(out groundTag);
        }

        public sealed override void SetHeight(float height)
        {
            if (controller != null)
            {
                controller.height = height;
                controller.center = Vector3.zero + Vector3.up * Mathf.Clamp01(controller.height / GetDefaultHeight());
            }
        }


        public sealed override void SetHeight(float height, float transitionSpeed)
        {
            if (controller != null) SetHeight(Mathf.Lerp(controller.height, height, Time.deltaTime * transitionSpeed));
        }

#if UNITY_EDITOR
        protected override void DisplayMovementControlTypes(ref int previousValue)
        {
            base.DisplayMovementControlTypes(ref previousValue);
            controlType = (MovementControlType)UnityEditor.EditorGUILayout.EnumPopup(controlType);

            bool wasButtonPressed = false;
            EditorExtension.DisplayButton($"Reset {controlType}", () => { wasButtonPressed = true; });
           
            if (wasButtonPressed) previousValue = -1;

            if (previousValue != (int)controlType)
            {
                if      (controlType == MovementControlType.TopDown)      handler = new PlayerTopDown3DMovementUpdater(this);
                else if (controlType == MovementControlType.SideScroller) handler = new PlayerSideScroller3DMovementUpdater(this);
                else if (controlType == MovementControlType.FirstPerson)  handler = new PlayerFirstPersonMovementUpdater(this);
                else handler = null;

                previousValue = (int)controlType;
            }
        }
#endif

        [System.Serializable]
        private enum MovementControlType {
             SideScroller,
             TopDown,
             FirstPerson,
             ThirdPerson
        }
    }
}