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

        private List<TextMeshProUGUI> screenSubtitles;

        public float SubtitleCatchupSpeed => subtitleCatchupSpeed;
        public bool CanSubtitleUseWorldSpace => canSubtitleUseWorldSpace;
        public float SubtitleFadeDuration => subtitleFadeDuration;    
        public static SubtitleDisplayMode SubtitleDisplayMode => SubtitleDisplayMode.Instant;


        private async void Awake() {
            canUpdateSubtitle = false;
            screenSubtitles   = new List<TextMeshProUGUI>();
        }

        public void Play(Subtitle subtitle)
        {
           GetSubtitleHandler()?.Play(subtitle);
        }

        public void Play(Subtitle subtitle, int index) 
        {
           GetSubtitleHandler()?.Play(subtitle, index);
        }

        public void Play(Subtitle subtitle, int[] indexes)
        {
            GetSubtitleHandler()?.Play(subtitle, indexes);
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
                for (int i = displayers.Length - 1; i > 0; i--)
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

        public SubtitleHandler GetSubtitleHandler() {
            if (subtitleHandler != null) return Instantiate(subtitleHandler);
            return null;
        }
    }
}