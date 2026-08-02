using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    public class ItemStateMachine : UpdatableStateMachine
    {
        private float stateChangeCooldown;

        protected sealed override void OnLateUpdate() {

        }

        protected sealed override void OnUpdate() {
            if (stateChangeCooldown > 0f) {
                stateChangeCooldown = Mathf.Clamp(stateChangeCooldown - Time.deltaTime, 0f, float.MaxValue);
            }
        }

        public bool IsCooldownOver() {
            return stateChangeCooldown <= 0f;
        }
    }
}
