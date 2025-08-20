using UnityEngine;

namespace RedSilver2.Framework.Scenes.UI
{
    public abstract class LoadingScreenUI : MonoBehaviour
    {
        protected virtual void OnDisable()
        {
           DisableEvent(SceneLoaderManager.Instance);
        }
        protected virtual void OnEnable()
        {
            Debug.Log("...");
            EnableEvent(SceneLoaderManager.Instance);
        }

        protected abstract void EnableEvent(SceneLoaderManager sceneLoader);
        protected abstract void DisableEvent(SceneLoaderManager sceneLoader);
    }
}
