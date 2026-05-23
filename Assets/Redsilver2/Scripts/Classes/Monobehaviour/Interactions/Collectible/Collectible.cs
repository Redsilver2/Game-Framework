using UnityEngine;

namespace RedSilver2.Framework.Interactions.Collectibles
{
    public abstract class Collectible : MonoBehaviour
    {
        private InteractionModule interactionModule;

        protected virtual void Awake() {
            interactionModule = GetComponent<InteractionModule>();
            CollectibleModelViewer.AddCollectibleModel(GetData());
        }

        protected virtual void Start() {
            SetInteractionModuleEvents(true);
        }

        private void OnDestroy() {
            SetInteractionModuleEvents(false);
        }

        private void SetInteractionModuleEvents(bool isAddingEvents) 
        {
            if (interactionModule == null) return;

       
        }

        protected virtual void OnInteract(InteractionHandler handler) {
            CollectibleNotificationManager collectibleNotification = GameManager.CollectibleNotification;
            if(collectibleNotification != null) collectibleNotification.Notify(this);
           
            if (interactionModule != null) interactionModule.enabled = false;
            gameObject.SetActive(false);
        }

        protected void OnInteract(float progression, InteractionHandler handler) {
            if(progression >= 1f) 
                OnInteract(handler);
        }


        public abstract CollectibleData GetData();
    }
}