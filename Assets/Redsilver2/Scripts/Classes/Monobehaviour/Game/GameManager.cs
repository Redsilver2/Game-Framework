using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Interactions;
using RedSilver2.Framework.Interactions.Collectibles;
using RedSilver2.Framework.Performance.Lights;
using RedSilver2.Framework.Player;
using RedSilver2.Framework.Scenes;
using RedSilver2.Framework.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.Dialogs;
using UnityEngine;

namespace RedSilver2.Framework
{
    [RequireComponent(typeof(SteamManager))]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private NotificationManager notification;
        [SerializeField] private SceneLoaderManager  sceneLoaderManager;
        
        [SerializeField] private DialogManager subtitleManager;
        [SerializeField] private SettingManager  settingManager;
        [SerializeField] private LightManager    lightManager;

        protected static GameManager instance;

        public static int InteractionLayer => LayerMask.NameToLayer(INTERACTION_LAYER_NAME);
        public static int GroundLayer      => LayerMask.NameToLayer(GROUND_LAYER_NAME);
        public static int PlayerLayer      => LayerMask.NameToLayer(PLAYER_LAYER_NAME);
        public static int AILayer          => LayerMask.NameToLayer(AI_LAYER_NAME);



        public static NotificationManager CollectibleNotification  => instance ? instance.notification : null;
        public static SceneLoaderManager SceneLoaderManager        => instance ? instance.sceneLoaderManager : null;
        public static DialogManager    DialogManager           => instance ? instance.subtitleManager    : null;
        public static SettingManager     SettingManager            => instance ? instance.settingManager     : null;
        public static LightManager       LightManager              => instance ? instance.lightManager       : null;

        public const string INTERACTION_LAYER_NAME = "Interaction";
        public const string GROUND_LAYER_NAME = "Ground";
        public const string PLAYER_LAYER_NAME = "Player";
        public const string AI_LAYER_NAME = "AI";


        protected virtual void Awake()
        {
            if (instance != null) { Destroy(gameObject); return; }
            instance = this;

            gameObject.name = "GameManager";
            DontDestroyOnLoad(instance);

            Application.targetFrameRate = 999;
        }

        private void Update() {
            InputManager.Update();
        }

        private void LateUpdate() {
            InputManager.LateUpdate();
        }

        public static void DisableControls()
        {
            PlayerController.Disable();
            CameraController.Disable();
            InteractionHandler.Disable();
        }

        public static void EnableControls()
        {
            PlayerController.Enable();
            CameraController.Enable();
            InteractionHandler.Enable();
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
