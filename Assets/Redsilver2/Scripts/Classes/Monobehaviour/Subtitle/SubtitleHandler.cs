using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.Subtitles.Datas;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


namespace RedSilver2.Framework.Subtitles
{
    [RequireComponent(typeof(Canvas))]
    public class SubtitleHandler : MonoBehaviour
    {
        [SerializeField] private Vector3 defaultPosition;

        [Space]
        [SerializeField] private float maxDistanceCheck;
        [SerializeField] private float distanceBetweenSubtitles;

        [Space]
        [SerializeField] private bool isExcludedFromWorldSpaceCheck;

        [Space]
        [SerializeField] private TextMeshProUGUI textDisplayer;



        private bool     isUpdatingSubtitles;
        private float    timeElapsed = 0f;

        private int subtitleDataToComplete = 0;
        private int subtitleDataProgress   = 0;

        private Subtitle currentSubtitle;

        private List<TextMeshProUGUI> availableDisplayers;
       
        public bool IsUpdatingSubtitles => isUpdatingSubtitles;

        private readonly static Dictionary<SubtitleHandler, List<TextMeshProUGUI>> actifDisplayers = new Dictionary<SubtitleHandler, List<TextMeshProUGUI>>(); 
        private readonly static Dictionary<Subtitle, List<IEnumerator>> actifSubtitles = new Dictionary<Subtitle, List<IEnumerator>>(); 

        private void Awake() {
            Canvas canvas = GetComponent<Canvas>();
            canvas.renderMode  = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;

            availableDisplayers = new List<TextMeshProUGUI>();
            actifDisplayers?.Add(this, new List<TextMeshProUGUI>());

            subtitleDataToComplete = 0;
            subtitleDataProgress   = 0;
        }

        public void Play(Subtitle subtitle) {
            if (subtitle == null) return;
            Stop();

            if (!isUpdatingSubtitles) {
                isUpdatingSubtitles = true;
                StartSubtitleUpdate(subtitle);
            }
        }

        public void Play(Subtitle subtitle, int index) {
            if (subtitle == null) return;
            Stop();

            if (!isUpdatingSubtitles) {
                isUpdatingSubtitles = true;
                StartSubtitleUpdate(subtitle, index);
            }
        }

        public void Play(Subtitle subtitle, int[] indexes)
        {
            if (subtitle == null) return;
            Stop();

            if (!isUpdatingSubtitles) {
                isUpdatingSubtitles = true;
                StartSubtitleUpdate(subtitle, indexes);
            }
        }

        public void Stop() {
            if (currentSubtitle == null) return;
            StopSubtitleUpdate(currentSubtitle);

            timeElapsed = 0f;
            currentSubtitle = null;

            subtitleDataToComplete = 0;
            subtitleDataProgress = 0;

            isUpdatingSubtitles = false;
            RemoveAllCurrentSubtitle(false);
        }

        private void RemoveAllCurrentSubtitle(bool instantFade)
        {
            if (actifDisplayers == null || !actifDisplayers.ContainsKey(this)) return;

            foreach (TextMeshProUGUI displayer in actifDisplayers[this].Where(x => x != null)) {
                if (displayer == null) continue;

                if (instantFade) {
                    displayer.text = string.Empty;
                    displayer.canvasRenderer.SetAlpha(0);
                    displayer.gameObject.SetActive(false);
                    continue;
                }
                           
                StartCoroutine(FadeAlpha(displayer));
            }

            actifDisplayers[this].Clear();
        }

        private void StopSubtitleUpdate(Subtitle subtitle) {
            if (actifSubtitles == null || subtitle == null || !actifSubtitles.ContainsKey(subtitle))
                return;

            foreach (IEnumerator enumerator in actifSubtitles[subtitle].ToArray())
                StopCoroutine(enumerator);

            actifSubtitles?.Remove(subtitle);
        }

        protected virtual void StartSubtitleUpdate(Subtitle subtitle) {
            if (subtitle == null) return;
            SubtitleData[] datas = subtitle.GetSubtitleDatas();

            if (datas != null) {
                StartSubtitleUpdate(subtitle, SubtitleDisplayerUpdate(transform));
                StartSubtitleUpdate(subtitle, UpdateTimeElapsed(datas[datas.Length - 1], 0f));

                subtitleDataToComplete = datas.Where(x => x != null).Count();
                subtitleDataProgress   = 0;

                foreach (SubtitleData data in datas.Where(x => x != null))
                    StartSubtitleUpdate(subtitle, SubtitleUpdate(subtitle, data));
            }
        }
        protected virtual void StartSubtitleUpdate(Subtitle subtitle, int index) {
            if (subtitle == null) return;
            SubtitleData data = subtitle.GetSubtitleData(index);

            if (data != null) {
                subtitleDataToComplete = 1;
                subtitleDataProgress = 0;

                UpdateTimeElapsed(data, data.StartTime);
                StartSubtitleUpdate(subtitle, SubtitleDisplayerUpdate(transform));
                StartSubtitleUpdate(subtitle, SubtitleUpdate(subtitle, data));

            }
        }

        protected virtual void StartSubtitleUpdate(Subtitle subtitle, int[] indexes) {
            if (subtitle == null) return;
        }

