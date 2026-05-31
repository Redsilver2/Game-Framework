using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Subtitles.Datas
{
    [System.Serializable]
    public class SubtitleData 
    {
        [SerializeField] private float startTime;
        [SerializeField] private float endTime;

        [Space]
        [SerializeField] [TextArea(3, 3)] private string textToDisplay;
        private UnityEvent<string> onValueChanged;

        public float StartTime => startTime;
        public float EndTime   => endTime;

        public SubtitleData() {
            onValueChanged = new UnityEvent<string>();
        }

        public IEnumerator Update(TextMeshProUGUI displayer) {
            float t = 0f;
            string previousText = string.Empty;
            if (displayer != null) displayer.text = string.Empty;

            while (t < GetDuration()) {
                Update(displayer, GetTextToDisplay(t / GetDuration()), GameManager.SubtitleManager);

                if(displayer != null) {
                    if (!previousText.Equals(displayer.text)) {
                        previousText = displayer.text;
                        onValueChanged.Invoke(displayer.text);
                    }
                }

                t += Time.deltaTime;
                yield return null;
            }


        }

        protected virtual void Update(TextMeshProUGUI displayer, string textToDisplay, SubtitleManager manager) {    
            if(displayer != null) displayer.text = string.IsNullOrEmpty(textToDisplay) ? string.Empty : textToDisplay;
        }

        public string GetTextToDisplay(float progression) {
            progression = Mathf.Clamp01(progression);
            if (string.IsNullOrEmpty(textToDisplay) || progression <= 0f) return string.Empty;

            if (progression >= 1f) return textToDisplay;
            string result = string.Empty;  

            for(int i = 0; i < (textToDisplay.Length * progression); i++)
                result += textToDisplay[i];

            return result;
        }

        public float GetDuration() {
            SubtitleManager manager = GameManager.SubtitleManager;
            return Mathf.Clamp(endTime - startTime, 0f, float.MaxValue) * (manager != null ? manager.SubtitleCatchupSpeed : 1f);
        }
    }
}
