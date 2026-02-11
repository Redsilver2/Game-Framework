using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public interface ICheckableStateTransition {
        bool GetTransitionState();
    }
}
