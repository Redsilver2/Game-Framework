using RedSilver2.Framework.Interactions.Items;
using UnityEngine;

namespace RedSilver2.Framework.Items
{
    public class TurnOnFlashlightAction : TurnFlashlightAction
    {
        protected override bool CanUpdate(Flashlight flashlight)
        {
            if(flashlight == null) return false;
            return !flashlight.IsOn;
        }

        protected override void OnTurnEventTriggered(Flashlight flashlight) {
            flashlight?.SetState(true);
        }
    }
}
