using RedSilver2.Framework.Animations;
using RedSilver2.Framework.StateMachines.States;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using State = RedSilver2.Framework.StateMachines.States.State;

namespace RedSilver2.Framework.StateMachines {
    public class LightSourceItemStateMachine : EquippableItemStateMachine {
        [Space]
        [SerializeField] private float defaultMaxLifeTime;

        [Space]
        [SerializeField] private float drainLifeTimeSpeed;

        private float lifetime;
        private float maxLifeTime;

        private bool isOn;

        private Light _light;
        private LightSourceItemState currentState;

        private IEnumerator drainLightUpdater; 
        private UnityEvent<float> onLifeTimeProgressUpdate;

        public float LifeTime    => lifetime;
        public float MaxLifeTime => maxLifeTime;

        public bool IsOn => isOn;   

        public Light Light                        => _light;

        protected override void Awake()
        {
            base.Awake();
            onLifeTimeProgressUpdate = new UnityEvent<float>();

           _light = transform.root != null ? transform.root.GetComponentInChildren<Light>() : 
                                                             GetComponentInChildren<Light>();

            if(_light != null) _light.enabled = false;   
            AddOnLifeTimeProgressUpdateListener(OnLifeTimeProgressUpdate);
          
            SetMaxLifeTime(defaultMaxLifeTime);
            SetLifeTime(maxLifeTime);
        }

        protected virtual void OnLifeTimeProgressUpdate(float progress) {   
            if(progress <= 0f)  ChangeState(LightSourceItemStateType.Off);
        }


        protected override void OnStateAdded(State state) {
            base.OnStateAdded(state);

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
                if (currentState.Type != LightSourceItemStateType.On || lifetime <= 0f) break;
                lifetime = Mathf.Clamp(lifetime - Time.deltaTime * drainLifeTimeSpeed, 0f, maxLifeTime);

                onLifeTimeProgressUpdate?.Invoke(maxLifeTime < 0f ? 1f :  Mathf.Clamp01(lifetime/maxLifeTime));
                yield return null;
            }

            if(_light != null) _light.enabled = false;
        }

        protected override bool CanAddState(EquippableItemState state) {

            return base.CanAddState(state) && CanAddState(state as LightSourceItemState);
        }

        protected virtual bool CanAddState(LightSourceItemState state) {
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

        public void ChangeState(LightSourceItemState state) {
            ChangeState(state as State);
        }

        public void ChangeState(LightSourceItemStateType type) {
            ChangeState(GetState(type));
        }

        public LightSourceItemAnimationController GetLightSourceItemAnimationController()
        {
            return GetEquippableItemAnimationController() as LightSourceItemAnimationController;
        }


        public LightSourceItemState GetState(LightSourceItemStateType type)
        {
            foreach(State state in States) {
                LightSourceItemState _state = state as LightSourceItemState;
                if(_state == null || _state.Type != type) continue;
                return _state;
            }

            return null;
        }
    }
}
