using RedSilver2.Framework.StateMachines.Events;
using Unity.VisualScripting;
using UnityEngine;

namespace RedSilver2.Framework.Items
{
    [RequireComponent(typeof(MouseMovementTiltMotion))]
    public sealed class EquippableItemMouseTiltMotion : EquippableItemMovementUpdater
    {
        protected sealed override void Awake()
        {
            gameObject.GetOrAddComponent<MouseMovementTiltMotion>();
            base.Awake();
        }
    }
}