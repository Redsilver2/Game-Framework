using RedSilver2.Framework.Dialogs.Datas;
using RedSilver2.Framework.StateMachines.Controllers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.Dialogs
{
    [System.Serializable]
    public class Subtitle {
        [SerializeField] private List<SubtitleData> datas;
       
        private Transform parent; 
        private readonly UnityEvent onPlay, onStop;
        private const string PATH = "Subtitle/";

        public Subtitle() {
            onPlay = new UnityEvent();
            onStop = new UnityEvent();
            datas  = new List<SubtitleData>();
        }

        public Subtitle(Transform parent)
        {
            onPlay = new UnityEvent();
            onStop = new UnityEvent();
            datas = new List<SubtitleData>();
            this.parent = parent;
        }

        public Subtitle(List<SubtitleData> datas) {
            onPlay = new UnityEvent();
            onStop = new UnityEvent();
            this.datas = datas  != null ? datas : new List<SubtitleData>();

            SortDatasByTime();
        }

        public Subtitle(List<SubtitleData> datas, Transform parent)
        {
            onPlay = new UnityEvent();
            onStop = new UnityEvent();
            this.datas = datas != null ? datas : new List<SubtitleData>();

            this.parent = parent;
            SortDatasByTime();
        }

        public Subtitle(SubtitleData[] datas) {
            onPlay = new UnityEvent();
            onStop = new UnityEvent();

            this.datas = datas != null ? datas.ToList() : new List<SubtitleData>();
            SortDatasByTime();
        }

        public Subtitle(SubtitleData[] datas, Transform parent)
        {
            onPlay = new UnityEvent();
            onStop = new UnityEvent();

            this.datas = datas != null ? datas.ToList() : new List<SubtitleData>();
            this.parent = parent;

            SortDatasByTime();
        }

        public void AddOnPlayListener(UnityAction action){
            if (action != null) onPlay?.AddListener(action);
        }
        public void RemoveOnPlayListener(UnityAction action) {
            if (action != null) onPlay?.RemoveListener(action);
        }

        public void AddOnStopListener(UnityAction action)
        {
            if (action != null) onStop?.AddListener(action);
        }
        public void RemoveOnStopListener(UnityAction action) {
            if (action != null) onStop?.RemoveListener(action);
        }

        public void Play() { onPlay?.Invoke(); }
        public void Stop() { onStop?.Invoke(); }    

        public void AddData(SubtitleData data) {
            datas?.Add(data);
            SortDatasByTime();
        }

        public void RemoveData(SubtitleData data) {
            datas?.Remove(data);
            SortDatasByTime();
        }

        public void RemoveData(int index) {
            RemoveData(GetData(index));         
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

        public bool IsWorldSpace()
        {
            PlayerController controller = PlayerController.Current;
            DialogManager manager = GameManager.DialogManager;

            if(parent == null || manager == null || controller == null || !manager.CanSubtitleUseWorldSpace) 
                return false;

            if(Vector3.Distance(parent.position, controller.transform.position) <=  manager.SubtitleWorldSpaceDistance)
            {

            }

            return false;
        }

#if UNITY_EDITOR
        public void ValidateDatas() { 
            if (datas == null) return;
            foreach(SubtitleData data in datas) {
                 data.Validate();
            }
        }
#endif

        private void SortDatasByTime() {
           datas = datas.OrderBy(x => x.StartTime).ToList();
        }


        public string GetPath()
        {
            return $"{Dialog.GetPath()}{PATH}"; 
        }
    }
}
