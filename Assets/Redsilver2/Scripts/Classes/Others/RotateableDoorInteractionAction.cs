using UnityEngine;
using UnityEngine.Events;
namespace RedSilver2.Framework.Interactions.Actions
{
    [System.Serializable]
    public abstract class RotateableDoorInteractionAction : DoorInteractionAction
    {
        protected RotateableDoorInteractionAction(RotateableDoor module, Interaction interaction) : base(module, interaction)
        {
        }
    }
}