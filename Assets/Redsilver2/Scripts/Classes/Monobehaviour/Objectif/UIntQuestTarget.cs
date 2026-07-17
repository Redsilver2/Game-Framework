
using UnityEngine;

namespace RedSilver2.Framework.Quests.Targets {
    [System.Serializable]
    public class UIntQuestTarget : ValueQuestTarget
    {
        [SerializeField][Range(1, 9999)] private uint value;
        public  uint Value => value;

        public UIntQuestTarget(string name, uint value) : base(name)  {
            this.value = (uint)Mathf.Clamp(value, 1, uint.MaxValue);
        }

        public UIntQuestTarget(string name, uint value, bool isIncrementing) : base(name, isIncrementing) {
            this.value = (uint)Mathf.Clamp(value, 1, uint.MaxValue);
        }

        public void SetValue(uint value) { this.value = (uint)Mathf.Clamp(value, 1, uint.MaxValue); }

        public void Update(ref uint value, uint maxValue) {
            if (IsIncrementing) value += this.value;
            else                value -= this.value;

            value = (uint)Mathf.Clamp(value, uint.MinValue, maxValue);
        }
    }
}
