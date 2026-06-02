using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Subtitles.Datas
{
    [System.Serializable]
    public struct SubtitleData 
    {
        public float startTime;
        public float duration;
        public float fadeDelayTime;

        [Space]
        [TextArea(3, 3)] public string textToDisplay;

        public SubtitleData(string textToDisplay) {
            this.textToDisplay = textToDisplay;

            startTime = 0f;
            duration = 0f;
            fadeDelayTime = 0f;
        }


        public SubtitleData(string textToDisplay, float startTime) {
            this.textToDisplay = textToDisplay;

            this.startTime = startTime;
            duration = 0f;
            fadeDelayTime = 0f;
        }

        public SubtitleData(string textToDisplay, float startTime, float duration) {
            this.textToDisplay = textToDisplay;

            this.startTime = startTime;
            this.duration  = duration;
            fadeDelayTime  = 0f;
        }

        public SubtitleData(string textToDisplay, float startTime, float duration, float fadeDelayTime) {
            this.textToDisplay = textToDisplay;

            this.startTime     = startTime;
            this.duration      = duration;
            this.fadeDelayTime = fadeDelayTime;
        }
    }
}
