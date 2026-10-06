using RedSilver2.Framework.Subtitles;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.References
{
    public abstract class SubtitleDisplayerAnchorReference : MonoBehaviour {
        [SerializeField] private string    subtitleAnchorName;
        [SerializeField] private Transform anchor;

        [Space]
        [SerializeField] private Vector3 defaultPosition;
        [SerializeField] private Vector3 positionSpacing;

        [Space]
        [SerializeField] private float positionYOffset;

        private IEnumerator updateEnumerator;
        private UnityEvent<SubtitleDisplayer[]> onUpdateDisplayers;

        private readonly static UnityEvent<string> onDisplayerListUpdated = new UnityEvent<string>();
        private readonly static Dictionary<string, List<SubtitleDisplayer>> displayers = new Dictionary<string, List<SubtitleDisplayer>>();  

        private void Awake() {
            onUpdateDisplayers = new UnityEvent<SubtitleDisplayer[]>();

            if (displayers != null && !string.IsNullOrEmpty(subtitleAnchorName)) {
                if (!displayers.ContainsKey(subtitleAnchorName.ToLower())) {
                    displayers?.Add(subtitleAnchorName.ToLower(), new List<SubtitleDisplayer>());
                }
            }

            onDisplayerListUpdated?.AddListener(OnDisplayerListUpdated);
            AddOnUpdateDisplayersListener(OnUpdateDisplayers);
        }

        private void OnEnable()
        {
            OnDisplayerListUpdated(subtitleAnchorName);
        }

        private void OnDisable()
        {
            if (updateEnumerator != null) StopCoroutine(updateEnumerator);
            updateEnumerator = null;
        }

        private void OnDestroy() {
            if (displayers != null) {
                if (displayers.ContainsKey(subtitleAnchorName.ToLower())) {
                    displayers?.Remove(subtitleAnchorName.ToLower());
                }
            }

            onDisplayerListUpdated?.RemoveListener(OnDisplayerListUpdated);
        }

        protected virtual void OnUpdateDisplayers(SubtitleDisplayer[] displayers) {
            if(displayers != null) {
                Vector3               nextPosition      = defaultPosition;
                SubtitleDisplayer previousDisplayer = null;

                for (int i = 0; i < displayers.Length; i++) {
                    SubtitleDisplayer displayer = displayers[i];

                    if (displayer == null) continue;
                    displayer?.SetAnchor(anchor);

                    if (previousDisplayer != null) { nextPosition += Vector3.up * positionYOffset; }
                    Transform transform = displayer.transform;
                    transform?.SetParent(anchor);


                    nextPosition += Vector3.right * positionSpacing.x +
                                    Vector3.up    * positionSpacing.y * (i > 0 ? displayer.LineCount : displayer.LineCount - 1) +
                                    Vector3.left  * positionSpacing.z;


                    transform.localRotation = Quaternion.Slerp(transform.localRotation, GetDesiredRotation(transform), Time.deltaTime * 10f);
                    transform.localPosition = Vector3.Lerp(transform.localPosition, nextPosition, Time.deltaTime * 10f);
                  
                    previousDisplayer = displayer;
                }
            }
        }

        protected abstract Quaternion GetDesiredRotation(Transform transform);

        private void OnDisplayerListUpdated(string subtitleAnchorName)
        {
            if(!string.IsNullOrEmpty(subtitleAnchorName) && subtitleAnchorName.ToLower() == this.subtitleAnchorName.ToLower()) {
                if (updateEnumerator != null) StopCoroutine(updateEnumerator);
                updateEnumerator = UpdateDisplayers(GetDisplayers(subtitleAnchorName));
                StartCoroutine(updateEnumerator);
            }
        }

        private IEnumerator UpdateDisplayers(SubtitleDisplayer[] displayers)
        {
            if(displayers != null) System.Array.Reverse(displayers);

            while (true) {
               if (displayers == null || displayers.Length == 0) break;

                onUpdateDisplayers?.Invoke(displayers);
               yield return null;
            }
        }

        public void AddOnUpdateDisplayersListener(UnityAction<SubtitleDisplayer[]> action)
        {
            if(action != null) onUpdateDisplayers?.AddListener(action);
        }

        public void RemoveOnUpdateDisplayersListener(UnityAction<SubtitleDisplayer[]> action)
        {
            if (action != null) onUpdateDisplayers?.RemoveListener(action);
        }

        public static void AddSubtitleDisplayer(string subtitleAnchorName, SubtitleDisplayer displayer) {
            if(!string.IsNullOrEmpty(subtitleAnchorName) && displayers != null && displayer != null) {
                subtitleAnchorName = subtitleAnchorName.ToLower();

                if (displayers.ContainsKey(subtitleAnchorName) && !displayers[subtitleAnchorName].Contains(displayer)) {
                    displayers[subtitleAnchorName]?.Add(displayer);
                    onDisplayerListUpdated?.Invoke(subtitleAnchorName);
                }
            }           
        }

        public static void RemoveSubtitleDisplayer(string subtitleAnchorName, SubtitleDisplayer displayer) {
            if (!string.IsNullOrEmpty(subtitleAnchorName) && displayers != null && displayer != null) {
                subtitleAnchorName = subtitleAnchorName.ToLower();

                if (displayers.ContainsKey(subtitleAnchorName) && displayers[subtitleAnchorName].Contains(displayer)) {
                    displayer?.SetAnchor(null);
                    displayers[subtitleAnchorName]?.Remove(displayer);
                    onDisplayerListUpdated?.Invoke(subtitleAnchorName);
                }
            }
        }

        private static SubtitleDisplayer[] GetDisplayers(string subtitleAnchorName)
        {
            if (string.IsNullOrEmpty(subtitleAnchorName) || displayers == null || !displayers.ContainsKey(subtitleAnchorName.ToLower())) return null;
            return displayers[subtitleAnchorName.ToLower()].ToArray();
        }
    }
}
