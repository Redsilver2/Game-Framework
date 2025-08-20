using UnityEngine;

namespace RedSilver2.Framework.Scenes.UI
{
    public class LoadingTextPercentage : LoadingText
    {
        protected virtual void OnSingleSceneLoadStarted(int sceneIndex)
        {
            SetText($"0%");
        }

        protected override void OnSingleSceneLoadProgressChanged(int sceneIndex, float progress)
        {
            SetText($"{(int)(progress * 100)}%");
        }

        protected override void DisableEvent(SceneLoaderManager sceneLoader)
        {
            base.DisableEvent(sceneLoader);
            if (sceneLoader != null) sceneLoader.RemoveOnSingleSceneLoadStartedListener(OnSingleSceneLoadStarted);
        }

        protected override void EnableEvent(SceneLoaderManager sceneLoader)
        {
            base.EnableEvent(sceneLoader);
            if (sceneLoader != null) sceneLoader.AddOnSingleSceneLoadStartedListener(OnSingleSceneLoadStarted);
        }
    }
}
