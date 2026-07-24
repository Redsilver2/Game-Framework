using UnityEngine;

namespace RedSilver2.Framework.Interactions.Actions.Modules
{
    public abstract class InteractionModuleEvent : MonoBehaviour {

        private void Awake() {
            SetInteractionModule(GetComponent<InteractionModule>());
        }

        protected abstract void Start();
        protected abstract void OnEnable();
        protected abstract void OnDisable();

        protected abstract void SetInteractionModule(InteractionModule module);
    }
}
