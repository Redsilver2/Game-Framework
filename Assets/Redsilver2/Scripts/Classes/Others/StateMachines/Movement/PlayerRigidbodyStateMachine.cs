using RedSilver2.Framework.StateMachines.Controllers;
using Unity.VisualScripting;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class PlayerRigidbodyStateMachine : PlayerMovementStateMachine
    {
        private Rigidbody _rigidbody;

        protected sealed override void Awake()
        {
            base.Awake();
            _rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
        }

        protected sealed override void OnMoved(Vector3 nextPosition)
        {
            _rigidbody?.MovePosition(nextPosition);
        }
 
    }
}
