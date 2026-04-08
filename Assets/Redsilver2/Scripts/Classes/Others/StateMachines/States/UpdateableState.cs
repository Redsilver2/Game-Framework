using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class UpdateableState : State
    {

        protected UpdateableState(StateMachine owner) : base(owner) {
       
        }
    }
}
