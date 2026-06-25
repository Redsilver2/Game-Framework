using RedSilver2.Framework.Dialogs.Datas;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.Dialogs
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SubtitleHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textDisplayer;

        private bool isUpdateStarted;
        private bool isUpdateFinished;
        private bool canTextDisplayShowCharacterName;

        private CanvasGroup group;
        private IEnumerator fadeUpdater;

        private float timeElpased;

        private UnityEvent<SubtitleData> onUpdateStarted;
        private UnityEvent<SubtitleData> onUpdateFinished;
        private UnityEvent<SubtitleData, float> onProgressUpdate;
        public bool IsUpdateStarted => isUpdateStarted;
        public bool IsUpdateFinished => isUpdateFinished;

        public int LineCount => textDisplayer == null ? 0 : textDisplayer.textInfo.lineCount;
        protected Subtitle CurrentSubtitle { get; private set; }


        protected virtual void Awake() {
            group = GetComponent<CanvasGroup>();
            canTextDisplayShowCharacterName = true;

            onUpdateStarted  = new UnityEvent<SubtitleData>();
            onUpdateFinished = new UnityEvent<SubtitleData>();
            onProgressUpdate = new UnityEvent<SubtitleData, float>();

            AddOnUpdateStartedListener(OnUpdateStarted);
            AddOnUpdateFinishedListener(OnUpdateFinished);
            AddOnProgressUpdateListener(UpdateTextDisplayer);

            isUpdateStarted  = false;
            isUpdateFinished = false;   

            group.alpha = 0;
        }

        private void OnEnable() {
            DialogManager.AddSubtitleHandlerInstance(this);
        }

        private void OnDisable() {
            DialogManager.RemoveSubtitleHandlerInstance(this);
        }

        public void AddOnUpdateStartedListener(UnityAction<SubtitleData> action) {
            if (action != null) onUpdateStarted?.AddListener(action);
        }
        public void RemoveOnUpdateStartedListener(UnityAction<SubtitleData> action) {
            if (action != null) onUpdateStarted?.RemoveListener(action);
        }

        public void AddOnUpdateFinishedListener(UnityAction<SubtitleData> action)
        {
            if (action != null) onUpdateFinished?.AddListener(action);
        }
        public void RemoveOnUpdateFinishedListener(UnityAction<SubtitleData> action) {
            if (action != null) onUpdateFinished?.RemoveListener(action);
        }

        public void AddOnProgressUpdateListener(UnityAction<SubtitleData, float> action)
        {
            if (action != null) onProgressUpdate?.AddListener(action);
        }
        public void RemoveOnProgressUpdateListener(UnityAction<SubtitleData, float> action)
        {
            if (action != null) onProgressUpdate?.RemoveListener(action);
        }

        protected void SetCanTextDisplayShowCharacterName(bool canTextDisplayShowCharacterName)
        {
            this.canTextDisplayShowCharacterName = canTextDisplayShowCharacterName;
        }

        protected virtual void OnUpdateStarted(SubtitleData data) {
            isUpdateStarted = true;
            timeElpased = 0f;

            StartFade(false);
            GameManager.DialogManager?.AddActifSubtitleHandler(this);
        }

        protected virtual void OnUpdateFinished(SubtitleData data) {
            isUpdateFinished = true;
            timeElpased = 0f;

            UpdateTextDisplayer(data, 1f);
            Stop();
        }

        private void UpdateTextDisplayer(SubtitleData data, float progress) {
            DialogManager manager = GameManager.DialogManager;
            if (manager == null || textDisplayer == null) return;
           
            string result = data.textToDisplay;
            progress = Mathf.Clamp01(progress);
           
            UpdateTextDisplayer(manager, data.textToDisplay, Mathf.Clamp01(progress), ref result);
            UpdateTextDisplayer(manager, CurrentSubtitle as CharacterSubtitle, ref result);

           textDisplayer.text = result;
        }

        private void UpdateTextDisplayer(DialogManager manager, string textToDisplay, float progress, ref string result)
        {
            if (manager == null) return;

            if (manager.CanShowSubtitleByTime) {
                char[] characters = textToDisplay.ToCharArray();
                result = string.Empty;

                for (int i = 0; i < characters.Length * progress; i++)
                    result += characters[i];
            }
        }

        private void UpdateTextDisplayer(DialogManager manager, CharacterSubtitle subtitle,  ref string result)
        {
            if(manager == null || subtitle == null) return;
            string characterName = subtitle.CharacterName;

            if (!string.IsNullOrEmpty(characterName) && manager.CanDisplayCharacterName)
                result =  $"{characterName}: " + result;
        }



        public void UpdateDisplayers(DialogManager manager, Subtitle subtitle, int dataIndex, float timeElapsed) {
            CurrentSubtitle = subtitle;
            if (manager == null || subtitle == null || isUpdateFinished) return;

            UpdateDisplayers(manager, subtitle.GetData(dataIndex), timeElapsed);
        }

        private void UpdateDisplayers(DialogManager manager, SubtitleData data, float timeElapsed)
        {
            if(manager == null || isUpdateFinished || timeElapsed < data.StartTime) return;
            if (!isUpdateStarted) onUpdateStarted?.Invoke(data);

            float progress = Mathf.Clamp01(this.timeElpased / (data.Duration * manager.SubtitleCatchupSpeed));

            if (progress < 1f)
                onProgressUpdate?.Invoke(data, progress);
            else {
                onProgressUpdate?.Invoke(data, 1f);
                if(timeElapsed >= data.EndTime) onUpdateFinished?.Invoke(data);
            }

            this.timeElpased += Time.deltaTime;
        }

        public void Play(Subtitle subtitle, int index) {

        }

        public void Play(Subtitle subtitle, int[] indexes)
        {

        }

        public virtual void Stop() {
            CurrentSubtitle  = null;

            if (group != null) group.alpha = 0f;
            if (textDisplayer != null) textDisplayer.text = string.Empty;

            GameManager.DialogManager?.RemoveActifSubtitleHandler(this);
        }


        private void StartFade(bool isFadingIn){
            CancelFade();
            fadeUpdater = Fade(isFadingIn);
            StartCoroutine(fadeUpdater);
        }

        private void CancelFade()
        {
            if (fadeUpdater != null) StopCoroutine(fadeUpdater);
            fadeUpdater = null;
        }

        public bool IsFadedIn()
        {
            return group != null ? group.alpha == 0f : false; 
        }

        private IEnumerator Fade(bool isFadingIn) {
            float t = 0f, currentAlpha = group != null ? group.alpha : 0f;
            float desiredAlpha = isFadingIn ? 0f : 1f;

            while (group != null) {
                DialogManager manager = GameManager.DialogManager;
                if (manager == null || group == null) break;

                float progress = Mathf.Clamp01(t / manager.SubtitleFadeDuration);
                group.alpha = Mathf.Lerp(currentAlpha, desiredAlpha, t);

                if (progress >= 1f) {
                    group.alpha = desiredAlpha;
                    break;
                }

                t += Time.deltaTime;
                yield return null;
            }
            
        }

    } 
}
