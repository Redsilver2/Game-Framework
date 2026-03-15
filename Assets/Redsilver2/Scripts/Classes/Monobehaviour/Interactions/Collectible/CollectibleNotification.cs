using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
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
            if (collectible != null) CollectibleNotification.RegisterData(collectible.GetData());
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

        public static bool WasDataRegistered(IPickableInteractable pickable)
        {
            GameObject model = GetDataModel(pickable);
            if (model == null) return false;
            return registeredCollectible.Contains(model.name.ToLower());
        }

        public static void RegisterData(IPickableInteractable pickable)
        {
            GameObject model = GetDataModel(pickable);
          
            if (model != null && registeredCollectible != null) 
            {
                if (!WasDataRegistered(pickable)) {
                    registeredCollectible.Add(model.name.ToLower());
                }
            }    
        }

        protected static GameObject GetDataModel(IPickableInteractable pickable)
        {
            if (pickable == null) return null;
            GameObject model = pickable.GetModel();

            if (model == null) return null;
            return CollectibleModelViewer.GetCollectibleModel(model.name);
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