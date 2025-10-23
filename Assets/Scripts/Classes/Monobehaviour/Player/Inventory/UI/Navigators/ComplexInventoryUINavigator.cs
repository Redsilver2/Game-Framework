using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Interactions.Items;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public sealed class ComplexInventoryUINavigator : InventoryUINavigator
    {
        

        [Space]
        [SerializeField] private UnityEvent<int> onVerticalIndexChanged;

        private OverrideablePressInput nextVerticalPressInput;
        private OverrideablePressInput previousVerticalPressInput;

        private int verticalIndex;
        public  int VerticalIndex => verticalIndex;

        public const string NEXT_VERTICAL_INPUT_NAME     = "Next Vertical Navigator Input";
        public const string PREVIOUS_VERTICAL_INPUT_NAME = "Previous Vertical Navigator Input";


        protected sealed override void Awake()
        {
            base.Awake();

            nextVerticalPressInput     = GetNextVerticalInput();
            previousVerticalPressInput = GetPreviousVerticalInput();

            nextVerticalPressInput.Enable();
            previousVerticalPressInput.Enable();
        }

        public void AddOnVerticalIndexChangedListener(UnityAction<int> action) 
        {
            if (onVerticalIndexChanged != null && action != null)
                onVerticalIndexChanged.AddListener(action);
        }

        public void RemoveOnVerticalIndexChangedListener(UnityAction<int> action) 
        {
              if(onVerticalIndexChanged != null && action != null)
                  onVerticalIndexChanged.RemoveListener(action);
        }
        protected sealed override void UpdateInput() 
        {
            base.UpdateInput();

            if (nextVerticalPressInput != null && previousVerticalPressInput != null) {
                nextVerticalPressInput.Update();
                previousVerticalPressInput.Update();

                if      (nextVerticalPressInput.Value)     IncrementVerticalIndex();
                else if (previousVerticalPressInput.Value) DecrementVerticalIndex();
            }
        }

        private void DecrementVerticalIndex()  {
            verticalIndex--;
            if(verticalIndex < 0) verticalIndex = GetMaxVerticalIndex() - 1;

            int maxHorizontalIndex = GetMaxHorizontalIndex();
            if(horizontalIndex >= maxHorizontalIndex) horizontalIndex = maxHorizontalIndex - 1;

            if(onVerticalIndexChanged != null) onVerticalIndexChanged.Invoke(verticalIndex);
        }

        private void IncrementVerticalIndex() 
        {
            verticalIndex++;
            if (verticalIndex >= GetMaxVerticalIndex()) verticalIndex = 0;

            int maxHorizontalIndex = GetMaxHorizontalIndex();
            if (horizontalIndex >= maxHorizontalIndex) horizontalIndex = maxHorizontalIndex - 1;

            if (onVerticalIndexChanged != null) onVerticalIndexChanged.Invoke(verticalIndex);
        }

        public sealed override void SetIndex(Item item) 
        {
            if(inventory is ComplexInventory && item != null)
            {
                ComplexInventory inventory = (this.inventory as ComplexInventory);
               
                if(inventory.Contains(item))
                    inventory.GetItemIndexes(item, out verticalIndex, out horizontalIndex);
            }
        }

        protected sealed override int GetMaxHorizontalIndex() {
            if (inventory is ComplexInventory)
                return (inventory as ComplexInventory).GetMaxHorizontalIndex(verticalIndex);
            return -1;
        }

        private int GetMaxVerticalIndex() 
        {
            if (inventory is ComplexInventory)
                return (inventory as ComplexInventory).GetMaxVerticalIndex();
            return -1;
        }

        public static OverrideablePressInput GetNextVerticalInput() {
            return InputManager.GetOrCreateOverrideablePressInput(NEXT_VERTICAL_INPUT_NAME, KeyboardKey.S, GamepadButton.DpadDown); 
        }


        public OverrideablePressInput GetPreviousVerticalInput() {
            return InputManager.GetOrCreateOverrideablePressInput(PREVIOUS_VERTICAL_INPUT_NAME, KeyboardKey.W, GamepadButton.DpadUp);
        }
    }
}
