using RedSilver2.Framework.Dialogs.Datas;
using RedSilver2.Framework.StateMachines.Controllers;
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
        private Transform   parent;
        private IEnumerator fadeUpdater, handlerUpdater;

        private UnityEvent onUpdateStarted;
        private UnityEvent onUpdateFinished;
        private UnityEvent<float> onProgressUpdate;
        public bool IsUpdateStarted => isUpdateStarted;
        public bool IsUpdateFinished => isUpdateFinished;
        public int LineCount => textDisplayer == null ? 0 : textDisplayer.textInfo.lineCount;

        public Transform Parent => parent;

        protected virtual void Awake() {
            group = GetComponent<CanvasGroup>();
            canTextDisplayShowCharacterName = true;

            onUpdateStarted  = new UnityEvent();
            onUpdateFinished = new UnityEvent();
            onProgressUpdate = new UnityEvent<float>();

            AddOnUpdateStartedListener(OnUpdateStarted);
            AddOnUpdateFinishedListener(OnUpdateFinished);

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

        public void AddOnUpdateStartedListener(UnityAction action) {
            if (action != null) onUpdateStarted?.AddListener(action);
        }
        public void RemoveOnUpdateStartedListener(UnityAction action) {
            if (action != null) onUpdateStarted?.RemoveListener(action);
        }

        public void AddOnUpdateFinishedListener(UnityAction action)
        {
            if (action != null) onUpdateFinished?.AddListener(action);
        }
        public void RemoveOnUpdateFinishedListener(UnityAction action) {
            if (action != null) onUpdateFinished?.RemoveListener(action);
        }

        public void AddOnProgressUpdateListener(UnityAction< float> action)
        {
            if (action != null) onProgressUpdate?.AddListener(action);
        }
        public void RemoveOnProgressUpdateListener(UnityAction<float> action)
        {
            if (action != null) onProgressUpdate?.RemoveListener(action);
        }

        protected void SetCanTextDisplayShowCharacterName(bool canTextDisplayShowCharacterName)
        {
            this.canTextDisplayShowCharacterName = canTextDisplayShowCharacterName;
        }

        protected virtual void OnUpdateStarted() {

            StartFade(false, () => {
                isUpdateStarted = true;
                if (textDisplayer != null) textDisplayer.text = string.Empty;
            }, null);
        }

        protected virtual void OnUpdateFinished() {
            StartFade(true, () => {
                isUpdateFinished = true;
            }, () => {
                DialogManager manager = DialogManager.GetInstance();
                manager?.AddAvailableSubtitleHandler(this);
            });

        }

        public void Play(SubtitleUpdater updater, Subtitle subtitle, int dataIndex)
        {
            if (subtitle == null) return;
            DialogManager manager = DialogManager.GetInstance();
            Play(updater, subtitle, subtitle.GetData(dataIndex), manager != null ? manager.SubtitleFadeWaitTime : 0f);
        }

        public void Play(SubtitleUpdater updater, Subtitle subtitle, SubtitleData data)
        {
            if (subtitle == null || updater == null || !subtitle.ContainsData(data)) return;
            DialogManager manager = DialogManager.GetInstance();
            Play(updater, subtitle, data, manager != null ? manager.SubtitleFadeWaitTime : 0f);
        }

        public void Play(SubtitleUpdater updater, Subtitle subtitle, int dataIndex, float fadeWaitTime) {
            if (updater == null || subtitle == null) return;
            Play(updater, subtitle, subtitle.GetData(dataIndex), fadeWaitTime);
        }

        public void Play(SubtitleUpdater updater, Subtitle subtitle, SubtitleData data, float fadeWaitTime) {
            if (updater == null || subtitle == null || !subtitle.ContainsData(data)) return;
            string characterName = string.Empty;
          
            if (subtitle is CharacterSubtitle) characterName = (subtitle as CharacterSubtitle).CharacterName;
            parent = subtitle.Parent;

            StartHandlerUpdater(UpdateHandler(updater, characterName, data.textToDisplay, 
                                         data.StartTime, data.EndTime + fadeWaitTime, data.Duration));
        }

        public void Play(SubtitleUpdater updater, Transform worldSpaceParent, string characterName, string textToDisplay, float duration, float fadeWaitTime) {
            if (updater == null || string.IsNullOrEmpty(textToDisplay)) return;
            this.parent = worldSpaceParent;
            StartHandlerUpdater(UpdateHandler(updater, characterName, textToDisplay, 0f, duration + fadeWaitTime, duration));
        }

        private void StopHandlerUpdater() {
            if (handlerUpdater != null) StopCoroutine(handlerUpdater);
            handlerUpdater = null;
        }

        private void StartHandlerUpdater(IEnumerator enumerator) {
            StopHandlerUpdater();
            handlerUpdater = enumerator;
            if (handlerUpdater != null) StartCoroutine(handlerUpdater);
        }

        private IEnumerator UpdateHandler(SubtitleUpdater updater, string characterName, string textToDisplay, float startTime, float endTime, float duration) {
            float timeElapsed = 0f;
            DialogManager manager = DialogManager.GetInstance();

            while (updater != null && manager != null && !isUpdateFinished) {  
                float progress = Mathf.Clamp01(timeElapsed / duration);

                if (updater.TimeElapsed < startTime) {
                    yield return null;
                    continue;
                }
                else if(!isUpdateStarted) onUpdateStarted?.Invoke();

                if (!isUpdateFinished) {
                    UpdateTextDisplayer(manager, characterName, textToDisplay, progress);

                    if (updater.TimeElapsed >= endTime) {
                        onUpdateFinished?.Invoke();
                        yield return null;
                        break;
                    }
                }

                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }

        private void UpdateTextDisplayer(DialogManager manager, string characterName, string textToDisplay, float progress)
        {
            if (manager == null) return;
            string result = string.Empty;
            UpdateTextDisplayer(manager, textToDisplay, progress, ref result);
            UpdateTextDisplayer(manager, characterName, ref result);
            if (textDisplayer != null) textDisplayer.text = result;
        }



        private void UpdateTextDisplayer(DialogManager manager, string textToDisplay, float progress, ref string result)
        {
            if (manager == null || string.IsNullOrEmpty(textToDisplay)) return;
            char[] characters = textToDisplay.ToCharArray();
            int lenght = characters.Length;

            if (manager.CanShowSubtitleByTime && progress < 1f) 
                lenght = (int)(lenght * Mathf.Clamp01(progress));

            result = string.Empty;
            for (int i = 0; i < lenght; i++) result += characters[i];
        }

        protected virtual void UpdateTextDisplayer(DialogManager manager, string characterName, ref string result)
        {
            if(manager == null || string.IsNullOrEmpty(characterName) || !canTextDisplayShowCharacterName || !manager.CanDisplayCharacterName) 
                return;

             result = $"{characterName}: " + result;
        }

        public virtual void Stop(bool resetAvailability) 
        {
            StopHandlerUpdater();

            isUpdateFinished = false;
            isUpdateStarted  = false;
            Debug.Log("B");

            if (resetAvailability) {
                DialogManager manager = DialogManager.GetInstance();
                manager?.AddAvailableSubtitleHandler(this);
            }

            if (group != null) group.alpha = 0f;
            if (textDisplayer != null) textDisplayer.text = string.Empty;
        }


        private void StartFade(bool isFadingIn, UnityAction startAction, UnityAction endAction)
        {
            CancelFade();
            fadeUpdater = Fade(isFadingIn, startAction, endAction);
            StartCoroutine(fadeUpdater);
        }

        private void CancelFade()
        {
            if (fadeUpdater != null) StopCoroutine(fadeUpdater);
            fadeUpdater = null;
        }

        public bool IsFadedIn() {
            return group != null ? group.alpha == 0f : false; 
        }

        public bool IsWorldSpace()
        {
            return IsWorldSpace(DialogManager.GetInstance());
        }

        public bool IsWorldSpace(DialogManager manager)
        {
            return IsWorldSpace(manager, PlayerController.Current);
        }

       private bool IsWorldSpace(DialogManager manager, PlayerController controller) {    
            if(controller == null || manager == null || parent == null) return false;
            SubtitleDisplayMode displayMode = manager.SubtitleDisplayMode;

            if(displayMode == SubtitleDisplayMode.WorldSpace) return true;
            else if(displayMode == SubtitleDisplayMode.ScreenSpace) return false;

            return Vector3.Distance(parent.position, controller.transform.position) <= manager.SubtitleWorldSpaceDistance;
        }

        private IEnumerator Fade(bool isFadingIn, UnityAction startAction, UnityAction endAction) {
            float t = 0f, currentAlpha = group != null ? group.alpha : 0f;
            float desiredAlpha = isFadingIn ? 0f : 1f;

            startAction?.Invoke();

            while (group != null) {
                Fade(GameManager.DialogManager, desiredAlpha, currentAlpha, ref t, out float progress);
                if (progress >= 1f) break;
                yield return null;
            }

            endAction?.Invoke();
        }

        private void Fade(DialogManager manager, float desiredAlpha, float currentAlpha, ref float timeElapsed, out float progress) {
            if (group == null || manager == null) {
                progress = 1f;
                return;
            }

            progress = Mathf.Clamp01(timeElapsed / manager.SubtitleFadeDuration);
            group.alpha = Mathf.Lerp(currentAlpha, desiredAlpha, timeElapsed);

            if (progress >= 1f) group.alpha = desiredAlpha;
            timeElapsed += Time.deltaTime;
        }

    } 
}
