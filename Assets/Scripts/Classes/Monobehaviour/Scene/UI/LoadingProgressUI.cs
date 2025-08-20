namespace RedSilver2.Framework.Scenes.UI
{
    public abstract class LoadingProgressUI : LoadingScreenUI
    {
        protected abstract void OnSingleSceneLoadProgressChanged(int sceneIndex, float progress);   

        protected override void DisableEvent(SceneLoaderManager sceneLoader)
        {
            if (sceneLoader != null) { sceneLoader.RemoveOnSingleSceneLoadProgressChangedListener(OnSingleSceneLoadProgressChanged); }
        }

        protected override void EnableEvent(SceneLoaderManager sceneLoader)
        {
            if (sceneLoader != null) { sceneLoader.AddOnSingleSceneLoadProgressChangedListener(OnSingleSceneLoadProgressChanged); }
        }
    }
}
