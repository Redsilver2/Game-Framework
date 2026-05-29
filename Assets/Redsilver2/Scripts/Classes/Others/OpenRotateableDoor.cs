using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Actions
{
    [CreateAssetMenu(fileName = "New Open Rotateable Door Interaction", menuName = "Interaction/Door/Rotateable/Open")]
    public class OpenRotateableDoor : RotateableDoorInteractionAction
    {
        [Space]
        [SerializeField] private float desiredForwardAngle;
        [SerializeField] private float desiredBackwardAngle;

        public sealed override string GetInteractionName() {
            return "Open";
        }

        protected override UnityAction<InteractionHandler> GetBaseEvent(RotateableDoor door)
        {
            if(door == null) return null;   

            return handler => {
                if (handler == null || door == null) return;
                door?.SetOpenRotation(true ? desiredBackwardAngle : desiredForwardAngle);
                door?.Open();
            };
        }
    }
}
