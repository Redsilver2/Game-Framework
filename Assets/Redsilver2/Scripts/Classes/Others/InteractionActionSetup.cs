using UnityEngine;

namespace RedSilver2.Framework.Interactions.Actions.Setups
{
    public abstract class InteractionActionSetup : MonoBehaviour {
        
        private void Start() {
           SetInteractionModule(GetComponent<InteractionModule>());   
        }

        protected abstract void OnEnable();
        protected abstract void OnDisable();
        protected abstract void SetInteractionModule(InteractionModule module);
    }
}
