using RedSilver2.Framework.Subtitles.Datas;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.Subtitles
{
    public class SubtitleManager : MonoBehaviour     
    {
        [SerializeField] private SubtitleHandler subtitleHandler;

        [Space]
        [SerializeField] private float defaultSubtitleSpacing = 10f;
        [SerializeField] private float subtitleFadeDuration = 0.25f;

        [Space]
        [SerializeField] private float positionLerpSpeed;

        [Space]
        [SerializeField] private float subtitleCatchupSpeed;

        [Space]
        [SerializeField] private bool  canSubtitleUseWorldSpace;

        [Space]
        [SerializeField] private Vector3   defaultScreenSubtitlePosition;
        [SerializeField] private Transform screenSubtitleParent;

        

        private bool canUpdateSubtitle;
        private Queue<SubtitleHandler> availabeHandlers;
        private List<TextMeshProUGUI> screenSubtitles;
       
        private Dictionary<DialogData, List<SubtitleHandler>> actifDialogDatas;
        private readonly static List<SubtitleHandler> handlers = new List<SubtitleHandler>();

        public float  SubtitleCatchupSpeed                    => subtitleCatchupSpeed;
        public bool   CanSubtitleUseWorldSpace                => canSubtitleUseWorldSpace;
        public float  SubtitleFadeDuration                    => subtitleFadeDuration;    
        public static SubtitleDisplayMode SubtitleDisplayMode => SubtitleDisplayMode.Instant;


        private async void Awake() {
            canUpdateSubtitle = false;
            screenSubtitles   = new List<TextMeshProUGUI>();
            availabeHandlers  = new Queue<SubtitleHandler>();

            actifDialogDatas = new Dictionary<DialogData, List<SubtitleHandler>>();
        }

        public void Play(DialogData data) {
            if (actifDialogDatas == null || actifDialogDatas.ContainsKey(data)) return;
            StopSimilar(data);


            actifDialogDatas?.Add(data, new List<SubtitleHandler>());

            foreach (Subtitle subtitle in data.GetSubtitles().Where(x => x != null))
            {
                Play(data, subtitle);
            }
        }


        public void Play(DialogData data, int subtitleIndex) {
            if (actifDialogDatas == null || actifDialogDatas.ContainsKey(data)) return;
            StopSimilar(data);

            actifDialogDatas.Add(data, new List<SubtitleHandler>());
            Play(data, data.GetSubtitle(subtitleIndex));
        }

        public void Play(DialogData data, int subtitleIndex, int dataIndex)
        {
            if (actifDialogDatas == null || actifDialogDatas.ContainsKey(data)) return;
            StopSimilar(data);

            Subtitle subtitle = data.GetSubtitle(subtitleIndex);
            actifDialogDatas.Add(data, new List<SubtitleHandler>());

            if (subtitle != null) {

            }



        }

        public void Play(DialogData data, int subtitleIndex, int[] dataIndexes) {
            StopSimilar(data);

        }

        public void Stop(DialogData data) {
            if(actifDialogDatas == null || !actifDialogDatas.ContainsKey(data)) { return; }
            SubtitleHandler[] handlers = actifDialogDatas[data].Where(x => x != null).ToArray();

            foreach (SubtitleHandler handler in handlers) {
                handler?.Stop();
            }

            actifDialogDatas?.Remove(data); 
        }

        public void Stop() {
            Dictionary<DialogData, List<SubtitleHandler>> copy = actifDialogDatas;
            if(copy == null) return;

            foreach (KeyValuePair<DialogData, List<SubtitleHandler>> keyValue in copy) {
                Stop(keyValue.Key);
            }

            actifDialogDatas?.Clear();
        }

        private void StopSimilar(DialogData data) {
            Dictionary<DialogData, List<SubtitleHandler>> copy = actifDialogDatas;
            if (copy == null) return;

            foreach (KeyValuePair<DialogData, List<SubtitleHandler>> keyValue in copy) {
                DialogData current = keyValue.Key;
                if (current.IsSimilar(data)) Stop(data);
            }
        }


        private void Play(DialogData data, Subtitle subtitle) {
            if(subtitle == null || actifDialogDatas == null || !actifDialogDatas.ContainsKey(data)) return;
            SubtitleHandler handler = GetSubtitleHandler();
           
            if (actifDialogDatas[data].Contains(handler) || handler == null) return;
            handler?.Play(subtitle);    

            actifDialogDatas[data]?.Add(handler);
        }

        private void Play(DialogData data, Subtitle subtitle, int index) {

        }

        private void Play(DialogData data, Subtitle[] subtitle){

        }

        public void StopSubtitle() {

        }

        public void Stop(Subtitle subtitle) {
            var results = handlers.Where(x => x != null).Where(x => x.CompareSubtitle(subtitle));

            foreach(SubtitleHandler handler in results) {
                Stop(handler);
            }
        }

        public void Stop(SubtitleHandler handler) {
            handler?.Stop();
        }



        public void AddScreenSubtitle(TextMeshProUGUI displayer) {
            if (screenSubtitles == null || displayer == null || screenSubtitles.Contains(displayer)) {
                return;
            }

            screenSubtitles.Add(displayer);

            if (!canUpdateSubtitle){
                StartCoroutine(UpdateScreenSubtitles());
            }
        }

        public void RemoveScreenSubtitle(TextMeshProUGUI displayer)
        {
            if (screenSubtitles == null || displayer == null || !screenSubtitles.Contains(displayer)) {
                return;
            }

            screenSubtitles.Remove(displayer);
            if (screenSubtitles.Count <= 0) canUpdateSubtitle = false;
        }

        public void RemoveActifSubtitleHandler(SubtitleHandler handler) {
            if (actifDialogDatas == null || handler == null) return;
            Dictionary<DialogData, List<SubtitleHandler>> copy = actifDialogDatas;

            if(copy != null)
            {
                foreach (KeyValuePair<DialogData, List<SubtitleHandler>> pair in copy)
                {
                    if (pair.Value.Contains(handler))
                    {
                        actifDialogDatas[pair.Key].Remove(handler);

                        if (actifDialogDatas[pair.Key].Count <= 0) {
                            actifDialogDatas?.Remove(pair.Key);
                        }

                        break;
                    }
                }
            }
        }

        private IEnumerator UpdateScreenSubtitles() {

           canUpdateSubtitle = true;

            while (screenSubtitles.Count > 0) {
                TextMeshProUGUI[] displayers        = GetReversedDisplayers(screenSubtitles);
                TextMeshProUGUI   previousDisplayer = null;

                for (int i = 0; i < displayers.Length; i++) {
                    TextMeshProUGUI displayer = displayers[i];
                    if (displayer == null) continue;

                    UpdateScreenSubtitleParent(Vector3.one, screenSubtitleParent, displayer);
                    UpdateScreenSubtitlePosition(i > 0 && previousDisplayer != null ? previousDisplayer.transform.localPosition + Vector3.up * defaultSubtitleSpacing * displayer.textInfo.lineCount : defaultScreenSubtitlePosition, displayer);
                    previousDisplayer = displayer;
                }

                yield return null;
           }

            canUpdateSubtitle = false;
        }

        public IEnumerator UpdateScreenSubtitles(float distanceBetweenSubtitles, Vector3 defaultPosition, Transform parent, TextMeshProUGUI[] displayers) {
            if (displayers == null || parent == null) yield return null;

            TextMeshProUGUI previousDisplayer = null;
            displayers = GetReversedDisplayers(displayers);

            for (int i = 0; i < displayers.Length; i++)  {
                TextMeshProUGUI displayer = displayers[i];
              
                if (displayer == null) continue;
                RemoveScreenSubtitle(displayer);

                UpdateScreenSubtitleParent(Vector3.right * 0.01f + Vector3.up * 0.01f + Vector3.forward * 0.01f, parent, displayer);
                UpdateScreenSubtitlePosition(i > 0 && previousDisplayer != null ? previousDisplayer.transform.localPosition + Vector3.up * distanceBetweenSubtitles * displayer.textInfo.lineCount : defaultPosition, displayer);
                previousDisplayer = displayer;

                yield return null;
            }
        }

        private TextMeshProUGUI[] GetReversedDisplayers(List<TextMeshProUGUI> displayers)
        {
            return GetReversedDisplayers(displayers != null ? displayers.ToArray() : null);
        }

        private TextMeshProUGUI[] GetReversedDisplayers(TextMeshProUGUI[] displayers)
        {
            List<TextMeshProUGUI> results = new List<TextMeshProUGUI>();
            if (displayers != null) {
                for (int i = displayers.Length - 1; i >= 0; i--)
                    if(displayers[i] != null) { results.Add(displayers[i]); }
            }

            return results.ToArray();   
        }


        private void UpdateScreenSubtitleParent(Vector3 scale, Transform parent, TextMeshProUGUI displayer)
        {
            if(displayer == null || parent == null) return;
            Transform transform = displayer.transform;

            if (transform.parent != parent) {
                transform.SetParent(parent);
                transform.localEulerAngles = Vector3.zero;
                transform.localPosition    = Vector3.zero;
            }

            transform.localScale = scale;
        }

        private void UpdateScreenSubtitlePosition(Vector3 nextPosition, TextMeshProUGUI displayer)
        {
            if (displayer == null) return;
            Transform transform = displayer.transform;
            transform.localPosition  = Vector3.Lerp(transform.localPosition, nextPosition, Time.deltaTime * positionLerpSpeed);
        }


        public void UpdateScreenSubtitlesRotations(TextMeshProUGUI[] displayers)
        {
            if (displayers == null) return;
            for (int i = 0; i < displayers.Length; i++) displayers[i].transform.localEulerAngles = Vector3.zero;
        }

        public SubtitleHandler GetSubtitleHandler() 
        {
            if (handlers != null) {
                if (handlers.Count > 0) {
                    SubtitleHandler handler = handlers[0];
                    handlers.RemoveAt(0);
                    return handler;
                }
            }

            if (subtitleHandler != null) return Instantiate(subtitleHandler);
            return null;
        }

        public static void AddSubtitleHandlerInstance(SubtitleHandler handler)
        {
            if (handler == null || handlers == null) return;
            if (!handlers.Contains(handler)) handlers?.Add(handler);
        }


        public static void RemoveSubtitleHandlerInstance(SubtitleHandler handler)
        {
            if (handler == null || handlers == null) return;
            if (handlers.Contains(handler)) handlers?.Remove(handler);
        }
    }
}