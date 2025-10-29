using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Interactions.Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class InventoryUINavigator : MonoBehaviour
    {
        [SerializeField] private Transform modelParentTransform;

        [Space]
        [SerializeField] protected Inventory inventory;

        [Space]
        [SerializeField] private bool canWrapHorizontalIndex = true;


        [Space]
        [SerializeField] private UnityEvent onUpdate, onLateUpdate, onEnable, onDisable;

        [Space]
        [SerializeField] private UnityEvent<int> onHorizontalIndexChanged;

        protected int horizontalIndex;

        private OverrideablePressInput nextHorizontalPressInput;
        private OverrideablePressInput previousHorizontalPressInput;

        private static List<InventoryUINavigator> navigators;
        private static InventoryUINavigator current;

        public int HorizontalIndex      => horizontalIndex;
        public Transform ModelParentTransform => modelParentTransform;

        public InventoryUINavigator[] Navigators
        {
            get
            {
                if(navigators == null) return new InventoryUINavigator[0];
                return navigators.ToArray();
            }
        }

        public const string NEXT_HORIZONTAL_INPUT_NAME     = "Next Horizontal Navigator Input";
        public const string PREVIOUS_HORIZONTAL_INPUT_NAME = "Previous Horizontal Navigator Input";

        protected virtual void Awake() 
        {
            this.enabled    = false;
            horizontalIndex = 0;

            nextHorizontalPressInput     = GetNextHorizontalInput();
            previousHorizontalPressInput = GetPreviousHorizontalInput();

            nextHorizontalPressInput.Enable();
            previousHorizontalPressInput.Enable();


            inventory.AddOnItemAddedListener(OnItemAdded);
            inventory.AddOnItemRemovedListener(OnItemRemoved);

            inventory.AddOnOpenUIListener(OnOpenInventoryUI);
            inventory.AddOnCloseUIListener(OnCloseInventoryUI);
            AddOnUpdateListener(UpdateInput);
        }

        private void Update() {
            if(onUpdate != null) onUpdate.Invoke();
        }

        private void LateUpdate()  {
            if (onLateUpdate != null) onLateUpdate.Invoke();
        }

        private void OnDisable() 
        {
            if (current != this) return;
            if (onDisable != null && didAwake) onDisable.Invoke();
        }

        private void OnEnable() {
            if (current != this) { enabled = false; return; }
            if (onEnable != null && didAwake) onEnable.Invoke();
        }


        protected virtual void OnOpenInventoryUI()
        {
            UpdateModels();
            SetCurrent(this);
        }

        protected virtual void OnCloseInventoryUI()
        {
            UpdateModels();
            SetCurrent(null);
        }

        public void AddOnInventoryOpenUIListener(UnityAction action)
        {
            if(inventory != null) inventory.AddOnOpenUIListener(action);
        }
        public void RemoveOnInventoryOpenUIListener(UnityAction action)
        {
            if (inventory != null) inventory.RemoveOnOpenUIListener(action);
        }


        public void AddOnInventoryCloseUIListener(UnityAction action)
        {
            if (inventory != null) inventory.AddOnCloseUIListener(action);
        }
        public void RemoveOnInventoryCloseUIListener(UnityAction action)
        {
            if (inventory != null) inventory.RemoveOnCloseUIListener(action);
        }

        public void AddOnInventoryItemAddedListener(UnityAction<Item> action)
        {
            if (inventory != null) inventory.AddOnItemAddedListener(action);
        }
        public void RemoveOnInventoryItemAddedListener(UnityAction<Item> action)
        {
            if (inventory != null) inventory.RemoveOnItemAddedListener(action);
        }

        public void AddOnInventoryItemRemovedListener(UnityAction<Item> action)
        {
            if (inventory != null) inventory.AddOnItemRemovedListener(action);
        }
        public void RemoveOnInventoryItemRemovedListener(UnityAction<Item> action)
        {
            if (inventory != null) inventory.RemoveOnItemRemovedListener(action);
        }


        public void AddOnEnableListener(UnityAction action) {
            if (onEnable != null && action != null)
                onEnable.AddListener(action);
        }
        public void RemoveOnEnableListener(UnityAction action)
        {
            if (onEnable != null && action != null)
                onEnable.RemoveListener(action);
        }

        public void AddOnDisableListener(UnityAction action)
        {
            if (onDisable != null && action != null)
                onDisable.AddListener(action);
        }
        public void RemoveOnDisableListener(UnityAction action)
        {
            if (onDisable != null && action != null)
                onDisable.RemoveListener(action);
        }

        public void AddOnUpdateListener(UnityAction action)
        {
            if(onUpdate != null && action != null)
                onUpdate.AddListener(action);
        }
        public void RemoveOnUpdateListener(UnityAction action)
        {
            if (onUpdate != null && action != null)
                onUpdate.RemoveListener(action);
        }

        public void AddOnLateUpdateListener(UnityAction action)
        {
            if (onLateUpdate != null && action != null)
                onLateUpdate.AddListener(action);
        }
        public void RemoveOnLateUpdateListener(UnityAction action)
        {
            if (onLateUpdate != null && action != null)
                onLateUpdate.RemoveListener(action);
        }

        public void AddOnHorizontalIndexChangedListener(UnityAction<int> action)
        {
            Debug.LogWarning(action);
            if (onHorizontalIndexChanged != null && action != null)
               onHorizontalIndexChanged.AddListener(action);
        }

        public void RemoveOnHorizontalIndexChangedListener(UnityAction<int> action)
        {
            if (onHorizontalIndexChanged != null && action != null)
                onHorizontalIndexChanged.RemoveListener(action);
        }

        protected virtual void OnItemAdded(Item item)
        {
            if (item != null && inventory != null)
            {
                Debug.LogWarning("We added item...");
                UpdateItems();

                if (inventory.IsUIOpened) 
                    UpdateModels();
            }
        }

        protected virtual void OnItemRemoved(Item item)
        {
            if (item != null)
            {
                UpdateItems();

                if (inventory.IsUIOpened)
                    UpdateModels();
            }
        }

        protected virtual void UpdateInput()
        {
            if (nextHorizontalPressInput != null) {
                nextHorizontalPressInput.Update();
                if(nextHorizontalPressInput.Value) IncrementHorizontalIndex();
            }

            if(previousHorizontalPressInput != null){
                previousHorizontalPressInput.Update();
                if (previousHorizontalPressInput.Value) DecrementHorizontalIndex();
            }
        }

        private void IncrementHorizontalIndex() 
        {
           if (inventory == null) return;
           horizontalIndex++;
           ClampIncrementHorizontalIndex(ref horizontalIndex, canWrapHorizontalIndex);
           if (onHorizontalIndexChanged != null) onHorizontalIndexChanged.Invoke(horizontalIndex);
        }

        private void DecrementHorizontalIndex() 
        {
            if (inventory == null) return;
            horizontalIndex--;
            ClampDecrementHorizontalIndex(ref horizontalIndex, canWrapHorizontalIndex);
            if (onHorizontalIndexChanged != null) onHorizontalIndexChanged.Invoke(horizontalIndex);
        }

        protected virtual void ClampIncrementHorizontalIndex(ref int horizontalIndex, bool canWarpHorizontalIndex) {

            int maxHorizontalIndex = GetMaxHorizontalIndex();
            
            if (horizontalIndex >= maxHorizontalIndex) {
                if(canWarpHorizontalIndex) horizontalIndex = 0;
                else                       horizontalIndex = maxHorizontalIndex - 1;
            }
        }

        protected virtual void ClampDecrementHorizontalIndex(ref int horizontalIndex, bool canWarpHorizontalIndex) 
        {
            if (horizontalIndex < 0) {
               if(canWarpHorizontalIndex) horizontalIndex = GetMaxHorizontalIndex() - 1;
               else                       horizontalIndex = 0;
            }
        }

        public abstract void Select(Item item);
        public abstract int GetMaxHorizontalIndex();

        protected abstract void UpdateItems();
        protected abstract void UpdateModels();

        public abstract Item GetSelectedItem();

        public static void Disable() {
            if (current != null) current.enabled = false;
        }

        public static void Enable() {
            if (current != null) current.enabled = true;
        }

        public static void SetCurrent(InventoryUINavigator navigator) 
        {
            if (current != navigator) {
                Disable();
                current = navigator;
                Enable();
            }
        }

        public static OverrideablePressInput GetNextHorizontalInput() {
            return InputManager.GetOrCreateOverrideablePressInput(NEXT_HORIZONTAL_INPUT_NAME, KeyboardKey.D, GamepadButton.DpadRight);
        }

        public static OverrideablePressInput GetPreviousHorizontalInput() {
            return InputManager.GetOrCreateOverrideablePressInput(PREVIOUS_HORIZONTAL_INPUT_NAME, KeyboardKey.A, GamepadButton.DpadLeft); ;
        }
    }
}
