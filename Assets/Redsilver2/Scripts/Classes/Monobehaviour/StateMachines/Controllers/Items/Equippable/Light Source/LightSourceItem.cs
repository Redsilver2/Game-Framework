using RedSilver2.Framework.Animations;
using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Items;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Items
{
    public abstract class LightSourceItem : EquippableItem
    {
        [Space]
        [SerializeField] private AnimationData openLightData;

        [Space]
        [SerializeField] private AnimationData closeLightData;

        [Space]
        [SerializeField] private float maxLightDuration;
        [SerializeField] private float lightDrainSpeed;

        [Space]
        [SerializeField] private bool canDrainLightDuration;
         

        protected float currentLightDuration;
        protected bool isOn;

        private IEnumerator drainLightUpdater;

        private Light _light;
        private UnityEvent<bool> onStateChanged;
        private UnityEvent<float> onLightDurationChanged;

        public float MaxLightDuration => maxLightDuration;
        public float LightDrainSpeed  => lightDrainSpeed;

        public bool IsOn => isOn;
        public Light Light => _light;

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            base.OnValidate();
            maxLightDuration = Mathf.Clamp(maxLightDuration, 0.1f, float.MaxValue);
        }

        protected override void ValidateAnimationDatas(RuntimeAnimatorController animatorController)
        {
            base.ValidateAnimationDatas(animatorController);  
            openLightData?.Validate(animatorController);
            closeLightData?.Validate(animatorController);
        }
#endif


        protected override void Awake()
        {
            base.Awake();
            _light = transform.GetComponentInChildren<Light>();

            onStateChanged         = new UnityEvent<bool>();
            onLightDurationChanged = new UnityEvent<float>();

            AddOnStateChangedListener(SetLightState);

            AddOnLightDurationChangedListener(value =>
            {
                if (value <= 0f) {
                    StopDrainLightUpdater();
                    PlayAnimation(closeLightData);
                }
            });

            currentLightDuration = maxLightDuration;
            onStateChanged?.Invoke(IsOn);
        }

        protected override void OnUpdate()
        {
            if(InputManager.GetKeyDown(KeyboardKey.F) && currentLightDuration > 0f) {
                if (isOn) PlayAnimation(closeLightData);
                else      PlayAnimation(openLightData);
            }
        }

        private void StartDrainLightUpdater()
        {
            StopDrainLightUpdater();
            drainLightUpdater = DrainLightUpdater();
            StartCoroutine(drainLightUpdater);
        }

        private void StopDrainLightUpdater()
        {
            if (drainLightUpdater != null) StopCoroutine(drainLightUpdater);
            drainLightUpdater = null;
        }

        private IEnumerator DrainLightUpdater() {
            while (isOn && currentLightDuration > 0) {
                if(maxLightDuration <= 0f) break;
                else if (canDrainLightDuration) {
                    currentLightDuration = Mathf.Clamp(currentLightDuration - Time.deltaTime * lightDrainSpeed, 0f, maxLightDuration);
                    onLightDurationChanged?.Invoke(Mathf.Clamp01(currentLightDuration / maxLightDuration));
                    if(currentLightDuration <= 0f) break;
                }

                yield return null;
            }
        }

        

        public void AddOnStateChangedListener(UnityAction<bool> action)
        {
            if (action != null) onStateChanged?.AddListener(action);
        }

        public void RemoveOnStateChangedListener(UnityAction<bool> action)
        {
            if (action != null) onStateChanged?.RemoveListener(action);
        }

        public void AddOnLightDurationChangedListener(UnityAction<float> action)
        {
            if (action != null) onLightDurationChanged?.AddListener(action);
        }

        public void RemoveOnLightDurationChangedListener(UnityAction<float> action)
        {
            if (action != null) onLightDurationChanged?.RemoveListener(action);
        }


        public void SetCanDrainLightDuration(bool canDrainLightDuration) {
            this.canDrainLightDuration = canDrainLightDuration;
        }

        public virtual void SetState(bool isOn)
        {
            if (this.isOn != isOn)
            {
                this.isOn = isOn;
                onStateChanged?.Invoke(isOn);
            }
        }


        protected virtual void SetLightState(bool isOn) {
            if (_light != null) _light.enabled = isOn;

            if (isOn) StartDrainLightUpdater();
            else StopDrainLightUpdater();
        }

        protected sealed override ItemType GetItemType() => ItemType.LightSource;
    }
}