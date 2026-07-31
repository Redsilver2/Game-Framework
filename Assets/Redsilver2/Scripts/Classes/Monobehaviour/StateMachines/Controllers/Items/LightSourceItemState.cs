using RedSilver2.Framework.StateMachines.States;
using System.Collections.Generic;

namespace RedSilver2.Framework.Items
{
    public abstract class LightSourceItemState : UpdatableState {

        private LightSourceItemStateMachine stateMachine;
        private List<LightSourceItemState> transitionStates;
    
        private LightSourceItemStateType type;
        public LightSourceItemStateType Type => type;



        protected override void Awake()
        {
            base.Awake();
            SetStateType(ref type);

            stateMachine  = GetComponent<LightSourceItemStateMachine>();


            transitionStates = new List<LightSourceItemState>();
        }

        protected abstract void SetStateType(ref LightSourceItemStateType type);

    }
}
