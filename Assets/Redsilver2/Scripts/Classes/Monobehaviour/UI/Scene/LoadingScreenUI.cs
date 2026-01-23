using UnityEngine;

namespace RedSilver2.Framework.Scenes.UI
{
    public abstract class LoadingScreenUI : MonoBehaviour
    {
        protected virtual void OnDisable() {
           DisableEvent(GameManager.SceneLoaderManager);
        }
        protected virtual void OnEnable() {
            EnableEvent(GameManager.SceneLoaderManager);
        }

        protected abstract void EnableEvent(SceneLoaderManager sceneLoader);
        protected abstract void DisableEvent(SceneLoaderManager sceneLoader);
    }
}
