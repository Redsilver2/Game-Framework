using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RedSilver2.Framework.UI
{
    public abstract class UISelection : MonoBehaviour, IPointerEnterHandler
    {
        private bool isSelected;
 
        private UnityEvent onSelect;
        private UnityEvent onDeselect;

        private UnityEvent onDisable;
        private UnityEvent onEnable;

        private UISelector owner;
        private IEnumerator updateCoroutine;

        public bool IsSelected => isSelected;

        protected virtual void Awake()
        {
            onSelect   = new UnityEvent();
            onDeselect = new UnityEvent();

            onDisable  = new UnityEvent();
            onEnable   = new UnityEvent();

            updateCoroutine = UpdateCoroutine();

            AddOnSelectListener  (() => {
                isSelected = true;
                if (updateCoroutine != null) StartCoroutine(updateCoroutine); 
            });
            AddOnDeselectListener(() => {
                isSelected = false;
                if (updateCoroutine != null) StopCoroutine(updateCoroutine); 
            });

            AddOnDisableListener (() => { 
                isSelected = false;
                if (updateCoroutine != null) StopCoroutine(updateCoroutine);
            });
        }

        public void SetOwner(UISelector owner) {
            this.owner = owner;
        }

        private void OnDisable()
        {
            onDisable?.Invoke();
        }

        private void OnEnable()
        {
            onEnable?.Invoke();
        }

        public void Select()
        {
            if(!isSelected) onSelect?.Invoke();
        }

        public void Deselect()
        {
            if (isSelected) onDeselect?.Invoke();
        }

        public void AddOnSelectListener(UnityAction action)
        {
            if (action != null) onSelect?.AddListener(action);
        }
        public void RemoveOnSelectListener(UnityAction action)
        {
            if (action != null) onSelect?.RemoveListener(action);
        }

        public void AddOnDeselectListener(UnityAction action)
        {
            if (action != null) onDeselect?.AddListener(action);
        }
        public void RemoveOnDeselectListener(UnityAction action)
        {
            if (action != null) onDeselect?.RemoveListener(action);
        }

        public void AddOnEnableListener(UnityAction action)
        {
            if (action != null) onEnable?.AddListener(action);
        }
        public void RemoveOnEnableListener(UnityAction action)
        {
            if (action != null) onEnable?.RemoveListener(action);
        }

        public void AddOnDisableListener(UnityAction action)
        {
            if (action != null) onDisable?.AddListener(action);
        }
        public void RemoveOnDisableListener(UnityAction action)
        {
            if (action != null) onDisable?.RemoveListener(action);
        }
        public void OnPointerEnter(PointerEventData eventData) { owner?.Select(this); }
        protected abstract IEnumerator UpdateCoroutine();
    }
}
