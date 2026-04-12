using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Stats
{
    public abstract class NumberStat : MonoBehaviour
    {
        [SerializeField] private float maxValue;

        [Space]
        [SerializeField] private bool useWholeValue;

        private float currentValue;
        private UnityEvent<float> onProgressChanged;

        public float CurrentValue => currentValue;
        public float MaxValue     => maxValue;
        public float Progress     => currentValue / maxValue;

        public bool UseWholeValue => useWholeValue;

        protected virtual void Awake()
        {
            this.onProgressChanged = new UnityEvent<float>();        
            maxValue = Mathf.Clamp(maxValue, 1f, float.MaxValue);
            currentValue = maxValue;
        }

        public void SetMaxValue(float maxValue)
        {
             maxValue = Mathf.Clamp(maxValue, 1f, float.MaxValue);
             if (useWholeValue) maxValue = Mathf.Clamp((int)maxValue, 1, int.MaxValue);

             this.maxValue = maxValue;    
             if(currentValue >= maxValue) currentValue = maxValue;
            
            onProgressChanged?.Invoke(Mathf.Clamp01(this.currentValue / this.maxValue));
        }

        public void SetCurrentValue(float currentValue) {
            currentValue = Mathf.Clamp(currentValue, 0f, maxValue);
            if (useWholeValue) currentValue = (int)currentValue;
           
            this.currentValue = currentValue >= maxValue ? maxValue : currentValue;     
            onProgressChanged?.Invoke(Mathf.Clamp01(this.currentValue / this.maxValue)); 
        }

        public void AddOnProgressChangedListener(UnityAction<float> action) {
            if (action != null) onProgressChanged?.AddListener(action);
        }

        public void RemoveOnProgressChangedListener(UnityAction<float> action) {
            if (action != null) onProgressChanged?.AddListener(action);
        }
    }
}
