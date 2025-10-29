using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace RedSilver2.Framework.Scenes
{
    public abstract partial class SceneLoaderManager : MonoBehaviour
    {
        public abstract class SceneData
        {
            private bool isUnlocked;

            public readonly string SceneName;
            public readonly int    SceneIndex;

            private readonly Sprite[] Thumbnails;

            private readonly UnityEvent onLoadStarted;
            private readonly UnityEvent onLoadFinished;
            private readonly UnityEvent<float> onLoadProgressChanged;

            public bool IsUnlocked => isUnlocked;

            protected SceneData()
            {
            }

            public SceneData(int sceneIndex)
            {
                SetName(out this.SceneName);
                InitializeEvents(out onLoadStarted, out onLoadFinished, out onLoadProgressChanged);

                SetSceneLoaderManagerEvents(Instance);
                AddSceneData(this);

                this.SceneIndex       = sceneIndex;            
                this.isUnlocked       = false;

                this.Thumbnails       = GetThumbnails();
            }

            public SceneData(int sceneIndex, bool isUnlocked)
            {
                SetName(out this.SceneName);
                InitializeEvents(out onLoadStarted, out onLoadFinished, out onLoadProgressChanged);

                SetSceneLoaderManagerEvents(Instance);
                AddSceneData(this);

                this.SceneIndex       = sceneIndex;
                this.isUnlocked       = isUnlocked;

                this.Thumbnails       = GetThumbnails();
            }

            private void InitializeEvents(out UnityEvent event01, out UnityEvent event02, out UnityEvent<float> event03)
            {
                event01 = new UnityEvent();
                event02 = new UnityEvent();
                event03 = new UnityEvent<float>();
            }
            protected virtual void SetSceneLoaderManagerEvents(SceneLoaderManager sceneLoaderManager)
            {
                if (sceneLoaderManager != null)
                {
                    sceneLoaderManager.AddOnSingleSceneLoadStartedListener(OnLoadStarted);
                    sceneLoaderManager.AddOnSingleSceneLoadFinishedListener(OnLoadFinished);
                    sceneLoaderManager.AddOnSingleSceneLoadProgressChangedListener(OnLoadProgressChanged);
                }
            }
            
            
            private Sprite[] GetThumbnails() => Resources.LoadAll<Sprite>($"{SCENE_ROOT_PATH}{SceneName}/{SceneIndex}");

            public abstract string GetDescription();
            protected abstract void SetName(out string sceneName);


            private void OnLoadStarted(int sceneIndex)
            {
                if (sceneIndex == this.SceneIndex) { onLoadStarted.Invoke(); }
            }
            private void OnLoadFinished(int sceneIndex)
            {
                if (sceneIndex == this.SceneIndex) { onLoadFinished.Invoke(); }
            }
            private void OnLoadProgressChanged(int sceneIndex, float progress)
            {
                if (sceneIndex == this.SceneIndex) { onLoadProgressChanged.Invoke(progress); }
            }

            public void AddOnLoadStartedListener(UnityAction action)
            {
                if (onLoadStarted != null && action != null) onLoadStarted.AddListener(action);
            }
            public void AddOnLoadStartedListener(UnityAction[] actions)
            {
                if (actions != null)
                    foreach (UnityAction action in actions) AddOnLoadStartedListener(action);
            }

            public void RemoveOnLoadStartedListener(UnityAction action)
            {
                if (onLoadStarted != null && action != null) onLoadStarted.RemoveListener(action);
            }
            public void RemoveOnLoadStartedListener(UnityAction[] actions)
            {
                if (actions != null)
                    foreach (UnityAction action in actions) RemoveOnLoadStartedListener(action);
            }

            public void AddOnLoadFinishedListener(UnityAction action)
            {
                if (onLoadFinished != null && action != null) onLoadFinished.AddListener(action);
            }
            public void AddOnLoadFinishedListener(UnityAction[] actions)
            {
                if (actions != null)
                    foreach (UnityAction action in actions) AddOnLoadStartedListener(action);
            }

            public void RemoveOnLoadFinishedListener(UnityAction action)
            {
                if (onLoadFinished != null && action != null) onLoadFinished.RemoveListener(action);
            }
            public void RemoveOnLoadFinishedListener(UnityAction[] actions)
            {
                if (actions != null)
                    foreach (UnityAction action in actions) RemoveOnLoadStartedListener(action);
            }

            public void AddOnLoadProgressChangedListener(UnityAction<float> action)
            {
                if (onLoadStarted != null && action != null) onLoadProgressChanged.AddListener(action);
            }
            public void AddOnLoadProgressChangedListener(UnityAction<float>[] actions)
            {
                if (actions != null)
                    foreach (UnityAction<float> action in actions) AddOnLoadProgressChangedListener(action);
            }

            public void RemoveOnLoadProgressChangedListener(UnityAction<float> action)
            {
                if (onLoadProgressChanged != null && action != null) onLoadProgressChanged.RemoveListener(action);
            }
            public void RemoveOnLoadProgressChangedListener(UnityAction<float>[] actions)
            {
                if (actions != null)
                    foreach (UnityAction<float> action in actions) RemoveOnLoadProgressChangedListener(action);
            }

            public void SetLoadProgression(float progression) { onLoadProgressChanged.Invoke(progression); }
            public bool IsLoaded() => SceneManager.GetSceneByBuildIndex(SceneIndex).isLoaded;

            public bool Compare(string sceneName)
            {
                if (!string.IsNullOrEmpty(sceneName)) return sceneName.ToLower() == this.SceneName.ToLower();
                return false;
            }
            public bool Compare(int sceneIndex)
            {
                return sceneIndex == this.SceneIndex;
            }

            public Sprite GetThumbnail(int index)
            {
                if (index >= 0 && index < Thumbnails.Length) return Thumbnails[index];
                return null;
            }

            public Sprite GetRandomThumbnail()
            {
                if (Thumbnails.Length > 0) return Thumbnails[Random.Range(0, Thumbnails.Length)];
                return null;
            }
        }
    }
}
