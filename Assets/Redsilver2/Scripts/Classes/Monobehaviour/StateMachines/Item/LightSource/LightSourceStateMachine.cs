using RedSilver2.Framework.Animations;
using RedSilver2.Framework.StateMachines.States;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines {
    public class LightSourceStateMachine : EquippableItemStateMachine
    {
        [Space]
        [SerializeField] private float defaultMaxLifeTime;

        [Space]
        [SerializeField] private float drainLifeTimeSpeed;

        [Space]
        [SerializeField] private AnimationData onStateData;

        [Space]
        [SerializeField] private AnimationData offStateData;

        private float lifetime;
        private float maxLifeTime;

        private Light _light;
        private LightSourceState currentState;

        private IEnumerator drainLightUpdater;
        
        private UnityEvent<float> onLifeTimeProgressUpdate;
        private UnityEvent<LightSourceState> onStateAdded, onStateRemoved;
        private UnityEvent<LightSourceState> onStateEntered, onStateExited;

        public float LifeTime    => lifetime;
        public float MaxLifeTime => maxLifeTime;
        public LightSourceState  CurrentState        => currentState;
        public Light Light       => _light;

        public AnimationData OnStateData => onStateData;
        public AnimationData OffStateData => offStateData;

#if UNITY_EDITOR
        protected override void ValidateAnimations(RuntimeAnimatorController controller)
        {
            base.ValidateAnimations(controller);
            onStateData?.Validate(controller);
            offStateData?.Validate(controller);
        }
#endif


        protected override void Awake()
        {
            base.Awake();
            onLifeTimeProgressUpdate = new UnityEvent<float>();

            onStateAdded = new UnityEvent<LightSourceState>();
            onStateRemoved = new UnityEvent<LightSourceState>();

            onStateEntered = new UnityEvent<LightSourceState>();
            onStateExited = new UnityEvent<LightSourceState>();

           _light = transform.root != null ? transform.root.GetComponentInChildren<Light>() : 
                                                             GetComponentInChildren<Light>();

            if(_light != null) _light.enabled = false;   
            AddOnLifeTimeProgressUpdateListener(OnLifeTimeProgressUpdate);
          
            SetMaxLifeTime(defaultMaxLifeTime);
            SetLifeTime(maxLifeTime);
        }

        protected virtual void OnLifeTimeProgressUpdate(float progress) {   
            if(progress <= 0f)  ChangeState(LightSourceStateType.Off);
        }

        protected sealed override void OnStateAdded(EquippableItemState state) {
            base.OnStateAdded(state);
            OnStateAdded(state as LightSourceState);
        }

        protected sealed override void OnStateEntered(EquippableItemState state) {
            base.OnStateEntered(state);
            OnStateEntered(state as LightSourceState);
        }

        protected sealed override void OnStateExited(EquippableItemState state) {
            base.OnStateExited(state);
            OnStateExited(state as LightSourceState);
        }

        protected sealed override void OnStateRemoved(EquippableItemState state) {
            base.OnStateRemoved(state);
            OnStateRemoved(state as LightSourceState);
        }

        protected virtual void OnStateAdded(LightSourceState state) {
            onStateAdded?.Invoke(state);

            if (state != null) {
                if(currentState == null) {
                    ChangeState(state);
                }
            }
        }

        public void StopDrainingLightSource()
        {
            if(drainLightUpdater != null) StopCoroutine(drainLightUpdater);
            drainLightUpdater = null;
        }

        public void StartDrainingLightSource(float waitTime) {
            StopDrainingLightSource();
            drainLightUpdater = UpdateDrainLife(waitTime);

            StartCoroutine(drainLightUpdater);
        }

        private IEnumerator UpdateDrainLife(float waitTime)
        {
            float t = 0f;

            while(t < waitTime) {
                t += Time.deltaTime;
                yield return null;
            }

            StartCoroutine(UpdateDrainLife());
        }

        private IEnumerator UpdateDrainLife()
        {
            if (_light != null) _light.enabled = true;

            while (currentState != null) {
                if (currentState.Type != LightSourceStateType.On || lifetime <= 0f) break;
                lifetime = Mathf.Clamp(lifetime - Time.deltaTime * drainLifeTimeSpeed, 0f, maxLifeTime);

                onLifeTimeProgressUpdate?.Invoke(maxLifeTime < 0f ? 1f :  Mathf.Clamp01(lifetime/maxLifeTime));
                yield return null;
            }

            if(_light != null) _light.enabled = false;
        }

        protected virtual void OnStateEntered(LightSourceState state) {

            Animator animator = Animator;

            if (state != null && animator != null) {
                if (state.Type == LightSourceStateType.On) animator.PlayAnimation(onStateData);
                else animator.PlayAnimation(offStateData);
            }

            currentState = state;
            onStateEntered?.Invoke(state);   
        }

        protected virtual void OnStateExited(LightSourceState state) {

            currentState = null;
            onStateExited?.Invoke(state);
        }

        protected virtual void OnStateRemoved(LightSourceState state) {


            onStateRemoved?.Invoke(state);
        }

        protected override bool CanAddState(EquippableItemState state) {

            return base.CanAddState(state) && CanAddState(state as LightSourceState);
        }

        protected virtual bool CanAddState(LightSourceState state) {
            return state != null ? true : false;
        }
        public void AddOnLifeTimeProgressUpdateListener(UnityAction<float> action)
        {
            if (action != null) onLifeTimeProgressUpdate?.AddListener(action);
        }
        public void RemoveOnLifeTimeProgressUpdateListener(UnityAction<float> action)
        {
            if (action != null) onLifeTimeProgressUpdate?.RemoveListener(action);
        }

        public void AddOnStateAddedListener(UnityAction<LightSourceState> action)
        {
            if (action != null) onStateAdded?.AddListener(action);
        }
        public void RemoveOnStateAddedListener(UnityAction<LightSourceState> action)
        {
            if (action != null) onStateAdded?.RemoveListener(action);
        }

        public void AddOnStateRemovedListener(UnityAction<LightSourceState> action)
        {
            if (action != null) onStateRemoved?.AddListener(action);
        }
        public void RemoveOnStateRemovedListener(UnityAction<LightSourceState> action)
        {
            if (action != null) onStateRemoved?.RemoveListener(action);
        }

        public void AddOnStateEnteredListener(UnityAction<LightSourceState> action)
        {
            if (action != null) onStateEntered?.AddListener(action);
        }
        public void RemoveOnStateEnteredListener(UnityAction<LightSourceState> action)
        {
            if (action != null) onStateEntered?.RemoveListener(action);
        }

        public void AddOnStateExitedListener(UnityAction<LightSourceState> action)
        {
            if (action != null) onStateExited?.AddListener(action);
        }
        public void RemoveOnStateExitedListener(UnityAction<LightSourceState> action)
        {
            if (action != null) onStateExited?.RemoveListener(action);
        }


        public void SetDrainLifeTimeSpeed(float speed) { this.drainLifeTimeSpeed = Mathf.Clamp(speed, 0f, float.MaxValue); }
        public void SetLifeTime(float lifetime) { 
            this.lifetime = Mathf.Clamp(lifetime, 0f, maxLifeTime);
        }
        public void SetMaxLifeTime(float maxLifeTime) {
            this.maxLifeTime = Mathf.Clamp(maxLifeTime, 0f, float.MaxValue);
            SetLifeTime(this.lifetime);
        }

        public bool IsLifeTimeFull()
        {
            if (maxLifeTime <= 0f) return false;
            return Mathf.Clamp01(lifetime / maxLifeTime) == 1f;
        }

        public void ChangeState(LightSourceState state) {
            ChangeState(state as State);
        }

        public void ChangeState(LightSourceStateType type) {
            ChangeState(GetState(type));
        }

        public LightSourceState GetState(LightSourceStateType type)
        {
            foreach(State state in States) {
                LightSourceState _state = state as LightSourceState;
                if(_state == null || _state.Type != type) continue;
                return _state;
            }

            return null;
        }
    }
}
