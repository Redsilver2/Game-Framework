using RedSilver2.Framework.Items;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Items
{
    public abstract class LightSourceItem : EquippableItem
    {
        protected bool isOn;
        private Light _light;
        private UnityEvent<bool> onStateChanged;

        public bool IsOn => isOn;
        public Light Light => _light;

        protected override void Awake()
        {
            base.Awake();
            _light = GetLight();

            onStateChanged = new UnityEvent<bool>();

            AddOnStateChangedListener(isOn => {
                SetLightState();
            });
        }

        private Light GetLight()
        {
            if (transform.root == null) return transform.GetComponent<Light>();
            return transform.root.GetComponentInChildren<Light>();  
        }

        public void AddOnStateChangedListener(UnityAction<bool> action)
        {
            if (action != null) onStateChanged?.AddListener(action);
        }

        public void RemoveOnStateChangedListener(UnityAction<bool> action)
        {
            if (action != null) onStateChanged?.RemoveListener(action);
        }

        public void SetState(bool isOn)
        {
            if (this.isOn != isOn)
            {
                this.isOn = isOn;
                onStateChanged?.Invoke(isOn);
            }
        }

        protected void SetLightState()
        {
            if (_light != null) _light.enabled = isOn;
        }

        protected sealed override ItemType GetItemType() => ItemType.LightSource;
    }
}