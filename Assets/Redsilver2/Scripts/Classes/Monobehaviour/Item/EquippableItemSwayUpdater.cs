using RedSilver2.Framework.Interactions.Items;
using UnityEngine;

namespace RedSilver2.Framework.Items
{
    [RequireComponent(typeof(SwayMotion))]
    public class EquippableItemSwayUpdater : MonoBehaviour {

        private SwayMotion swayMotion;
        // Add Mouse Rotation Event;
        private EquippableItem item;

        private void Start() {
            swayMotion = GetComponent<SwayMotion>();
            item       = transform.GetComponentInChildren<EquippableItem>();
            EnableEvents();
        }

        private void OnEnable() {
            EnableEvents();
        }

        private void OnDisable() {
            DisableEvents();
        }

        private void SetSwayMotionState(bool isEnabled)
        {
            if (swayMotion != null) 
                swayMotion.enabled = isEnabled;
        }

        private void EnableEvents() {
            
        }

        private void DisableEvents()
        {

        }  
    }
}
