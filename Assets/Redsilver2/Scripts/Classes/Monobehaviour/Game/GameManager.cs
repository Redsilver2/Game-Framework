using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Interactions.Collectibles;
using RedSilver2.Framework.Performance.Lights;
using RedSilver2.Framework.Scenes;
using RedSilver2.Framework.Settings;
using RedSilver2.Framework.Subtitles;
using UnityEngine;

namespace RedSilver2.Framework
{
    [RequireComponent(typeof(SteamManager))]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private CollectibleNotificationManager collectibleNotification;
        [SerializeField] private SceneLoaderManager sceneLoaderManager;
        
        [SerializeField] private SubtitleManager subtitleManager;
        [SerializeField] private SettingManager settingManager;
        [SerializeField] private LightManager  lightManager;

        protected static GameManager instance;


        public static int GroundLayer
        {
            get {
                return LayerMask.NameToLayer(GROUND_LAYER_NAME);
            }
        }

        public static int PLAYER_LAYER
        {
            get {
                return LayerMask.NameToLayer(PLAYER_LAYER_NAME);
            }
        }

        public static int AI_LAYER
        {
            get
            {
                return LayerMask.NameToLayer(AI_LAYER_NAME);
            }
        }


        public static CollectibleNotificationManager CollectibleNotification {
            get {
                return instance ? instance.collectibleNotification : null;
            }
        }

        public static SceneLoaderManager SceneLoaderManager {
            get {
                return instance ? instance.sceneLoaderManager : null;
            }
        }

        public static SubtitleManager SubtitleManager
        {
            get {
                return instance ? instance.subtitleManager : null;
            }
        }

        public static SettingManager SettingManager
        {
            get {
                return instance ? instance.settingManager : null;
            }
        }

        public static LightManager LightManager {
            get {
                return instance ? instance.lightManager : null;
            }
        }

        public const string GROUND_LAYER_NAME = "Ground";
        public const string PLAYER_LAYER_NAME = "Player";
        public const string AI_LAYER_NAME = "AI";


        protected virtual void Awake()
        {
            if (instance != null) { Destroy(gameObject); return; }
            instance = this;

            gameObject.name = "GameManager";

            Debug.unityLogger.logEnabled = false;
            DontDestroyOnLoad(instance);
        }

        private void Update() {
            InputManager.Update();
        }

        private void LateUpdate() {
            InputManager.LateUpdate();
        }

        public static bool IsGroundLayer(GameObject gameObject)
        {
            if(gameObject == null) return false;
            return gameObject.layer.Equals(GroundLayer);
        }

        protected static GameManager GetInstance() {
            return instance;
        }
    }
}
