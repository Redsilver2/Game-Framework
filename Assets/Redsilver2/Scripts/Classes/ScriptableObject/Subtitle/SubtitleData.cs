using UnityEngine;

namespace RedSilver2.Framework.Dialogs.Datas
{
    [System.Serializable]
    public struct SubtitleData 
    {
        [SerializeField] private GameTimeConverter startTime;
        [SerializeField] private GameTimeConverter endTime;

        [Space]
        [TextArea(3, 3)] public string textToDisplay;

        public string TextToDisplay => textToDisplay;
        public float StartTime      => startTime.GetConversion();
        public float EndTime        => endTime.GetConversion();

        public float Duration
        {
            get {
                float result = EndTime - StartTime;
                return Mathf.Clamp(result, 0f, Mathf.Infinity);
            }
        }


        public SubtitleData(string textToDisplay) {
            this.textToDisplay = textToDisplay;
            startTime          = new GameTimeConverter();
            endTime            = new GameTimeConverter();
        }

        public SubtitleData(string textToDisplay, GameTimeConverter startTime)
        {
            this.textToDisplay = textToDisplay;
            this.startTime     = startTime;
            endTime            = new GameTimeConverter();
        }
        public SubtitleData(string textToDisplay, GameTimeConverter startTime, GameTimeConverter endTime)
        {
            this.textToDisplay = textToDisplay;
            this.startTime     = startTime;
            this.endTime       = endTime;
        }

#if UNITY_EDITOR
        public void Validate()
        {
            startTime.Validate();
            endTime.Validate();
        }
#endif
    }
}
