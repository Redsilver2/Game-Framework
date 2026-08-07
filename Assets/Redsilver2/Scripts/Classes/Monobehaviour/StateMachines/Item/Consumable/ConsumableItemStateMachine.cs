using RedSilver2.Framework.Animations;
using RedSilver2.Framework.StateMachines.States;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines
{
    public abstract class ConsumableItemStateMachine : EquippableItemStateMachine {

        [Space]
        [SerializeField] private float defaultMaxConsumptionValue;

        [Space]
        [SerializeField] private AnimationData consumeStateData;

        private float maxConsumptionValue;
        private float consumptionValue;

        private IEnumerator drinkingCoroutine;
        private UnityEvent<float> onProgressValueUpdate, onConsumed;


        private UnityEvent<ConsumableItemState> onStateAdded, onStateRemoved;
        private UnityEvent<ConsumableItemState> onStateEntered, onStateExited;

        public float         ConsumptionValue    => consumptionValue;
        public float         MaxConsumptionValue => maxConsumptionValue;
        public AnimationData ConsumeStateData    => consumeStateData;

        protected  override void Awake() {
            base.Awake();

            onProgressValueUpdate = new UnityEvent<float>();
            onConsumed            = new UnityEvent<float>();

            SetMaxConsumptionValue(defaultMaxConsumptionValue);
            SetConsumptionValue(maxConsumptionValue);

            consumeStateData?.AddOnFinishedListener(() => {
                Animator?.CrossFadeAnimation(DefaultStateData);
                ChangeState(null);
            });
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            defaultMaxConsumptionValue = Mathf.Clamp(defaultMaxConsumptionValue, 0f, float.MaxValue);
        }

        protected override void ValidateAnimations(RuntimeAnimatorController controller)
        {
            base.ValidateAnimations(controller);
            consumeStateData?.Validate(controller);
        }
#endif

        protected override void OnItemRemoved()
        {
            base.OnItemRemoved();
            StopConsuming();
        }

        protected override void OnItemUnEquipped()
        {
            base.OnItemUnEquipped();
            StopConsuming();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            onProgressValueUpdate?.Invoke(maxConsumptionValue <= 0f ? 0f : Mathf.Clamp01(consumptionValue/maxConsumptionValue));
        }

        protected sealed override void OnStateAdded(EquippableItemState state)
        {
            base.OnStateAdded(state);
            OnStateAdded(state as ConsumableItemState);
        }

        protected sealed override void OnStateEntered(EquippableItemState state)
        {
            base.OnStateEntered(state);
            OnStateEntered(state as ConsumableItemState);
        }

        protected sealed override void OnStateExited(EquippableItemState state)
        {
            base.OnStateExited(state);
            OnStateExited(state as ConsumableItemState);
        }

        protected sealed override void OnStateRemoved(EquippableItemState state)
        {
            base.OnStateRemoved(state);
            OnStateRemoved(state as ConsumableItemState);

        }

        protected virtual void OnStateAdded(ConsumableItemState state)
        {
            onStateAdded?.Invoke(state);
        }

        protected virtual void OnStateEntered(ConsumableItemState state)
        {
            onStateEntered?.Invoke(state);  
        }

        protected virtual void OnStateExited(ConsumableItemState state)
        {
            onStateExited?.Invoke(state);
        }

        protected virtual void OnStateRemoved(ConsumableItemState state)
        {
            onStateRemoved?.Invoke(state);
        }

        public void StopConsuming() {
            if (drinkingCoroutine != null) StopCoroutine(drinkingCoroutine);
            drinkingCoroutine = null;
        }

        public void StartConsuming(float waitTime, float consumption) {
            StopConsuming();
            drinkingCoroutine = ConsumingUpdate(waitTime, consumption);
            StartCoroutine(drinkingCoroutine);
        }

        private IEnumerator ConsumingUpdate(float waitTime, float consumption)
        {
            float t = 0f;

            while (t < waitTime) {
                t += Time.deltaTime;
                yield return null;
            }

            SetConsumptionValue(consumptionValue - consumption);
        }

        public  void ChangeState(ConsumableItemState state) {
            ChangeState(state as State);
        }

        public sealed override void ChangeState(State state)
        {
            if(IsEquipped()) base.ChangeState(state);
        }

        protected sealed override bool CanAddState(EquippableItemState state)
        {
            return base.CanAddState(state) && CanAddState(state as ConsumableItemState);
        }

        protected virtual bool CanAddState(ConsumableItemState state) {
             return state != null ? true : false;
        }

        public void SetConsumptionValue(float drinkValue) {
           this.consumptionValue = Mathf.Clamp(drinkValue, 0f, maxConsumptionValue);
        }

        public void SetMaxConsumptionValue(float maxDrinkValue)
        {
            this.maxConsumptionValue = Mathf.Clamp(maxDrinkValue, 0f, float.MaxValue);
            SetConsumptionValue(this.consumptionValue);
        }

        public void AddOnProgressValueUpdateListener(UnityAction<float> action)
        {
            if(action != null) onProgressValueUpdate?.AddListener(action);
        }
        public void RemoveOnProgressValueUpdateListener(UnityAction<float> action)
        {
            if (action != null) onProgressValueUpdate?.RemoveListener(action);
        }


        public void AddOnConsumedListener(UnityAction<float> action) {
            if (action != null) onConsumed?.AddListener(action);
        }

        public void RemoveOnConsumedListener(UnityAction<float> action) {
            if (action != null) onConsumed?.RemoveListener(action);
        }

        public void AddOnStateAddedListener(UnityAction<ConsumableItemState> action)
        {
            if (action != null) onStateAdded?.AddListener(action);
        }
        public void RemoveOnStateAddedListener(UnityAction<ConsumableItemState> action)
        {
            if (action != null) onStateAdded?.RemoveListener(action);
        }

        public void AddOnStateRemovedListener(UnityAction<ConsumableItemState> action)
        {
            if (action != null) onStateRemoved?.AddListener(action);
        }
        public void RemoveOnStateRemovedListener(UnityAction<ConsumableItemState> action)
        {
            if (action != null) onStateRemoved?.RemoveListener(action);
        }

        public void AddOnStateEnteredListener(UnityAction<ConsumableItemState> action)
        {
            if (action != null) onStateEntered?.AddListener(action);
        }
        public void RemoveOnStateEnteredListener(UnityAction<ConsumableItemState> action)
        {
            if (action != null) onStateEntered?.RemoveListener(action);
        }

        public void AddOnStateExitedListener(UnityAction<ConsumableItemState> action)
        {
            if (action != null) onStateExited?.AddListener(action);
        }
        public void RemoveOnStateExitedListener(UnityAction<ConsumableItemState> action)
        {
            if (action != null) onStateExited?.RemoveListener(action);
        }
    }
}
