using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.UI
{
    public abstract class UISelection : MonoBehaviour
    {
        private UnityEvent onSelect;
        private UnityEvent onDeselect;

        private UnityEvent onDisable;
        private UnityEvent onEnable;

        private IEnumerator updateCoroutine;

        protected virtual void Awake()
        {
            onSelect   = new UnityEvent();
            onDeselect = new UnityEvent();

            onDisable  = new UnityEvent();
            onEnable   = new UnityEvent();

            updateCoroutine = UpdateCoroutine();

            AddOnSelectListener  (() => { if (updateCoroutine != null) StartCoroutine(updateCoroutine); });
            AddOnDeselectListener(() => { if (updateCoroutine != null) StopCoroutine(updateCoroutine); });
            AddOnDisableListener (() => { if (updateCoroutine != null) StopCoroutine(updateCoroutine); });
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
            onSelect?.Invoke();
        }

        public void Deselect()
        {
            onDeselect?.Invoke();
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

        protected abstract IEnumerator UpdateCoroutine();
    }
}
