using UnityEngine;

namespace RedSilver2.Framework.Scenes
{
    public abstract class SceneLoaderEvent : MonoBehaviour {
        private SceneLoaderManager sceneLoaderManager;

        public void Start() {
            sceneLoaderManager = GameManager.SceneLoaderManager;
            SetEvents(sceneLoaderManager, true);
        }

        public void OnEnable() {
            SetEvents(sceneLoaderManager, true);
        }

        private void OnDisable() {
            SetEvents(sceneLoaderManager, false);
        }

        protected abstract void SetEvents(SceneLoaderManager manager, bool isAddingEvents); 
    }
}
