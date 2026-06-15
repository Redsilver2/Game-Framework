using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Interactions;
using RedSilver2.Framework.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace RedSilver2.Framework.Scenes
{
    public partial class SceneLoaderManager : MonoBehaviour   
    {
        [SerializeField] private SceneData[] sceneDatas;

        [Space]
        [SerializeField] private LoadingScreen[] loadingScreens;
        [SerializeField] private int defaultLoadingScreenIndex;
        
        private LoadingScreen currentLoadingScreen;


        private UnityEvent<SceneData> onSceneDataAdded;
        private UnityEvent<SceneData> onSceneDataRemoved;

        private UnityEvent<int>        onSingleSceneLoadStarted;
        private UnityEvent<int>        onSingleSceneLoadFinished;
        private UnityEvent<int, float> onSingleSceneLoadProgressChanged;

        private readonly static Dictionary<int, IEnumerator> sceneLoadingOperations = new Dictionary<int, IEnumerator>();
        public const string SCENE_ROOT_PATH = "Scenes/";

        public static bool IsLoadingSingleScene { get; private set; } = false;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            defaultLoadingScreenIndex = Mathf.Clamp(defaultLoadingScreenIndex, 0, loadingScreens != null ? loadingScreens.Length - 1 : 0);
            ValidateSceneDatas();
        }

        private void ValidateSceneDatas() {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            if (!CanValidateSceneDatas() || sceneDatas == null || sceneDatas.Length == sceneCount)
                return;

            SceneData[] datas = new SceneData[sceneCount];

            for (int i = 0; i < datas.Length; i++) {
                SceneData data = GetSceneData(i);

                if (data == null || data.SceneIndex != i) {
                    datas[i] = new SceneData(i);
                    continue;
                }

                datas[i] = data;
            }

            sceneDatas = datas;
            foreach (SceneData data in sceneDatas) data?.Validate();

        }

        private bool CanValidateSceneDatas()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;

            if (sceneCount == 0) {
                if (sceneDatas.Length > 0) sceneDatas = new SceneData[0];
                return false;
            }

            return true;
        }

        #endif


        private void Awake()
        {
            onSingleSceneLoadStarted = new UnityEvent<int>();
            onSingleSceneLoadProgressChanged = new UnityEvent<int, float>();
            onSingleSceneLoadFinished = new UnityEvent<int>();

            onSceneDataAdded = new UnityEvent<SceneData>();
            onSceneDataRemoved = new UnityEvent<SceneData>();

            AddOnSingleSceneLoadStartedListener(OnSingleSceneLoadStarted);
            AddOnSingleSceneLoadFinishedListener(OnSingleSceneLoadFinished);

            if (loadingScreens.Length > 0) currentLoadingScreen = loadingScreens[defaultLoadingScreenIndex];
        }

        private void Start()
        {
            foreach (SceneData sceneData in sceneDatas) sceneData?.Enable();
        }

        private void Update() {
            if (InputManager.GetKeyDown(KeyboardKey.Space)) {
                bool isDefaultSceneLoaded = IsSceneLoaded(0);
                LoadSingleScene(isDefaultSceneLoaded ? 1 : 0);
            }
        }


        public void AddSceneData(SceneData sceneData) {
            if(sceneData == null) return;
            int sceneIndex = sceneData.SceneIndex;

            RemoveSceneData(sceneIndex);
            sceneDatas[sceneIndex] = sceneData;

            sceneData?.Enable();
            onSceneDataAdded?.Invoke(sceneDatas[sceneIndex]);
        }

        public void RemoveSceneData(int sceneIndex)
        {
            if(sceneDatas == null) return;
            var results = sceneDatas.Where(x => x != null).Where(x => x.Compare(sceneIndex));

            if(results.Count() > 0) {
                onSceneDataRemoved.Invoke(sceneDatas[sceneIndex]);
                sceneDatas[sceneIndex]?.Disable();
                sceneDatas[sceneIndex] = null;
            }
        }

        private void OnSingleSceneLoadStarted(int sceneIndex)
        {
            Debug.Log("Single scene load started. " + sceneIndex);

            GameManager.DisableControls();
            GameManager.SubtitleManager?.Stop();

            StopAllSceneLoadingOperations();
            IsLoadingSingleScene = true;
        }

        private void OnSingleSceneLoadFinished(int sceneIndex) {

            IsLoadingSingleScene = false;
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

                while (!currentLoadingScreen.IsDoneFading(isVisible)) yield return null;
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
            if (operation != null) {
                while (operation.progress < 0.9f) {
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

        public void AddOnSceneAssetAddedListener(UnityAction<SceneData> action)
        {
            if(action != null && onSceneDataAdded != null)
                onSceneDataAdded.AddListener(action);
        }
        public void RemoveOnSceneDataAddedListener(UnityAction<SceneData> action)
        {
            if (action != null && onSceneDataAdded != null)
                onSceneDataAdded.RemoveListener(action);
        }


        public void AddOnSceneDataRemovedListener(UnityAction<SceneData> action)
        {
            if (action != null && onSceneDataRemoved != null)
                onSceneDataRemoved.AddListener(action);
        }

        public void RemoveOnSceneAssetRemovedListener(UnityAction<SceneData> action)
        {
            if (action != null && onSceneDataRemoved != null)
                onSceneDataRemoved.RemoveListener(action);
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
            if (data != null)  LoadSingleScene(data.SceneIndex); 
        }

        public void LoadSingleScene(uint sceneIndex)
        {
            LoadSingleScene((int)sceneIndex);
        }

        public void LoadSingleScene(int sceneIndex) 
        {
            if (IsValidSceneIndex(sceneIndex) && CanLoadScene(sceneIndex))
            {
                if (currentLoadingScreen) currentLoadingScreen.gameObject.SetActive(true);
               
                onSingleSceneLoadStarted?.Invoke(sceneIndex);
                StartSceneLoad(sceneIndex);
            }
        }

        public void LoadScene(string sceneName)
        {
            SceneData data = GetSceneData(sceneName);
            if (data != null) { LoadScene(data.SceneIndex); }
        }

        public void LoadScene(uint sceneIndex)
        {
            LoadSingleScene((int)sceneIndex);
        }

        public void LoadScene(int sceneIndex) 
        {
            if (IsValidSceneIndex(sceneIndex) && CanLoadScene(sceneIndex))
                onSingleSceneLoadStarted.Invoke(sceneIndex);
        }

        public bool CanLoadScene(int sceneIndex)
        {
            if (sceneLoadingOperations == null || sceneLoadingOperations.ContainsKey(sceneIndex) || IsLoadingSingleScene) return false;
            Debug.Log("Is Unlocked : " + IsSceneUnlocked(sceneIndex) + " | Is Loaded Scene : " + IsSceneLoaded(sceneIndex));


            if (!IsSceneLoaded(sceneIndex) && IsSceneUnlocked(sceneIndex)) return true;

            return false;
        }

        public bool IsSceneLoaded(int sceneIndex)
        {
            if(!IsValidSceneIndex(sceneIndex)) return false; 
            return SceneManager.GetSceneByBuildIndex(sceneIndex).isLoaded;
        }

        public bool IsSceneUnlocked(string sceneIndex)
        {
            return IsSceneUnlocked(GetSceneData(sceneIndex));
        }

        public bool IsSceneUnlocked(int sceneIndex)
        {
            return IsSceneUnlocked(GetSceneData(sceneIndex));
        }

        private bool IsSceneUnlocked(SceneData sceneData)
        {
            if (sceneData != null) return sceneData.IsUnlocked;
            return false;
        }

        public bool IsValidSceneIndex(int sceneIndex)
        {
            if(sceneIndex < 0 || sceneIndex >= SceneManager.sceneCountInBuildSettings) return false;
            return true;
        }

        public string GetSceneName(int sceneIndex, bool getActualName)
        {
            if (!IsValidSceneIndex(sceneIndex)) return null;

            SceneData  data = GetSceneData(sceneIndex);
            if (data != null) return getActualName ? data.BuildSettingSceneName : data.CustomSceneName;

            return string.Empty;

        }

        public string[] GetSceneNames()
        {
            List<string> names = new List<string>();
            if (sceneDatas == null || sceneDatas.Length == 0) return names.ToArray();

            foreach(SceneData data in sceneDatas) {
                if (data == null) names.Add("Data Not Found");
                else names?.Add("Build Setting Scene Name: " + data.BuildSettingSceneName + " | " + "Custom Scene Name: " + data.CustomSceneName);
            }

            return names.ToArray();
        }

        public SceneData GetSceneData(string sceneName)
        {
            if (sceneDatas == null || string.IsNullOrEmpty(sceneName)) return null;
            var results = sceneDatas.Where(x => x != null).Where(x => x.Compare(sceneName));

            return results.Count() > 0 ? results.First() : null; 
        }

        public SceneData[] GetScenesDatas(string[] sceneNames)
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

        public SceneData GetSceneData(int sceneIndex)
        {
            if (sceneDatas == null || sceneDatas.Length <= 0 || 
                sceneIndex < 0     || sceneIndex >= sceneDatas.Length) return null;

            return sceneDatas[sceneIndex];
        }

        public SceneData[] GetSceneDatas(int[] sceneIndexes)
        {
            List<SceneData> results = new List<SceneData>();
            if (sceneIndexes == null || sceneDatas == null) return results.ToArray();

            foreach (int sceneIndex in sceneIndexes)
            {
                SceneData data = GetSceneData(sceneIndex);
                if (data != null) { results.Add(data); }
            }

            return results.ToArray();
        }

     
        
    }
}