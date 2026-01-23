using UnityEngine;
using UnityEngine.SceneManagement;

namespace RedSilver2.Framework.Scenes
{
    [CreateAssetMenu(menuName = "Scene/Data", fileName = "New Scene Data")]
    public class SceneData : ScriptableObject {

        [Space]
        [SerializeField] private int sceneIndex;
        [SerializeField] [TextArea(4,4)] private string  sceneDescription;

        [Space]
        [SerializeField] private bool      defaultSceneUnlockedState;

        [Space]
        [SerializeField] private Sprite[]  sceneThumbnails;

        private string sceneName;
        private SceneLoaderManager.SceneAsset asset;

        public string SceneName         => sceneName;
        public string SceneDescription  => sceneDescription;
        public int    SceneIndex        => sceneIndex;

        public Sprite[] SceneThumbnails => sceneThumbnails;
        public SceneLoaderManager.SceneAsset  Asset => asset;

        private void OnValidate() {
            int sceneCount = SceneManager.sceneCountInBuildSettings;

            if (sceneCount > 0) {
                sceneIndex = Mathf.Clamp(sceneIndex, 0, sceneCount - 1);
                sceneName = SceneManager.GetSceneByBuildIndex(sceneIndex).name;
            }
            else {
                sceneIndex = -1;
                sceneName = string.Empty;
            }
         
        }

        public void Load() {
            this.asset = SceneLoaderManager.SceneAsset.CreateAndGet(this, defaultSceneUnlockedState);
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
