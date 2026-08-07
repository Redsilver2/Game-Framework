using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Interactions
{
    public sealed class DoorGroup : MonoBehaviour {

        [Space]
        [SerializeField] private bool isOpen;

        private Door[] doors;

        public bool IsOpen => isOpen;

        private void Awake() {
            doors = GetComponentsInChildren<Door>(true);
        }

        private void Start() {
            UpdateDoors(isOpen, false);

            if (doors != null) {
                foreach (Door door in doors)  {
                    if (door == null) continue;
                    door?.SetIsInteractable(false);
                }
            }
        }

        public void UpdateDoors(bool isOpen) {
            UpdateDoors(isOpen, true);
        }

        private void UpdateDoors(bool isOpen, bool canCheckCondition)
        {
            if (doors == null) { return; }
            else if (canCheckCondition) {
                if (this.isOpen == isOpen) return;
            }

            this.isOpen = isOpen;

            foreach(Door door in doors) {
                if (door == null) continue;
                else if (isOpen) door?.Open();
                else             door?.Close();
            }
        }
    }
}
