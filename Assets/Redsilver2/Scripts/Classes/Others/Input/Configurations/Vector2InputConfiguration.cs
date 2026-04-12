using RedSilver2.Framework.Inputs.Settings;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.Inputs.Configurations
{
    public abstract class Vector2InputConfiguration : InputConfiguration
    {
        private bool isOverrideable;
        private UnityEvent<Vector2> onUpdated;

        public Vector2 Value { get; private set; }


        protected Vector2InputConfiguration(Vector2InputSettings settings) : base(settings)
        {
            onUpdated = new UnityEvent<Vector2>();
           


            if (settings != null) {
                isOverrideable = settings.IsOverrideable;
            }
        }

        protected sealed override UnityAction GetOnUpdate() {
            return () => {
                Vector2Input input = GetVector2Input();
             
                if (input == null) 
                    Value = Vector2.zero;
                else 
                    Value = input.Value;

                onUpdated?.Invoke(Value);
            };
        }

        public void AddOnUpdatedListener(UnityAction<Vector2> action) {
            if (action != null) onUpdated?.AddListener(action);
        }
        public void RemoveOnUpdatedListener(UnityAction<Vector2> action) {
            if (action != null) onUpdated?.RemoveListener(action);
        }

        public override bool IsOverrideable() {
            return isOverrideable;
        }    
        protected Vector2Input GetVector2Input() {
            return GetInput() as Vector2Input;
        }
    }
}
