using UnityEngine;

namespace RedSilver2.Framework.Quests {
    public sealed class QuestExpiration {

        private float timeElapsed;
        private float expirationTime;
        private TimeExpirationMode mode;

        public float TimeElapsed       => timeElapsed;
        public float ExpirationTime    => expirationTime;
        public float Progress          => expirationTime <= 0f ? 1f : Mathf.Clamp01(timeElapsed / expirationTime);
        public TimeExpirationMode Mode => mode;

        public QuestExpiration(float expirationTime, TimeExpirationMode expirationMode) {
            this.mode = expirationMode;     
            SetExpirationTime(expirationTime);
            Reset();
        }

        public void SetExpirationTime(float expirationTime)
        {
            this.expirationTime = Mathf.Clamp(expirationTime, Mathf.Epsilon, float.MaxValue);
        }

        public void SetExpirationMode(TimeExpirationMode mode) {
            this.mode = mode;
        }

        public void Reset()
        {
            if (mode == TimeExpirationMode.ReachTarget) timeElapsed = 0f;
            else timeElapsed = expirationTime;
        }

        public void Update()
        {
            if (expirationTime <= 0f) return;

            if (mode == TimeExpirationMode.ReachTarget) timeElapsed += Time.deltaTime;
            else timeElapsed -= Time.deltaTime;

            timeElapsed = Mathf.Clamp(timeElapsed, 0f, expirationTime);
        }

        public bool IsDone() {
            if (mode == TimeExpirationMode.ReachTarget && timeElapsed == expirationTime) return true;
            else if (mode == TimeExpirationMode.ReachZero && timeElapsed <= 0f) return true;
            return false;
        }
    }
}
