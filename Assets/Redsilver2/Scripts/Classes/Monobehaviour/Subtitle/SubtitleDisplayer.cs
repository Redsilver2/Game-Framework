using RedSilver2.Framework.References;
using RedSilver2.Framework.StateMachines.Controllers;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using static RedSilver2.Framework.Subtitles.Subtitle;


namespace RedSilver2.Framework.Subtitles {


    [System.Serializable]
    public class SubtitleDisplayer : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        [Space]
        [SerializeField] private TextMeshProUGUI contextDisplayer;

        private bool canOverrideAlpha;
        private CancellationTokenSource displayModeUpdateSource;
        private Transform anchor;

        protected TextMeshProUGUI ContextDisplayer => contextDisplayer;
        public int LineCount => contextDisplayer != null ? contextDisplayer.textInfo.lineCount : 0;

        private void Awake()
        {
            canOverrideAlpha = false;
            anchor = null;
        }

        public void CanOverrideAlpha(bool canOverrideAlpha)
        {
            this.canOverrideAlpha = canOverrideAlpha;
        }


        public virtual void DisplayCharacterName(string characterName, SubtitleManager manager) {
            //bool canDisplayCharacterName = true;
            //if (manager != null) canDisplayCharacterName = manager.CanDisplayCharacterName;

            //if (contextDisplayer != null) {
            //    if (canDisplayCharacterName) { contextDisplayer.text = characterName.FormatText(); }
            //    else {                         contextDisplayer.text = string.Empty; }
            //}
        }

        public void DisplayContext(string context, float progress, SubtitleManager manager) {
            //if (contextDisplayer) {
            //    string currentText = context.FormatText(manager != null ? (manager.CanShowSubtitleContextByTime ? progress : 1f) : progress);
                
            //    if (string.IsNullOrEmpty(contextDisplayer.text)) { contextDisplayer.text = currentText; }
            //    else { contextDisplayer.text += currentText; } 
            //}
        }

        public IEnumerator UpdateDisplayMode(SubtitleData data, SubtitleUpdater updater, CancellationToken token)
        {
            DisplayMode mode = DisplayMode.None;
            Debug.Log(data + " | " + updater);

            while (data != null && updater != null && !token.IsCancellationRequested)
            {
                if (data.IsUpdateFinished && GetAlpha() <= 0f) break;
                else if (updater.UpdaterType == SubtitleUpdater.SubtitleUpdaterType.ScreenSpace) { UpdateScreenSpaceDisplayMode(data, ref mode); }
                else if (updater.UpdaterType == SubtitleUpdater.SubtitleUpdaterType.WorldSpace)  { UpdateWorldSpaceDisplayMode(data, ref mode); }
                else if (updater.UpdaterType == SubtitleUpdater.SubtitleUpdaterType.Hybrid)      { UpdateHybridDisplayMode(data, ref mode); }

                yield return null;
            }
        }

        public void CancelUpdateDisplayMode()
        {
            displayModeUpdateSource?.Cancel();
            displayModeUpdateSource = null;
        }

        private void UpdateScreenSpaceDisplayMode(SubtitleData data, ref DisplayMode mode)
        {
            if (mode != DisplayMode.ScreenSpace && data != null) {

                SubtitleDisplayerAnchorReference.RemoveSubtitleDisplayer(data.AnchorName, this);
                SubtitleDisplayerAnchorReference.AddSubtitleDisplayer(SubtitleManager.SCREEN_SPACE_ANCHOR, this);
                mode = DisplayMode.ScreenSpace;
            }
        }

        private void UpdateWorldSpaceDisplayMode(SubtitleData data, ref DisplayMode mode)
        {
            if (mode != DisplayMode.WorldSpace && data != null) {
                SubtitleDisplayerAnchorReference.RemoveSubtitleDisplayer(SubtitleManager.SCREEN_SPACE_ANCHOR, this);
                SubtitleDisplayerAnchorReference.AddSubtitleDisplayer(data.AnchorName, this);
                mode = DisplayMode.WorldSpace;
            }
        }

        private void UpdateNoneDisplayMode(SubtitleData data, ref DisplayMode mode)
        {
            if (mode != DisplayMode.None && data != null) {
                SubtitleDisplayerAnchorReference.RemoveSubtitleDisplayer(SubtitleManager.SCREEN_SPACE_ANCHOR, this);
                SubtitleDisplayerAnchorReference.RemoveSubtitleDisplayer(data.AnchorName, this);
                mode = DisplayMode.None;
            }
        }

        private void UpdateHybridDisplayMode(SubtitleData data, ref DisplayMode mode)
        {
            SubtitleManager manager = SubtitleManager.GetInstance();
            Transform transform = PlayerController.Current != null ? PlayerController.Current.transform : null;
            float distance = transform != null && anchor != null ? Vector3.Distance(anchor.position, transform.position) : 0f;

            if (transform == null || manager == null) { UpdateNoneDisplayMode(data, ref mode); }
            else if (distance <= manager.SubtitleWorldSpaceDistance) { UpdateWorldSpaceDisplayMode(data, ref mode); }
            else {
                if (distance >= manager.MaxSubtitleFadeDistance) {
                    if (canvasGroup != null && canOverrideAlpha) canvasGroup.alpha = 0f;
                    UpdateNoneDisplayMode(data, ref mode);
                }
                else {
                    if (distance >= manager.MinSubtitleFadeDistance)
                        if (canvasGroup != null && canOverrideAlpha) canvasGroup.alpha = Mathf.Lerp(0f, 1f, Mathf.Clamp01(distance - manager.MinSubtitleFadeDistance / manager.MaxSubtitleFadeDistance - manager.MinSubtitleFadeDistance));
                        else
                            if (canvasGroup != null && canOverrideAlpha) canvasGroup.alpha = 1f;

                    UpdateScreenSpaceDisplayMode(data, ref mode);
                }
            }
        }


        public void SetAnchor(Transform anchor)
        {
            this.anchor = anchor;
        }

        public float GetAlpha() { return canvasGroup != null ? canvasGroup.alpha : 0f; }
        public void SetAlpha(float alpha)
        {
            if(canvasGroup != null)
                canvasGroup.alpha = Mathf.Clamp01(alpha);
        }

        private enum DisplayMode
        {
            None,
            ScreenSpace,
            WorldSpace
        }
    }
}
