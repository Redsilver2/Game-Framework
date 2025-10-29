using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Interactions.Items;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class VerticalInventoryUINavigator : InventoryUINavigator
    {
        [Space]
        [SerializeField] private bool canWarpVerticalIndex = true;

        [Space]
        [SerializeField] private UnityEvent<int> onVerticalIndexChanged;

        private OverrideablePressInput nextVerticalPressInput;
        private OverrideablePressInput previousVerticalPressInput;

        protected int verticalIndex;
        public  int VerticalIndex => verticalIndex;

        public const string NEXT_VERTICAL_INPUT_NAME     = "Next Vertical Navigator Input";
        public const string PREVIOUS_VERTICAL_INPUT_NAME = "Previous Vertical Navigator Input";

        private Item      [,] items;
        private GameObject[,] models;

        public Item[,] Items        => items;
        public GameObject[,] Models => models;


        protected override void Awake()
        {
            base.Awake();
            verticalIndex = 0;

            nextVerticalPressInput     = GetNextVerticalInput();
            previousVerticalPressInput = GetPreviousVerticalInput();

            nextVerticalPressInput.Enable();
            previousVerticalPressInput.Enable();

            items = new Item[0, 0];
        }


        protected override void UpdateItems() {
            items = GetItems();
        }

        protected override void UpdateModels() {
            ItemModel.ReturnBorrowedModels(models);
            models = ItemModel.GetModels(items);
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

        private void DecrementVerticalIndex()  
        {
            verticalIndex--;
            ClampDecrementVerticalIndex(ref verticalIndex, canWarpVerticalIndex);
            if(onVerticalIndexChanged != null) onVerticalIndexChanged.Invoke(verticalIndex);
        }

        private void IncrementVerticalIndex()  
        {
            verticalIndex++;
            ClampIncrementVerticalIndex(ref verticalIndex, canWarpVerticalIndex);
            if (onVerticalIndexChanged != null) onVerticalIndexChanged.Invoke(verticalIndex);
        }

        protected virtual void ClampIncrementVerticalIndex(ref int verticalIndex, bool canWarpVerticalIndex)
        {
            int maxVerticalIndex = GetMaxVerticalIndex();
           
            if (verticalIndex >= maxVerticalIndex)
            {
               if(canWarpVerticalIndex) verticalIndex = 0;
               else                     verticalIndex = maxVerticalIndex - 1;
            }
        }

        protected virtual void ClampDecrementVerticalIndex(ref int verticalIndex, bool canWarpVerticalIndex)
        {
            if (verticalIndex < 0)
            {
               if(canWarpVerticalIndex) verticalIndex = GetMaxVerticalIndex() - 1;
               else                     verticalIndex = 0;
            }
        }


        public abstract int GetMaxVerticalIndex();
        public abstract Item[,] GetItems();

        public static OverrideablePressInput GetNextVerticalInput() {
            return InputManager.GetOrCreateOverrideablePressInput(NEXT_VERTICAL_INPUT_NAME, KeyboardKey.S, GamepadButton.DpadDown); 
        }


        public OverrideablePressInput GetPreviousVerticalInput() {
            return InputManager.GetOrCreateOverrideablePressInput(PREVIOUS_VERTICAL_INPUT_NAME, KeyboardKey.W, GamepadButton.DpadUp);
        }
    }
}
