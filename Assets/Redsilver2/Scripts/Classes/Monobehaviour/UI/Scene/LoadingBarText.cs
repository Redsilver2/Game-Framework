using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace RedSilver2.Framework.Scenes.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class LoadingBarText : LoadingTextDisplayer
    {
        [Space]
        [Header("Use " + PROGRESSION_PLACEHOLDER + " to display numeric progression")]
        [SerializeField] private string placeHolder;

        [Space]
        [SerializeField] private char progressionPlaceholderChar = '%';
        [SerializeField] private uint maxCharAmount = 3;

        [Space]
        [SerializeField] private bool useCharProgressionPlaceholder;
        private const string PROGRESSION_PLACEHOLDER = "{progression}";

        private void OnSingleSceneLoadStarted(int sceneIndex) {
            SetText(GetProgression(0f));
        }

        private void OnSingleSceneLoadProgressChanged(int sceneIndex, float progress)
        {
            SetText(GetProgression(progress));
        }

        protected override void DisableEvent(SceneLoaderManager sceneLoader)
        {
            base.DisableEvent(sceneLoader);
            if (sceneLoader != null)
            {
                sceneLoader.RemoveOnSingleSceneLoadStartedListener(OnSingleSceneLoadStarted);
                sceneLoader.RemoveOnSingleSceneLoadStartedListener(OnSingleSceneLoadStarted);
            }
        }

        protected override void EnableEvent(SceneLoaderManager sceneLoader)
        {
            base.EnableEvent(sceneLoader);

            if (sceneLoader != null)  {
                sceneLoader.AddOnSingleSceneLoadStartedListener(OnSingleSceneLoadStarted);
                sceneLoader.AddOnSingleSceneLoadProgressChangedListener(OnSingleSceneLoadProgressChanged);
            }
        }

        private string GetProgression(float progression)
        {
            progression = (int)Mathf.Clamp01(progression) * 100f;
            string result = $"{progression}";

            if (string.IsNullOrEmpty(placeHolder) || !placeHolder.Contains(PROGRESSION_PLACEHOLDER, System.StringComparison.OrdinalIgnoreCase))
                return result;

            if (useCharProgressionPlaceholder && !progressionPlaceholderChar.Equals('\0')) result = new string(progressionPlaceholderChar, (int)(maxCharAmount * (progression / 100f)));
            return placeHolder.ToLower().Replace(PROGRESSION_PLACEHOLDER, result);
        }
    }
}
