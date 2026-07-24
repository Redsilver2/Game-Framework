using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework.Interactions {
    public sealed class TriggerableDoor : MonoBehaviour {

        private Door door;

        private void Awake()
        {
            door = transform.root != null ? transform.root.GetComponentInChildren<Door>() : GetComponentInChildren<Door>();
            door?.SetIsInteractable(false);
        }

        private void Start() {
            StartCoroutine(ForceUpdate(new WaitForSeconds(0.25f)));
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

        private IEnumerator ForceUpdate(WaitForSeconds wait) {
            while (door != null) {
                door?.SetIsInteractable(false);
                yield return wait;
            }
        }
    }
}
