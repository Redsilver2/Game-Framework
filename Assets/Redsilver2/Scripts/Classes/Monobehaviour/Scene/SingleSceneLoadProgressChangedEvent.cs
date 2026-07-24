using UnityEngine;

namespace RedSilver2.Framework.Scenes
{
    public abstract class SingleSceneLoadProgressChangedEvent : SceneLoaderEvent
    {
        protected sealed override void SetEvents(SceneLoaderManager manager, bool isAddingEvents)
        {
            if (isAddingEvents) manager?.AddOnSingleSceneLoadProgressChangedListener(OnSingleSceneLoadProgressChanged);
            else manager?.RemoveOnSingleSceneLoadProgressChangedListener(OnSingleSceneLoadProgressChanged);
        }

        protected abstract void OnSingleSceneLoadProgressChanged(int sceneIndex, float progress);
    }
}
