using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.StateMachines.Controllers;

namespace RedSilver2.Framework.Stats {
    public sealed class PlayerHealth : Health
    {
        private PlayerController controller;

        protected override void Awake() {
            base.Awake();
            controller = transform.parent != null ? transform.parent.GetComponentInChildren<PlayerController>() 
                                                  : GetComponentInChildren<PlayerController>();

            AddOnProgressChangedListener(progress => {
                if(progress <= 0f) {
                    if(controller != null) controller.enabled = false;
                }
            });
        }
    }
}
