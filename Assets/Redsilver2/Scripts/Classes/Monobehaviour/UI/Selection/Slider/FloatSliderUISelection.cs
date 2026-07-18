using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


namespace RedSilver2.Framework.UI
{
    public sealed class FloatSliderUISelection : SliderUISelection
    {
        [SerializeField] private float incrementValue;
        [SerializeField] private bool  isInputDownMode;

#if UNITY_EDITOR
        private void OnValidate()
        {
            SetIncrementValue(incrementValue);
        }
#endif

        public void SetIncrementValue(float value) {
            incrementValue = Mathf.Clamp(value, 0f, float.MaxValue);
        }

        public void SetValue(float value)
        {
            if (Slider == null) return;
            Slider.value = value;
        }

        public void SetMaxValue(float maxValue)
        {
            if (Slider == null) return;
            Slider.maxValue = maxValue;
        }

        public void SetMinValue(float minValue)
        {
            if (Slider == null) return;
            Slider.minValue = minValue;
        }

        protected sealed override void OnUpdate()
        {
            if (Slider == null) return;

            else if (GameUIController.GetNavigateLeftState(isInputDownMode)) { 
                if (isInputDownMode) Slider.value -= incrementValue;
                else                 Slider.value -= Time.deltaTime * incrementValue;
            }
            else if (GameUIController.GetNavigateRightState(isInputDownMode))
            {
                if (isInputDownMode) Slider.value += incrementValue;
                else Slider.value                 += Time.deltaTime * incrementValue;
            }
        }

        protected sealed override void SetWholeNumber()
        {
            if (Slider != null) Slider.wholeNumbers = false;
        }
    }
}
