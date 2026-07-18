using UnityEngine;

namespace RedSilver2.Framework.UI
{
    public class UIntSliderUISelection : SliderUISelection
    {
        [SerializeField] private uint incrementValue;

#if UNITY_EDITOR
        private void OnValidate()
        {
            SetIncrementValue(incrementValue);
        }
#endif

        public void SetIncrementValue(uint value)
        {
            incrementValue = (uint)Mathf.Clamp(value, uint.MinValue, uint.MaxValue);
        }

        public void SetValue(uint value)
        {
            if (Slider == null) return;
            Slider.value = value;
        }

        public void SetMaxValue(uint maxValue)
        {
            if (Slider == null) return;
            Slider.maxValue = maxValue;
        }

        public void SetMinValue(uint minValue)
        {
            if (Slider == null) return;
            Slider.minValue = minValue;
        }

        protected sealed override void OnUpdate()
        {
            if (Slider == null) return;
            else if (GameUIController.GetNavigateLeftState(true)) Slider.value -= incrementValue;
            else if (GameUIController.GetNavigateRightState(true)) Slider.value += incrementValue;
        }

        protected sealed override void SetWholeNumber()
        {
            if (Slider != null) Slider.wholeNumbers = true;
        }
    }
}
