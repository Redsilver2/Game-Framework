using RedSilver2.Framework.Interactions.Collectibles;
using UnityEngine;

namespace RedSilver2.Framework
{
    [RequireComponent(typeof(SteamManager))]
    public sealed class GameManager : MonoBehaviour
    {
        public const string GROUND_LAYER_NAME = "Ground";

        private CollectibleNotificationManager collectibleNotification;
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(Instance);

            collectibleNotification = FindAnyObjectByType<CollectibleNotificationManager>();
        }

        public static CollectibleNotificationManager GetCollectibleNotification() {
            GameManager manager = Instance;
            if (manager != null) return manager.collectibleNotification; 
            return null;
        }
    }
}
