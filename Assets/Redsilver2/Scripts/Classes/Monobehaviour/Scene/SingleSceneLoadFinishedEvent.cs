using UnityEngine;

namespace RedSilver2.Framework.Scenes
{
    public abstract class SingleSceneLoadFinishedEvent : SceneLoaderEvent
    {
        protected sealed override void SetEvents(SceneLoaderManager manager, bool isAddingEvents)
        {
            if (isAddingEvents) manager?.AddOnSingleSceneLoadFinishedListener(OnSingleSceneLoadFinished);
            else manager?.RemoveOnSingleSceneLoadFinishedListener(OnSingleSceneLoadFinished);
        }

        protected abstract void OnSingleSceneLoadFinished(int sceneIndex);
    }
}
