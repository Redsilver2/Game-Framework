using RedSilver2.Framework.StateMachines.Events;
using Unity.VisualScripting;
using UnityEngine;

namespace RedSilver2.Framework.Items
{
    [RequireComponent(typeof(MovementSwayMotion))]
    public class EquippableItemSwayMotion : EquippableItemMovementUpdater
    {
        protected sealed override void SetMovementMotion(ref MovementMotion motion)
        {
            motion = gameObject.GetOrAddComponent<MovementSwayMotion>(); 
        }
    }
}