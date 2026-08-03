using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines {
    public class LightSourceStateMachine : EquippableItemStateMachine
    {
        [Space]
        [SerializeField] private float defaultMaxLifeTime;

        [Space]
        [SerializeField] private float drainLifeTimeSpeed; 

        private float lifetime;
        private float maxLifeTime;

        private Light _light;
        private LightSourceStateType currentType;
        
        private UnityEvent<float> onLifeTimeProgressUpdate;
        private UnityEvent<LightSourceState> onStateAdded, onStateRemoved;
        private UnityEvent<LightSourceState> onStateEntered, onStateExited;

        public float LifeTime    => lifetime;
        public float MaxLifeTime => maxLifeTime;
        public LightSourceStateType  CurrentType        => currentType;
        public Light Light       => _light;

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

            if(_light != null) {
                if (currentType == LightSourceStateType.On) _light.enabled = true;
                else                                        _light.enabled = false;
            }

            AddOnLifeTimeProgressUpdateListener(OnLifeTimeProgressUpdate);
            SetMaxLifeTime(defaultMaxLifeTime);
        }

        protected virtual void OnLifeTimeProgressUpdate(float progress) {
            if(progress <= 0f && currentType == LightSourceStateType.On) {
                ChangeState(LightSourceStateType.Off);
            }
        }

        protected override void OnUpdate() {
            base.OnUpdate();

            if (currentType == LightSourceStateType.On && lifetime > 0f)
                lifetime = Mathf.Clamp(lifetime - Time.deltaTime * drainLifeTimeSpeed, 0f, maxLifeTime);

            // Ceashes game for some reason
          //  onLifeTimeProgressUpdate?.Invoke(maxLifeTime <= 0f ? 0f : Mathf.Clamp01(lifetime / maxLifeTime));
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
        }

        protected virtual void OnStateEntered(LightSourceState state) {
            if (state != null) currentType = state.Type;
            else               currentType = LightSourceStateType.None;

            onStateEntered?.Invoke(state);   
        }

        protected virtual void OnStateExited(LightSourceState state) {
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
            ChangeState(state);
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
