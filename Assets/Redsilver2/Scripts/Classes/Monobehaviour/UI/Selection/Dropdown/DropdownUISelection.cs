using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RedSilver2.Framework.UI
{
    public class DropdownUISelection : UISelection
    {
        private bool     isDropdownClosed;
        private Dropdown dropdown;

        protected override void Awake()
        {
            base.Awake();
            dropdown = GetComponentInChildren<Dropdown>();
            AddOnDeselectListener(() => {
                isDropdownClosed = true;
                dropdown?.Hide(); 
            });
        }

        public void AddOnValueChangeListener(UnityAction<int> action)
        {
            if (action != null)
                dropdown?.onValueChanged.AddListener(action);
        }
        public void RemoveOnValueChangeListener(UnityAction<int> action)
        {
            if (action != null)
                dropdown?.onValueChanged.RemoveListener(action);
        }

        private void UpdateIndex(ref int currentIndex)
        {
            if (!isDropdownClosed && dropdown != null)
            {
                if (GameUIController.GetNavigateUpState(true))         currentIndex--;
                else if (GameUIController.GetNavigateDownState(false)) currentIndex++;

                currentIndex = Mathf.Clamp(currentIndex, 0, dropdown.options.Count - 1);
                if (GameUIController.GetConfirmState()) dropdown.value = currentIndex;
            }
        }

        private void OnUpdate()
        {
            if (GameUIController.GetConfirmState())
            {
                if (isDropdownClosed) {
                    isDropdownClosed = false;
                    dropdown?.Show();
                }
                else
                {
                    isDropdownClosed = true;
                    dropdown?.Hide();
                }
            }
        }

        protected override IEnumerator UpdateCoroutine()
        {
            int currentIndex = dropdown.value;

            while (dropdown != null){
                UpdateIndex(ref currentIndex);
                OnUpdate();
                yield return null;
            }
        }


    }
}
