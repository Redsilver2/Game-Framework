using RedSilver2.Framework.Dialogs;
using RedSilver2.Framework.Items;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines
{

    public class DrinkableItemStateMachine : ConsumableItemStateMachine {

        private UnityEvent<DrinkableItemState> onStateAdded, onStateRemoved;
        private UnityEvent<DrinkableItemState> onStateEntered, onStateExited;

        protected override void Awake()
        {
            base.Awake();

            onStateAdded = new UnityEvent<DrinkableItemState>();
            onStateEntered = new UnityEvent<DrinkableItemState>();
          
            onStateRemoved = new UnityEvent<DrinkableItemState>();
            onStateExited = new UnityEvent<DrinkableItemState>();

            ConsumeStateData.AddOnFinishedListener(() => { DialogManager.PlayScreenSpace("Water is so good :p", 0.5f, 1f); });
        }

        protected sealed override bool CanAddState(ConsumableItemState state) {
            return base.CanAddState(state) && CanAddState(state as DrinkableItemState);
        }

        protected virtual bool CanAddState(DrinkableItemState state) {
            return state != null ? true : false;
        }

        protected sealed override void OnStateAdded(ConsumableItemState state)
        {
            base.OnStateAdded(state);
            OnStateAdded(state as DrinkableItemState);
        }

        protected sealed override void OnStateEntered(ConsumableItemState state)
        {
            base.OnStateEntered(state);
            OnStateEntered(state as DrinkableItemState);
        }

        protected sealed override void OnStateExited(ConsumableItemState state)
        {
            base.OnStateExited(state);
            OnStateExited(state as DrinkableItemState);
        }

        protected sealed override void OnStateRemoved(ConsumableItemState state)
        {
            base.OnStateRemoved(state);
            OnStateRemoved(state as DrinkableItemState);
        }


        protected virtual void OnStateAdded(DrinkableItemState state) {
            onStateAdded?.Invoke(state);
        }
        protected virtual void OnStateEntered(DrinkableItemState state) 
        {
            if (state != null) {
                if (state.Type == DrinkableItemStateType.Drink)
                    Animator?.PlayAnimation(ConsumeStateData);
            }


            onStateEntered?.Invoke(state);
        }

        protected virtual void OnStateExited(DrinkableItemState state) {
            onStateExited?.Invoke(state);
        }
        protected virtual void OnStateRemoved(DrinkableItemState state)  {
            onStateRemoved?.Invoke(state);  
        }

        public void ChangeState(DrinkableItemState state) {
            ChangeState(state as State);
        }
        public void ChangeState(DrinkableItemStateType type) {
            ChangeState(GetState(type));
        }

        public DrinkableItemState GetState(DrinkableItemStateType type)
        {
            foreach(State state in States) {
                DrinkableItemState _state = state as DrinkableItemState;
                if (_state == null || _state.Type != type) continue;
                return _state;
            }

            return null;
        }
    }
}
