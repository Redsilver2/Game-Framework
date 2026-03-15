using RedSilver2.Framework.Interactions.Items;

namespace RedSilver2.Framework.Items
{
    public abstract class FlashlightAction : EquippableItemAction
    {
        private Flashlight flashlight;

        protected override void Awake() {
            base.Awake();
            AddOnDisabledListener(() => { flashlight?.RemoveAction(this); });
            AddOnEnabledListener(() => { flashlight?.AddAction(this); });
        }

        protected sealed override void SetItem(EquippableItem item)
        {
            if (item is Flashlight && flashlight == null && flashlight != item)
            {
                flashlight = item as Flashlight;
                flashlight?.AddAction(this);
            }
        }

        public sealed override bool CanUpdate()
        {
            return CanUpdate(flashlight);
        }


        protected bool TryGetFlashlight(out Flashlight flashlight)
        {
            flashlight = this.flashlight;
            return flashlight != null;
        }

        protected abstract bool CanUpdate(Flashlight flashlight);
    }

}
