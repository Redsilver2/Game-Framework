using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RedSilver2.Framework.UI
{
    public class TMP_DropdownUISelection : UISelection
    {
        private bool isDropdownClosed;

        private UnityEvent<GameObject> onOptionSelected;
        private UnityEvent<GameObject> onOptionDeselected;

        private TMP_Dropdown dropdown;
        private IEnumerator onUpdateDropdown;

        protected override void Awake()
        {
            base.Awake();
            dropdown         = GetComponentInChildren<TMP_Dropdown>();
            onUpdateDropdown = null;

            AddOnDeselectListener(() => {
                isDropdownClosed = true;
                dropdown?.Hide();
            });
        }

        private async void Start()
        {
            await Awaitable.WaitForSecondsAsync(0.1f);
            OpenDropdown();
            await Awaitable.WaitForSecondsAsync(2f);
            CloseDropdown();
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
                if      (GameUIController.GetNavigateUpState(true))    currentIndex--;
                else if (GameUIController.GetNavigateDownState(false)) currentIndex++;

                currentIndex = Mathf.Clamp(currentIndex, 0, dropdown.options.Count - 1);
                if (GameUIController.GetConfirmState()) dropdown.value = currentIndex;
            }
        }

        private void OpenDropdown()
        {
            if(dropdown != null && EventSystem.current != null)
            {
                isDropdownClosed = false;
                EventSystem.current.SetSelectedGameObject(dropdown.gameObject);
                ExecuteEvents.Execute(dropdown.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            }
       
        }

        private void CloseDropdown()
        {
            if(dropdown == null || EventSystem.current == null) return;
            isDropdownClosed = true;
            dropdown.Hide();
            EventSystem.current.SetSelectedGameObject(null);

        }

        private void OnUpdate()
        {
            if (GameUIController.GetConfirmState())
            {
                if (isDropdownClosed) {
                    isDropdownClosed = false;
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
            while (dropdown != null)
            {
                OnUpdate();
                yield return null;
            }
        }

        protected IEnumerator OnUpdateDropdown()
        {
            bool         isValuesSet = false;
            int          currentIndex   = 0;
            GameObject[] selectedObject = null;

            yield return new WaitForSeconds(0.5f);

            while (dropdown != null && !isDropdownClosed)
            {
                if (!isValuesSet) {
                    selectedObject = GetSelectedObjects();
                }

                isValuesSet = true;
                UpdateIndex(ref currentIndex);
                yield return null;
            }
        }

        private GameObject[] GetSelectedObjects()
        {
            GameObject result = GameObject.Find("Dropdown List/Viewport/Content");
            List<GameObject> gameObjects = new List<GameObject>();

            if (result == null || dropdown == null || result.transform.childCount == 0)
                return gameObjects.ToArray();

            for (int i = 0; i < result.transform.childCount; i++)
                gameObjects?.Add(result.transform.GetChild(i).gameObject);

            return gameObjects.ToArray();
        }


    }
}
