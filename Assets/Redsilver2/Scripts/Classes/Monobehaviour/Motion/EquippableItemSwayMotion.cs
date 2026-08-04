using Unity.VisualScripting;
using UnityEngine;

namespace RedSilver2.Framework.Items
{
    [RequireComponent(typeof(MovementSwayMotion))]
    public class EquippableItemSwayMotion : EquippableItemMovementUpdater
    {
        protected sealed override void Awake()
        {
            gameObject.GetOrAddComponent<MovementSwayMotion>();
            base.Awake();
        }
    }
}