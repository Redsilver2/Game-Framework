using UnityEngine;
using UnityEngine.UI;

namespace RedSilver2.Framework.Scenes.UI
{
    [RequireComponent(typeof(Image))]
    public class LoadingBar : LoadingProgressUI
    {
        [SerializeField] private Sprite background;

        private Image loadingBar;

        protected void Awake()
        {
            loadingBar = GetComponent<Image>();
           
            SetImage();
            FillLoadingBar(0f);
        }

        private void OnSingleSceneLoadStarted(int sceneIndex)
        {
            FillLoadingBar(0f);
        }

        private void OnSingleSceneLoadFinished(int sceneIndex)
        {
            FillLoadingBar(1f);
        }

        protected sealed override void OnSingleSceneLoadProgressChanged(int sceneIndex, float progress)
        {
            FillLoadingBar(Mathf.Clamp01(progress));
        }

        private void FillLoadingBar(float progress)
        {
            if (loadingBar != null) loadingBar.fillAmount = progress;
        }

        private void SetImage()
        {
            if (loadingBar != null) loadingBar.sprite = background;
        }


        protected override void DisableEvent(SceneLoaderManager sceneLoader)
        {
            base.DisableEvent(sceneLoader);
           
            if(sceneLoader != null)
            {
                sceneLoader.RemoveOnSingleSceneLoadFinishedListener(OnSingleSceneLoadFinished);
                sceneLoader.RemoveOnSingleSceneLoadStartedListener(OnSingleSceneLoadStarted);
            }
        }

        protected override void EnableEvent(SceneLoaderManager sceneLoader)
        {
            base.EnableEvent(sceneLoader);

            if (sceneLoader != null)
            {
                sceneLoader.AddOnSingleSceneLoadFinishedListener(OnSingleSceneLoadFinished);
                sceneLoader.AddOnSingleSceneLoadStartedListener(OnSingleSceneLoadStarted);
            }
        }
    }
}
