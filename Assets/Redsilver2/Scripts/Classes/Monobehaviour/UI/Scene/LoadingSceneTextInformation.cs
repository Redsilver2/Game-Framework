using UnityEngine;
using UnityEngine.SceneManagement;

namespace RedSilver2.Framework.Scenes.UI
{
    public class LoadingSceneTextInformation : LoadingTextDisplayer
    {
        [Space]
        [SerializeField] private SceneTextInformationType informationType;

        private void OnSingleSceneLoadStarted(int sceneIndex) {
            Display(GameManager.SceneLoaderManager, sceneIndex);
        }

        private void Display(SceneLoaderManager manager, int sceneIndex)
        {
            if (informationType == SceneTextInformationType.Name) DisplayName(manager, sceneIndex, true);
            else if (informationType == SceneTextInformationType.CustomName) DisplayName(manager, sceneIndex, false);
            else if (informationType == SceneTextInformationType.Description) DisplayDescription(manager, sceneIndex, false);
        }

        private void DisplayName(SceneLoaderManager manager,int sceneIndex,  bool showActualName)
        {
            SetText(string.Empty);

            if(manager != null) {
                SetText(manager.GetSceneName(sceneIndex, showActualName));
            }
         
        }

        private void DisplayDescription(SceneLoaderManager manager, int sceneIndex, bool showActualName)
        {
            SetText(string.Empty);

            if (manager != null) {
                SceneData data = manager.GetSceneData(sceneIndex);
                if (data != null) SetText(data.SceneDescription);
            }
        }

        protected sealed override void DisableEvent(SceneLoaderManager sceneLoader)
        {
            sceneLoader?.RemoveOnSingleSceneLoadStartedListener(OnSingleSceneLoadStarted);
        }

        protected sealed override void EnableEvent(SceneLoaderManager sceneLoader) {
            sceneLoader?.AddOnSingleSceneLoadStartedListener(OnSingleSceneLoadStarted);
        }

       
    }
}
