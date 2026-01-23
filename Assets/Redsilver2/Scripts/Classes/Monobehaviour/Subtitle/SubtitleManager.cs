using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace RedSilver2.Framework.Subtitles
{
    public class SubtitleManager : MonoBehaviour     
    {
        [SerializeField] private Subtitle  subtitleModel;

        [Space]
        [SerializeField] private float defaultSubtitleSpacing = 10f;
        [SerializeField] private float subtitleFadeDuration = 0.25f;

        [Space]
        [SerializeField] private bool  canSubtitleUseWorldSpace;


        [Space]
        [SerializeField] private Vector3   defaultScreenSubtitlePosition;
        [SerializeField] private Transform screenSubtitleParent;

        private bool canUpdateSubtitle;

        private List<Subtitle> screenSubtitles;
        public bool CanSubtitleUseWorldSpace => canSubtitleUseWorldSpace;
        public float SubtitleFadeDuration => subtitleFadeDuration;    
        public static SubtitleDisplayMode SubtitleDisplayMode => SubtitleDisplayMode.Instant;


        private async void Awake() {
            canUpdateSubtitle = false;
            screenSubtitles   = new List<Subtitle>();
        }

        public void AddScreenSubtitle(Subtitle subtitle) {
            if(screenSubtitles == null || subtitle == null || screenSubtitles.Contains(subtitle)) {
                return;
            }

            screenSubtitles.Add(subtitle);  

            if (!canUpdateSubtitle){
                StartCoroutine(UpdateScreenSubtitles());
            }
        }

        public void RemoveScreenSubtitle(Subtitle subtitle)
        {
            if (screenSubtitles == null || subtitle == null || !screenSubtitles.Contains(subtitle)) {
                return;
            }

            screenSubtitles.Remove(subtitle);
        }

        private IEnumerator UpdateScreenSubtitles() {

           canUpdateSubtitle = true;

           while (screenSubtitles.Count > 0) {
                Subtitle[] subtitles        = screenSubtitles.Where(x => x != null).Reverse().ToArray();
                Subtitle   previousSubtitle = null;

                for (int i = 0; i < subtitles.Length; i++) {
                    Subtitle subtitle = subtitles[i];
                    if (subtitle == null) continue;

                    UpdateScreenSubtitleParent(Vector3.right * 1f + Vector3.up * 1f + Vector3.forward * 1f, screenSubtitleParent, subtitle);
                    UpdateScreenSubtitlePosition(i > 0 && previousSubtitle != null ? previousSubtitle.transform.localPosition + Vector3.up * defaultSubtitleSpacing * subtitle.GetLineCount(): defaultScreenSubtitlePosition, subtitle);
                    previousSubtitle = subtitle;
                }

                yield return null;
           }

            canUpdateSubtitle = false;
        }

        public IEnumerator UpdateScreenSubtitles(float distanceBetweenSubtitles, Vector3 defaultPosition, Transform parent, Subtitle[] subtitles) {
            if (subtitles == null || parent == null) yield return null;

            Subtitle previousSubtitle = null;
            subtitles = subtitles.Where(x => x != null).Reverse().ToArray();

            for (int i = 0; i < subtitles.Length; i++)  {
                Subtitle subtitle = subtitles[i];
                if (subtitle == null) continue;

                RemoveScreenSubtitle(subtitle);

                UpdateScreenSubtitleParent(Vector3.right * 0.01f + Vector3.up * 0.01f + Vector3.forward * 0.01f, parent, subtitle);
                UpdateScreenSubtitlePosition(i > 0 && previousSubtitle != null ? previousSubtitle.transform.localPosition + Vector3.up * distanceBetweenSubtitles * subtitle.GetLineCount() : defaultPosition, subtitle);
                previousSubtitle = subtitles[i];

                yield return null;
            }
        }


        private void UpdateScreenSubtitleParent(Vector3 scale, Transform parent, Subtitle subtitle)
        {
            if(subtitle == null || parent == null) return;

            Transform transform = subtitle.transform;

            if (transform.parent != parent) {
                transform.SetParent(parent);
                transform.localEulerAngles = Vector3.zero;
                transform.localPosition    = Vector3.zero;
            }

            transform.localScale = scale;
        }

        private void UpdateScreenSubtitlePosition(Vector3 nextPosition, Subtitle subtitle)
        {
            if (subtitle == null) return;
            Transform transform = subtitle.transform;
            transform.localPosition  = Vector3.Lerp(transform.localPosition, nextPosition, Time.deltaTime * 1000f);
        }


        public void UpdateScreenSubtitlesRotations(Subtitle[] subtitles)
        {
            if (subtitles == null) return;

            for (int i = 0; i < subtitles.Length; i++) {
                subtitles[i].transform.localEulerAngles = Vector3.zero;
            }
        }

        public Subtitle GetSubtitle()
        {
            if (subtitleModel != null) return Instantiate(subtitleModel);
            return null;
        }
    }
}