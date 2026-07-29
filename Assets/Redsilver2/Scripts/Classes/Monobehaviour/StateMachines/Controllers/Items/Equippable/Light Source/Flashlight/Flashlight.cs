using RedSilver2.Framework.Items;

namespace RedSilver2.Framework.Interactions.Items
{
    public class Flashlight : LightSourceItem
    {
        protected virtual void Start()
        {

        }

        public override void AddAction(EquippableItemAction action)
        {
            if(action is FlashlightAction)
                base.AddAction(action);
        }

        public override void RemoveAction(EquippableItemAction action)
        {
            if (action is FlashlightAction)
                base.RemoveAction(action);
        }
    }
}
