using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ProBuilder.Shapes;

namespace RedSilver2.Framework.Interactions.Actions
{
    [System.Serializable]
    public class OpenRotateableDoorAction : RotateableDoorInteractionAction
    {
        [Space]
        [SerializeField] private float desiredForwardAngle = 90f;
        [SerializeField] private float desiredBackwardAngle = -90f;

        public OpenRotateableDoorAction(RotateableDoor module, Interaction interaction) : base(module, interaction)
        {
            Debug.Log(interaction + " waw");

            interaction?.AddOnInteractedListener(handler => {
                Debug.Log(module + " wow");

                module?.SetOpenRotation(true ? desiredBackwardAngle : desiredForwardAngle);
                module?.Open();
            });
        }
    }
}
