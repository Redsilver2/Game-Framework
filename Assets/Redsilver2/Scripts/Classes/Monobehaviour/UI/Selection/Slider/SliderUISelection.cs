using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RedSilver2.Framework.UI {
    public abstract class SliderUISelection : UISelection {
        protected Slider Slider { get; private set; }

        protected sealed override void Awake() {
            base.Awake();
            Slider = transform.GetComponentInChildren<Slider>();

            SetWholeNumber();
        }

        public void AddOnValueChangeListener(UnityAction<float> action) 
        {
            if(action != null) Slider?.AddOnValueChangedListener(action);
        }
        public void RemoveOnValueChangeListener(UnityAction<float> action)
        {
            if (action != null) Slider?.RemoveOnValueChangedListener(action);
        }

        protected override IEnumerator UpdateCoroutine()
        {
            while(Slider != null) {
                OnUpdate();
                yield return null;
            }
        }

        protected abstract void SetWholeNumber();
        protected abstract void OnUpdate();
    }
}
