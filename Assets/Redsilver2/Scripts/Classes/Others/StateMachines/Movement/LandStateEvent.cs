using RedSilver2.Framework.StateMachines.Events;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines {
    [System.Serializable]
    public abstract class LandStateEvent : MovementStateEvent
    {
        protected LandStateEvent() : base() { }

        protected sealed override void SetCompatibleStateTypes(ref MovementStateType[] types) {
            types = new MovementStateType[] { LandState.TYPE };
        }
    }
}
