using RedSilver2.Framework.Animations;
using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Items;
using UnityEngine;

namespace RedSilver2.Framework.Interactions.Items
{
    public class Flashlight : LightSourceItem
    {
        protected override void Start()
        {
            base.Start();
            SetLightState();
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
