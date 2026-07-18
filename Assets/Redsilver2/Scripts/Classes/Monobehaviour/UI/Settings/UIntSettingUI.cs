using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.UI
{
    public abstract class UIntSettingUI : MonoBehaviour
    {
        [SerializeField] private bool applyAutomaticallyNewValue;

        private uint index;
        private bool wasValueChangedApplied;
        private UnityEvent<uint> onValueChanged;

        public uint Index => index;

        protected virtual void Awake()
        {
            onValueChanged = new UnityEvent<uint>();
            AddOnValueChangedEvent(value => {
                wasValueChangedApplied = false;
                if (applyAutomaticallyNewValue) Apply();
            });

            Load();
        }

        public virtual void Apply() {
            string dataName = GetDataName();

            if (!string.IsNullOrEmpty(dataName)) {
                PlayerPrefs.SetInt(dataName, (int)index);
                PlayerPrefs.Save();
            }


            wasValueChangedApplied = true;
        }

        public void Load() {
            string dataName = GetDataName();

            if (!string.IsNullOrEmpty(dataName)) { index = (uint)PlayerPrefs.GetInt(dataName, (int)uint.MinValue); }
            else { index = 0; }

            Apply();
        }

        protected void SetIndex(uint index) {

            if(index != this.index && index < GetMaxIndex()){
                this.index = index;
                onValueChanged?.Invoke(this.index);  
            }
        }
        public void AddOnValueChangedEvent(UnityAction<uint> action) {
            if (action != null) onValueChanged?.AddListener(action);
        }
        public void RemoveOnValueChangedEvent(UnityAction<uint> action)
        {
            if (action != null) onValueChanged?.RemoveListener(action);
        }
        protected abstract uint GetMaxIndex();
        protected abstract string GetDataName();
    }
}