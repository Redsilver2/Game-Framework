using RedSilver2.Framework.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Collectibles
{
    public abstract class CollectibleNotification : MonoBehaviour
    {
        private UnityEvent<Collectible> onShowNotification;
        private UnityEvent<Collectible> onHideNotification;

        private static List<string> registeredCollectible = new List<string>();

        protected virtual void Awake()
        {
            onHideNotification = new UnityEvent<Collectible>();
            onShowNotification = new UnityEvent<Collectible>();

            onShowNotification.AddListener(OnShowNotification);
            onHideNotification.AddListener(OnHideNotification);
        }

        protected virtual void OnShowNotification(Collectible collectible)
        {
            PlayerController.Disable();
            CameraControllerModule.Disable();
        }

        protected virtual void OnHideNotification(Collectible collectible)
        {
            PlayerController.Enable();
            CameraControllerModule.Enable();
        }

        public void AddOnShowNotificationListener(UnityAction<Collectible> action) {
            if(onShowNotification != null && action != null)
                onShowNotification.AddListener(action);
        }

        public void RemoveOnShowNotificationListener(UnityAction<Collectible> action)
        {
            if (onShowNotification != null && action != null)
                onShowNotification.RemoveListener(action);
        }

        public void AddOnHideNotificationListener(UnityAction<Collectible> action)
        {
            if (onHideNotification != null && action != null)
                onHideNotification.AddListener(action);
        }


        public void RemoveOnHideNotificationListener(UnityAction<Collectible> action)
        {
            if (onHideNotification != null && action != null)
                onHideNotification.RemoveListener(action);
        }

        public static bool WasDataRegistered(CollectibleData data)
        {
            GameObject model = GetDataModel(data);
            if (model == null) return false;
            return registeredCollectible.Contains(model.name.ToLower());
        }

        public static void RegisterData(CollectibleData data)
        {
            GameObject model = GetDataModel(data);
          
            if (model != null && registeredCollectible != null) 
            {
                if (!WasDataRegistered(data))
                {
                    registeredCollectible.Contains(model.name.ToLower());
                }
            }    
        }

        protected static GameObject GetDataModel(CollectibleData data)
        {
            if (data == null || data.Model == null) return null;
            return CollectibleModelViewer.GetCollectibleModel(data.Model.name);
        }

        public IEnumerator UpdateNotification(Collectible collectible)
        {
            onShowNotification.Invoke(collectible);

            yield return StartCoroutine(DisplayNotification(collectible));

            onHideNotification.Invoke(collectible);
        }

        protected abstract IEnumerator DisplayNotification(Collectible collectible);
    }
}