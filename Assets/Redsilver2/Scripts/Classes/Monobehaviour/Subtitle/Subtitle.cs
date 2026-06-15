using RedSilver2.Framework.Subtitles.Datas;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.Subtitles
{
    [System.Serializable]
    public class Subtitle {
        [SerializeField] private List<SubtitleData> datas;

        private readonly UnityEvent<int>         onPlay, onStop;
        private readonly UnityEvent<int, string> onValueChanged;

        public Subtitle() {
            datas = new List<SubtitleData>();
            onPlay = new UnityEvent<int>();
           
            onStop = new UnityEvent<int>();
            onValueChanged = new UnityEvent<int, string>();
        }

        public Subtitle(List<SubtitleData> datas) {
            onPlay = new UnityEvent<int>();
            onStop = new UnityEvent<int>();
           
            onValueChanged = new UnityEvent<int, string>();
            this.datas     = datas  != null ? datas : new List<SubtitleData>();

            SortDatasByTime();
        }

        public Subtitle(SubtitleData[] datas) {
            onPlay = new UnityEvent<int>();
            onStop = new UnityEvent<int>();

            onValueChanged = new UnityEvent<int, string>();
            this.datas = datas != null ? datas.ToList() : new List<SubtitleData>();

            SortDatasByTime();
        }

        public void AddOnValueChangedListener(UnityAction<int, string> action) {
           if(action != null) onValueChanged?.AddListener(action);  
        }

        public void RemoveOnValueChangedListener(UnityAction<int, string> action) {
            if (action != null) onValueChanged?.RemoveListener(action);
        }

        public void AddOnPlayListener(UnityAction<int> action){
            if (action != null) onPlay?.AddListener(action);
        }

        public void RemoveOnPlayListener(UnityAction<int> action) {
            if (action != null) onPlay?.RemoveListener(action);
        }

        public void AddOnStopListener(UnityAction<int> action)
        {
            if (action != null) onStop?.AddListener(action);
        }

        public void RemoveOnStopListener(UnityAction<int> action) {
            if (action != null) onStop?.RemoveListener(action);
        }

        public void AddData(SubtitleData data) {
            datas?.Add(data);
            SortDatasByTime();
        }

        public void RemoveData(SubtitleData data) {
            datas?.Remove(data);
            SortDatasByTime();
        }

        public void RemoveData(int index)
        {
            RemoveData(GetData(index));         
        }

        public IEnumerator Update(int index, TextMeshProUGUI displayer)
        {
            float t = 0f;
            string previousText   = string.Empty;
            string currentText    = string.Empty;
          
            SubtitleData data = GetData(index);
            onPlay?.Invoke(index);

            while (t < GetDuration(data, false))
            {
                SubtitleManager manager = GameManager.SubtitleManager;
                bool canDisplayByTime = manager ? manager.CanShowSubtitleByTime : false;

                currentText = $"{GetCharacterName()}{GetTextToDisplay(data, t / GetDuration(index, true))}";

                if (!previousText.Equals(currentText)) {
                    previousText = currentText;
                    onValueChanged.Invoke(index, currentText);
                }

                if (!canDisplayByTime) { currentText = $"{GetCharacterName()}{data.textToDisplay}";  }
                if (displayer != null) { displayer.text = currentText; }

                t += Time.deltaTime;
                yield return null;
            }

            onStop?.Invoke(index);
            if (displayer != null) { displayer.text = $"{GetCharacterName()}{data.textToDisplay}"; }
            yield return new WaitForSeconds(data.fadeDelayTime);
        }


        public SubtitleData GetData(int index)
        {
            SubtitleData[] results = GetDatas();

            if(results == null || results.Length == 0 || index < 0 || index >= results.Length)
                return default;

            return results[index];  
        }

        public SubtitleData[] GetDatas(int[] indexes)
        {
            List<SubtitleData> results = new List<SubtitleData>();
            if (indexes == null) return null;

            for (int i = 0; i < indexes.Length; i++) {
               results.Add(GetData(indexes[i]));         
            }

            return results.ToArray();
        }

        public SubtitleData[] GetDatas() {
            if(datas == null) return null;
            return datas.ToArray();
        }

        public virtual bool IsSimilar(Subtitle subtitle) {
            if(subtitle == null) return false;
            return subtitle.Equals(this);
        }


        private string GetTextToDisplay(int index, float progression)
        {
            return GetTextToDisplay(GetData(index), progression);
        }

        private string GetTextToDisplay(SubtitleData data, float progression)
        {
            progression = Mathf.Clamp01(progression);

            if (string.IsNullOrEmpty(data.textToDisplay) || progression <= 0f) 
                return string.Empty;
            else if (progression >= 1f) return data.textToDisplay;

            string result = string.Empty;
            char[] characters = data.textToDisplay.ToCharArray();

            for (int i = 0; i < (characters.Length * progression); i++)
              result += characters[i];


            Debug.Log(result);
            return result;
        }

        private void SortDatasByTime() {
           datas = datas.OrderBy(x => x.startTime).ToList();
        }

        private float GetDuration(int index, bool allowCatchupSpeed) {
            if(datas == null || datas.Count <= 0f || index < 0 || index >= datas.Count) 
                return 0f;

            return GetDuration(GetData(index), allowCatchupSpeed);
        }
        private float GetDuration(SubtitleData data, bool allowCatchupSpeed)
        {
            if (!allowCatchupSpeed) return data.duration;

            SubtitleManager manager = GameManager.SubtitleManager;
            return data.duration * (manager != null ? manager.SubtitleCatchupSpeed : 1f);
        }

        protected virtual string GetCharacterName() {
            return string.Empty;
        }
    }
}
