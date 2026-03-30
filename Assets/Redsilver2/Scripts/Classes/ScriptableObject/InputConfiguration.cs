using RedSilver2.Framework.Inputs.Settings;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Inputs.Configurations
{
    public abstract class InputConfiguration 
    {
        public readonly string InputName;
      
        private bool isEnabled;
        private bool isInitialized;


        private UnityAction onUpdate;
        private InputHandler handler;
 
        public bool IsInitialized => isInitialized;
        public bool IsEnabled     => isEnabled;

        public InputConfiguration(InputSettings settings) {
            if(settings != null) {
                InputName     = settings.InputName;
                isEnabled     = settings.IsEnabled;
                isInitialized = settings.IsInitialized;
            }
        }


        public void Enable() {
            if (isEnabled || !isInitialized) return;
            isEnabled = true;
            handler?.Enable();

            Debug.Log("What?");
        }

        public void Disable() {
            if (!isEnabled || !isInitialized) return;
            isEnabled = false;
            handler?.Disable();


            Debug.Log("Wow?");
        }

        protected virtual void Initialize() {
            if(isInitialized) return;

            isEnabled = false;
            isInitialized = true;
            
            onUpdate  = GetOnUpdate();
         
            Initialize(InputName, ref handler);
            InputManager.AddInputConfiguration(InputName, this);
        }

        public void Update()
        {
            if(!isEnabled || !isInitialized) return;
            onUpdate?.Invoke(); 
        }

        protected InputHandler GetInput() { return handler; }
        protected abstract UnityAction GetOnUpdate();

        public abstract bool IsOverrideable();
        public abstract void Reset();
        protected abstract void Initialize(string name, ref InputHandler handler); 

    }
}
