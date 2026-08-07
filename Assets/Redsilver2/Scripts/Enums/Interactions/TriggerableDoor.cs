using UnityEngine;

namespace RedSilver2.Framework.Interactions {
    [RequireComponent(typeof(Door))]
    public sealed class TriggerableDoor : MonoBehaviour {

        private Door door;

        private void Awake()
        {
            door = GetComponent<Door>();
            door?.SetIsInteractable(false);

            if (gameObject.TryGetComponent(out Collider collider)) collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.tag.ToLower() == "player") door?.Open();
        }

        private void OnTriggerExit(Collider other) {
            if (other.tag.ToLower() == "player") door?.Close();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.tag.ToLower() == "player") door?.Open();
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if(collision.tag.ToLower() == "player") door?.Close();
        }
    }
}
