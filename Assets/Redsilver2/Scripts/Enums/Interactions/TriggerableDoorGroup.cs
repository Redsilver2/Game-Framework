using UnityEngine;

namespace RedSilver2.Framework.Interactions {

    [RequireComponent(typeof(DoorGroup))]
    public class TriggerableDoorGroup : MonoBehaviour {
        private DoorGroup doorGroup;

        private void Awake()
        {
            doorGroup = GetComponent<DoorGroup>();
            if (gameObject.TryGetComponent(out Collider collider)) collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.ToLower() == "player") doorGroup?.UpdateDoors(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag.ToLower() == "player") doorGroup?.UpdateDoors(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag.ToLower() == "player") doorGroup?.UpdateDoors(true);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag.ToLower() == "player") doorGroup?.UpdateDoors(false);
        }
    }
}