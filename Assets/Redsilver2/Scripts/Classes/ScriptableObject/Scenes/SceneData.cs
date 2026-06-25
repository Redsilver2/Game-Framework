using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace RedSilver2.Framework.Scenes {
    [System.Serializable]
    public class SceneData {

        [Space]
        [SerializeField] private int sceneIndex;

        [Space]
        [SerializeField] private string customSceneName;
        [SerializeField] [TextArea(4,4)] private string  sceneDescription;

        [Space]
        [SerializeField] private bool isUnlocked;

        [Space]
        [SerializeField] private Sprite[]  sceneThumbnails;

        private string buildSettingSceneName;

        private readonly UnityEvent onLoadStarted;
        private readonly UnityEvent onLoadFinished;
        private readonly UnityEvent<float> onLoadProgressChanged;

        public bool   IsUnlocked             => isUnlocked;
        public string CustomSceneName        => customSceneName;
        public string BuildSettingSceneName  => buildSettingSceneName;
        public string SceneDescription       => sceneDescription;
        public int    SceneIndex             => sceneIndex;
        public Sprite[] SceneThumbnails => sceneThumbnails;

        public SceneData(int sceneIndex) {
            this.sceneIndex = sceneIndex;
            onLoadFinished = new UnityEvent();

            onLoadStarted = new UnityEvent();
            onLoadProgressChanged = new UnityEvent<float>();
        }

#if UNITY_EDITOR
       public void Validate() {
            int sceneCount = SceneManager.sceneCountInBuildSettings;

            if (sceneCount > 0) {
                string path = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
                sceneIndex = Mathf.Clamp(sceneIndex, 0, sceneCount - 1);

                buildSettingSceneName = buildSettingSceneName = !string.IsNullOrEmpty(path) ? Path.GetFileNameWithoutExtension(path) :
                                                                      string.Empty;
            }
            else {
                sceneIndex = -1;
                buildSettingSceneName = string.Empty;
            }
         
        }
#endif
        public void Enable()
        {
            SceneLoaderManager manager = GameManager.SceneLoaderManager;
            manager?.AddOnSingleSceneLoadFinishedListener(OnLoadFinished);
            manager?.AddOnSingleSceneLoadProgressChangedListener(OnLoadProgressChanged);
            manager?.AddOnSingleSceneLoadStartedListener(OnLoadStarted);
        }

        public void Disable()
        {
            SceneLoaderManager manager = GameManager.SceneLoaderManager;
            manager?.RemoveOnSingleSceneLoadFinishedListener(OnLoadFinished);
            manager?.RemoveOnSingleSceneLoadProgressChangedListener(OnLoadProgressChanged);
            manager?.RemoveOnSingleSceneLoadStartedListener(OnLoadStarted);
        }

        private void OnLoadStarted(int sceneIndex)
        {
            if (this.sceneIndex == sceneIndex) { onLoadStarted?.Invoke(); }
        }
        private void OnLoadFinished(int sceneIndex)
        {
            if (this.sceneIndex == sceneIndex) { onLoadFinished?.Invoke(); }
        }
        private void OnLoadProgressChanged(int sceneIndex, float progress)
        {
            if (this.sceneIndex == sceneIndex) { onLoadProgressChanged?.Invoke(progress); }
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

        public void SetLoadProgression(float progression) {
            onLoadProgressChanged.Invoke(progression); 
        }
        public bool IsLoaded()
        {
            return SceneManager.GetSceneByBuildIndex(sceneIndex).isLoaded;
        }
        public bool Compare(int sceneIndex)
        {
            return this.SceneIndex == sceneIndex;
        }

        public bool Compare(string sceneName)
        {
            if (!string.IsNullOrEmpty(sceneName)) return sceneName.Equals(this.buildSettingSceneName, System.StringComparison.OrdinalIgnoreCase);
            return false;
        }

        public Sprite GetThumbnail(int index)
        {
            Sprite[] thumbnails = sceneThumbnails;

            if (index >= 0 && index < thumbnails.Length) return thumbnails[index];
            return null;
        }

        public Sprite GetRandomThumbnail()
        {
            Sprite[] thumbnails = sceneThumbnails;
            if (thumbnails.Length > 0) return thumbnails[Random.Range(0, thumbnails.Length)];
            return null;
        }

        public void Load() {
          
            //string sceneAssetPath = Application.persistentDataPath + $"/Saves/{0}/Scene_Datas";
            //if(!Directory.Exists(sceneAssetPath)) { Directory.CreateDirectory(sceneAssetPath); }

            //SceneLoaderManager.SceneAsset asset = JsonUtility.FromJson<SceneLoaderManager.SceneAsset>($"{sceneAssetPath}/Scene_{sceneIndex}_Asset");

            //if (asset != null)  this.asset = asset;
            //else                this.asset = SceneLoaderManager.SceneAsset.CreateAndGet(this, defaultSceneUnlockedState);
        }

        public void Save(){

        }
    }
}
