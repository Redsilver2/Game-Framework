using UnityEngine;

namespace RedSilver2.Framework.Scenes
{
    public abstract class SingleSceneLoadStartedEvent : SceneLoaderEvent {
        protected sealed override void SetEvents(SceneLoaderManager manager, bool isAddingEvents)
        {
            if (isAddingEvents) manager?.AddOnSingleSceneLoadStartedListener(OnSingleSceneLoadStarted);
            else manager?.RemoveOnSingleSceneLoadStartedListener(OnSingleSceneLoadStarted);
        }

        protected abstract void OnSingleSceneLoadStarted(int sceneIndex);
    }
}
