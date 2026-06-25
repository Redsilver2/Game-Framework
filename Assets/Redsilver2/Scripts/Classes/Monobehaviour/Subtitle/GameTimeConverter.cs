using UnityEngine;

namespace RedSilver2.Framework {
    [System.Serializable]
    public struct GameTimeConverter {
        public uint   hours;
        public uint   minutes;
        public uint   seconds;

#if UNITY_EDITOR
        public void Validate() {
            hours   = (uint)Mathf.Clamp(hours,   0, float.MaxValue);
            minutes = (uint)Mathf.Clamp(minutes, 0, 59);
            seconds = (uint)Mathf.Clamp(seconds, 0, 59);
        }
#endif

        public float GetConversion() {
            return GetHoursToSeconds(hours) + GetMinutesToSeconds(minutes) + seconds; 
        }

        public static float GetHoursToSeconds(uint hours) {
            return (uint)Mathf.Clamp(hours, 0, float.MaxValue) * (60 ^ 2);
        }

        public static float GetMinutesToSeconds(uint minutes) {
            return (uint)Mathf.Clamp(minutes, 0, 59) * 60f;
        }
    }
}
