using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RedSilver2.Framework.UI {
    public abstract class SliderUISelection : UISelection {
        [SerializeField] private float incrementValue = 0.1f;
        private Slider slider;

        protected override void Awake()
        {
            base.Awake();
            slider = transform.GetComponentInChildren<Slider>();
        }

        public void AddOnValueChangeListener(UnityAction<float> action) 
        {
            if(action != null) slider?.AddOnValueChangedListener(action);
        }
        public void RemoveOnValueChangeListener(UnityAction<float> action)
        {
            if (action != null) slider?.RemoveOnValueChangedListener(action);
        }

        protected override IEnumerator UpdateCoroutine()
        {
            while(slider != null) {
                OnUpdate();
                yield return null;
            }
        }

        private void OnUpdate()
        {
            if(slider == null) return;  

            if (GameUIController.GetNavigateLeftState(false))        slider.value -= incrementValue;
            else if (GameUIController.GetNavigateRightState(false))  slider.value += incrementValue;
        }
    }
}
