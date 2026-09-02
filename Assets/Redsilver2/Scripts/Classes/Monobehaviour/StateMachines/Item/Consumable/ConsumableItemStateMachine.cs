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

        private float maxConsumptionValue;
        private float consumptionValue;

        private IEnumerator drinkingCoroutine;
        private UnityEvent<float> onProgressValueUpdate, onConsumed;

        public float         ConsumptionValue    => consumptionValue;
        public float         MaxConsumptionValue => maxConsumptionValue;

        public const string CONSUME_ANIMATION_NAME = "Consume";

        protected  override void Awake() {
            base.Awake();

            onProgressValueUpdate = new UnityEvent<float>();
            onConsumed            = new UnityEvent<float>();

            SetMaxConsumptionValue(defaultMaxConsumptionValue);
            SetConsumptionValue(maxConsumptionValue);

            GetConsumableItemAnimationController()?.GetConsumeData()?.AddOnFinishedListener(() => { ChangeState(null as State); });
            // Set State to Null
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            defaultMaxConsumptionValue = Mathf.Clamp(defaultMaxConsumptionValue, 0f, float.MaxValue);
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

        public ConsumableItemAnimationController GetConsumableItemAnimationController() {
            return GetEquippableItemAnimationController() as ConsumableItemAnimationController;
        }
    }
}