        private void StartSubtitleUpdate(Subtitle subtitle, IEnumerator enumerator) {
            if (actifSubtitles == null || subtitle == null || enumerator == null) return;
            currentSubtitle = subtitle;

            if (!actifSubtitles.ContainsKey(subtitle)) actifSubtitles.Add(subtitle, new List<IEnumerator>());
            
            if (!actifSubtitles[subtitle].Contains(enumerator)) {
                actifSubtitles[subtitle].Add(enumerator);   
                StartCoroutine(enumerator);
            }
        }

        protected void AddActifDisplayer(TextMeshProUGUI displayer) {
            if (actifDisplayers == null || displayer == null || !actifDisplayers.ContainsKey(this)) 
                return;

            if (!actifDisplayers[this].Contains(displayer))
                actifDisplayers[this].Add(displayer);


            if (availableDisplayers != null) 
                if (availableDisplayers.Contains(displayer)) 
                    availableDisplayers.Remove(displayer);
        }

        protected void RemoveActifDisplayer(TextMeshProUGUI displayer) {
            if (actifDisplayers == null || displayer == null || !actifDisplayers.ContainsKey(this))
                return;

            if (actifDisplayers[this].Contains(displayer))
                actifDisplayers[this].Remove(displayer);

            if (availableDisplayers != null) 
                if (!availableDisplayers.Contains(displayer)) 
                    availableDisplayers.Add(displayer);
        }

        private TextMeshProUGUI GetDisplayer() {
            TextMeshProUGUI result;

            if (availableDisplayers == null || availableDisplayers.Count <= 0){
                result = Instantiate(textDisplayer); 
            }
            else { 
                result = availableDisplayers[0];
                availableDisplayers?.RemoveAt(0);
            }

            result.canvasRenderer.SetAlpha(0f);
            result.text = string.Empty;

            return result;
        }

        private IEnumerator UpdateTimeElapsed(SubtitleData lastData, float startTime) {
            timeElapsed = startTime;

            while(lastData != null) {
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= lastData.EndTime) break;
                yield return null;
            }

            if (lastData != null) {
                timeElapsed = lastData.EndTime;
            }
        }

        private  IEnumerator SubtitleUpdate(Subtitle subtitle, SubtitleData data) {
            TextMeshProUGUI textDisplayer = GetDisplayer();
            AddActifDisplayer(textDisplayer);
            StartSubtitleUpdate(subtitle, UpdateAlpha(textDisplayer.canvasRenderer, 1f, 0.25f));


            while(data != null && textDisplayer != null) {
                if (timeElapsed >= data.StartTime) {
                    yield return StartCoroutine(data.Update(textDisplayer));
                    break;
                }

                yield return null;
            }

            yield return StartCoroutine(FadeAlpha(textDisplayer));

        }

        protected IEnumerator SubtitleDisplayerUpdate(Transform transform) {
            while (transform != null) {
                PlayerController controller = PlayerController.Current;
                SubtitleManager manager = GameManager.SubtitleManager;

                if(manager == null) {
                    yield return null;
                    continue;
                }

                Vector3 currentPosition = transform.position;
                Vector3 playerPosition  = controller == null ? Vector3.zero : controller.transform.position;

                if (manager.CanSubtitleUseWorldSpace)  {
                    if (!isExcludedFromWorldSpaceCheck && maxDistanceCheck < Vector3.Distance(currentPosition, playerPosition))  {
                        yield return StartCoroutine(WorldSpaceSubtitleUpdate(controller, manager));
                        continue;
                    }

                }

                yield return StartCoroutine(ScreenSpaceSubtitleUpdate(manager));
            }
        }

        protected IEnumerator FadeAlpha(TextMeshProUGUI displayer) {
            if(displayer != null) {
                yield return StartCoroutine(UpdateAlpha(displayer.canvasRenderer, 0f, 0.25f));
                displayer.text = string.Empty;
                RemoveActifDisplayer(displayer);

                subtitleDataProgress++;

                if (subtitleDataProgress == subtitleDataToComplete) {
                    Stop();
                }
            }
        }

        private IEnumerator UpdateAlpha(CanvasRenderer renderer, float alpha, float duration) {
            float t = 0f;
            float current = renderer == null ? 0f : renderer.GetAlpha();

            alpha = Mathf.Clamp01(alpha);   

            while(renderer != null && t < duration) {
                renderer.SetAlpha(Mathf.Lerp(current, alpha, t/duration));
                t += Time.deltaTime;
                yield return null;
            }

            if (renderer != null) renderer.SetAlpha(alpha);
        }




        private IEnumerator ScreenSpaceSubtitleUpdate(SubtitleManager subtitleManager) {
            foreach (TextMeshProUGUI displayer in actifDisplayers[this].Where(x => x != null).ToArray()) { 
                subtitleManager?.AddScreenSubtitle(displayer);
            }

            yield return null;
        }

        private IEnumerator WorldSpaceSubtitleUpdate(PlayerController player, SubtitleManager subtitleManager)  {
            TextMeshProUGUI[] displayers = actifDisplayers[this].Where(x => x != null).ToArray();
            if (displayers == null || player == null || subtitleManager == null) yield return null;

            yield return StartCoroutine(subtitleManager.UpdateScreenSubtitles(distanceBetweenSubtitles, defaultPosition, transform, displayers)); 
        }
    } 
}
