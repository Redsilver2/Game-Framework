using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace RedSilver2.Framework.Scenes
{
    public abstract partial class SceneLoaderManager : MonoBehaviour   
    {
        private UnityEvent<int>        onSingleSceneLoadStarted;
        private UnityEvent<int>        onSingleSceneLoadFinished;
        private UnityEvent<int, float> onSingleSceneLoadProgressChanged;

        private LoadingScreen[] loadingScreens;
        private LoadingScreen   currentLoadingScreen; 

        public const string SCENE_ROOT_PATH = "Scenes/";


        private static List<SceneData>              SceneDatasInstances    = new List<SceneData>();
        private static Dictionary<int, IEnumerator> sceneLoadingOperations = new Dictionary<int, IEnumerator>();

        public static bool IsLoadingSingleScene { get; private set; } = false;
        public static SceneLoaderManager Instance { get; private set; }


        private void Awake()
        {
            if(Instance != null) { Destroy(this); }
            Instance = this;

            SetEvents();

            AddOnSingleSceneLoadStartedListener(OnSingleSceneLoadStarted);
            AddOnSingleSceneLoadFinishedListener(OnSingleSceneLoadFinished);

            FindLoadingScreens();
        }

        private void OnSingleSceneLoadStarted(int sceneIndex)
        {
            StopAllSceneLoadingOperations();
            IsLoadingSingleScene = true;
        }

        private void OnSingleSceneLoadFinished(int sceneIndex)
        {
            IsLoadingSingleScene = false;
        }

        private void SetEvents()
        {
            onSingleSceneLoadStarted         = new UnityEvent<int>();
            onSingleSceneLoadFinished        = new UnityEvent<int>();
            onSingleSceneLoadProgressChanged = new UnityEvent<int, float>();
        }

        private void FindLoadingScreens()
        {
            loadingScreens = FindObjectsByType<LoadingScreen>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            if(loadingScreens.Length > 0) currentLoadingScreen = loadingScreens[0];
        }

        private void StartSceneLoad(int sceneIndex)
        {
            IEnumerator operation;
            StopSceneLoadingOperation(sceneIndex);

            operation = SceneLoadingOperation(sceneIndex, IsLoadingSingleScene);
            sceneLoadingOperations.Add(sceneIndex, operation);
          
            StartCoroutine(operation);
        }

        private void FinishSceneLoad(int sceneIndex, bool isLoadingSingleScene)
        {
            if(sceneLoadingOperations.ContainsKey(sceneIndex))
                sceneLoadingOperations.Remove(sceneIndex);

            if (isLoadingSingleScene) 
            { 
                onSingleSceneLoadFinished.Invoke(sceneIndex);
                if (currentLoadingScreen) currentLoadingScreen.gameObject.SetActive(false);
            }
        }
 
        private IEnumerator WaitForLoadingScreen(bool isVisible)
        {
            if(currentLoadingScreen != null && IsLoadingSingleScene)
            {
                if (isVisible) currentLoadingScreen.Show();
                else           currentLoadingScreen.Hide();

                while (!currentLoadingScreen.IsUIVisibilitySet()) yield return null;
            }
        }

        private IEnumerator SceneLoadingOperation(int sceneIndex, bool isLoadingSingleScene)
        {
                yield return WaitForLoadingScreen(true);
                yield return StartCoroutine(WaitOperationLoading(sceneIndex, isLoadingSingleScene));
                yield return WaitForLoadingScreen(false);

                FinishSceneLoad(sceneIndex, isLoadingSingleScene);  
        }

        private IEnumerator WaitOperationLoading(int sceneIndex, bool isLoadingSingleScene)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, IsLoadingSingleScene ? LoadSceneMode.Single : LoadSceneMode.Additive);

            operation.allowSceneActivation = false;
            yield return StartCoroutine(WaitOperationLoading(operation, GetSceneData(sceneIndex), isLoadingSingleScene));       
        }

        private IEnumerator WaitOperationLoading(AsyncOperation operation, SceneData data, bool isLoadingSingleScene)
        {
            if (operation != null)
            {
                while (operation.progress < 0.9f)
                {
                    UpdateOperationProgress(data, operation.progress / 0.9f, isLoadingSingleScene);
                    yield return null;
                }

                UpdateOperationProgress(data, 1f, isLoadingSingleScene);
                operation.allowSceneActivation = true;
            }
        }

        private void UpdateOperationProgress(SceneData data, float progress, bool isLoadingSingleScene)
        {
            progress = Mathf.Clamp01(progress);

            if (data != null)
            {
                if (!isLoadingSingleScene) SetOperationProgress(progress, data);
                else SetOperationProgress(data.SceneIndex, progress);
            }
        }

        private void SetOperationProgress(float progression, SceneData data)
        {
            if (data != null) data.SetLoadProgression(progression);           
        }

        private void SetOperationProgress(int sceneIndex, float progress)
        {
            if (IsLoadingSingleScene) onSingleSceneLoadProgressChanged.Invoke(sceneIndex, progress);
        }


        private void StopSceneLoadingOperation(int sceneIndex)
        {
            if (sceneLoadingOperations.ContainsKey(sceneIndex))
            {
                IEnumerator operation = sceneLoadingOperations[sceneIndex];
                if(operation != null) StopCoroutine(operation);
            }
        }

        private void StopAllSceneLoadingOperations()
        {
            foreach(IEnumerator operation in sceneLoadingOperations.Values) StopCoroutine(operation);
            sceneLoadingOperations.Clear();
        }

        public void AddOnSingleSceneLoadStartedListener(UnityAction<int> action)
        {
            if (onSingleSceneLoadStarted != null && action != null) onSingleSceneLoadStarted.AddListener(action);
        }
        public void AddOnSingleSceneLoadFinishedListener(UnityAction<int> action)
        {
            if (onSingleSceneLoadFinished != null && action != null) onSingleSceneLoadFinished.AddListener(action);
        }
        public void AddOnSingleSceneLoadProgressChangedListener(UnityAction<int, float> action)
        {
            if (onSingleSceneLoadStarted != null && action != null) onSingleSceneLoadProgressChanged.AddListener(action);
        }

        public void RemoveOnSingleSceneLoadStartedListener(UnityAction<int> action)
        {
            if (onSingleSceneLoadStarted != null && action != null)  onSingleSceneLoadStarted.RemoveListener(action);
        }
        public void RemoveOnSingleSceneLoadFinishedListener(UnityAction<int> action)
        {
            if (onSingleSceneLoadFinished != null && action != null) onSingleSceneLoadFinished.RemoveListener(action);
        }
        public void RemoveOnSingleSceneLoadProgressChangedListener(UnityAction<int, float> action)
        {
            if (onSingleSceneLoadStarted != null && action != null)  onSingleSceneLoadProgressChanged.RemoveListener(action);
        }

        public void LoadSingleScene(string sceneName)
        {
            SceneData data = GetSceneData(sceneName);
            if (data != null) { LoadSingleScene(data.SceneIndex); }
        }

        public void LoadSingleScene(int sceneIndex) 
        {
            if (IsValidSceneIndex(sceneIndex) && CanLoadScene(sceneIndex))
            {
                if (currentLoadingScreen) currentLoadingScreen.gameObject.SetActive(true);
               
                onSingleSceneLoadStarted.Invoke(sceneIndex);
                StartSceneLoad(sceneIndex);
            }
        }

        public void LoadScene(string sceneName)
        {
            SceneData data = GetSceneData(sceneName);
            if (data != null) { LoadScene(data.SceneIndex); }
        }

        public void LoadScene(int sceneIndex) 
        {
            if (IsValidSceneIndex(sceneIndex) && CanLoadScene(sceneIndex))
            {
                onSingleSceneLoadStarted.Invoke(sceneIndex);
            }
        }

        public static bool CanLoadScene(int sceneIndex)
        {
            if (!IsLoadingSingleScene && !sceneLoadingOperations.ContainsKey(sceneIndex) 
             && !IsSceneLoaded(sceneIndex) && IsSceneUnlocked(sceneIndex)) return true;

            return false;
        }

        public static bool IsSceneLoaded(int sceneIndex)
        {
            if(!IsValidSceneIndex(sceneIndex)) return false; 
            return SceneManager.GetSceneByBuildIndex(sceneIndex).isLoaded;
        }

        public static bool IsSceneUnlocked(int sceneIndex)
        {
            SceneData data = GetSceneData(sceneIndex);
            if (data != null) return data.IsUnlocked;
            return false;
        }

        public static bool IsValidSceneIndex(int sceneIndex)
        {
            if(sceneIndex < 0 || sceneIndex >= SceneManager.sceneCountInBuildSettings) return false;
            return true;
        }

        public static void AddSceneData(SceneData sceneData)
        {
            if (SceneDatasInstances.Where(x => x.Compare(sceneData.SceneIndex)).Count() == 0)
                SceneDatasInstances.Add(sceneData);
        }
        public static void AddSceneData(SceneData[] sceneDatas)
        {
            if(sceneDatas != null)
            {
                sceneDatas = sceneDatas.Where(x => x != null).Distinct().ToArray();
                foreach(SceneData sceneData in sceneDatas) AddSceneData(sceneData);
            }
        }

        public static string[] GetSceneDatas()
        {
            List<string> results = new List<string>();
            SceneData[]  datas   = SceneDatasInstances.OrderBy(x => x.SceneIndex).ToArray();  
            
            foreach(SceneData data in datas) { results.Add($"{data.ToString()}\n"); }
            return results.ToArray();
        }

        public static SceneData   GetSceneData(string sceneName)
        {
            var results = SceneDatasInstances.Where(x => x.Compare(sceneName));
            if (results.Count() > 0) return results.First();
            return null;
        }
        public static SceneData[] GetScenesDatas(string[] sceneNames)
        {
            List<SceneData> results = new List<SceneData>();
            if (sceneNames == null) return results.ToArray();

            foreach (string sceneName in sceneNames)
            {
                SceneData data = GetSceneData(sceneName);
                if(data != null) { results.Add(data); }
            }

            return results.ToArray();
        }

        public static SceneData GetSceneData(int sceneIndex)
        {
            var results = SceneDatasInstances.Where(x => x.Compare(sceneIndex));
            if (results.Count() > 0) return results.First();
            return null;
        }

        public static SceneData[] GetScenesDatas(int[] sceneIndexes)
        {
            List<SceneData> results = new List<SceneData>();
            if (sceneIndexes == null) return results.ToArray();

            foreach (int sceneIndex in sceneIndexes)
            {
                SceneData data = GetSceneData(sceneIndex);
                if (data != null) { results.Add(data); }
            }

            return results.ToArray();
        }

        public static SceneData[] GetAllScenesDatas() => SceneDatasInstances.ToArray();
        
    }
}