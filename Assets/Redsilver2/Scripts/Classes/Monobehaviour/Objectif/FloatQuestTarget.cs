using UnityEngine;

namespace RedSilver2.Framework.Quests.Targets
{
    public class FloatQuestTarget : ValueQuestTarget
    {
        private float value;

        public FloatQuestTarget(string name, float value) : base(name) 
        {
            this.value = Mathf.Clamp(value, 0f, float.MaxValue);
        }

        public FloatQuestTarget(string name, float value, bool isIncrementing) : base(name, isIncrementing) 
        {
            this.value = Mathf.Clamp(value, 0f, float.MaxValue);
        }

        public void SetValue(float value)
        {
            this.value = Mathf.Clamp(value, 0f, float.MaxValue);
        }

        public void Update(ref float value, float maxValue)
        {
            if (IsIncrementing) value += this.value;
            else value -= this.value;

            value = Mathf.Clamp(value, 0f, maxValue);
        }
    }
}
