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


        private UnityAction  onUpdate;
    
        private UnityEvent   onEnable;
        private UnityEvent   onDisable;
        private UnityEvent   onLateUpdate;


        private InputHandler handler;
 
        public bool IsInitialized => isInitialized;
        public bool IsEnabled     => isEnabled;

        public InputConfiguration(InputSettings settings) {
            onLateUpdate = new UnityEvent();
            onEnable     = new UnityEvent();
            onDisable    = new UnityEvent();

            AddOnEnableListener(() =>
            {
                isEnabled = true;
                handler?.Enable();
            });

            AddOnDisableListener(() =>
            {
                isEnabled = false;
                handler?.Disable(); 
            });

            if (settings != null) {
                InputName     = settings.InputName;
                isEnabled     = settings.IsEnabled;
                isInitialized = settings.IsInitialized;
            }

            InputManager.AddInputConfiguration(InputName, this);
        }


        public void Enable() {
            if (isEnabled || !isInitialized) return;
            onEnable?.Invoke();
        }

        public void Disable() {
            if (!isEnabled || !isInitialized) return;
            onDisable?.Invoke();  
        }

        protected virtual void Initialize() {
            if(isInitialized) return;

            isEnabled = false;
            isInitialized = true;
            
            onUpdate  = GetOnUpdate();
         
            Initialize(InputName, ref handler);
        }

        public void Update()
        {
            if(!isEnabled || !isInitialized) return;
            onUpdate?.Invoke(); 
        }


        public void LateUpdate()
        {
            if (!isEnabled || !isInitialized) return;
            onLateUpdate?.Invoke();
        }

        public void AddOnLateUpdateListener(UnityAction action)
        {
            if(action != null) onLateUpdate?.AddListener(action);   
        }
        public void RemoveOnLateUpdateListener(UnityAction action)
        {
            if (action != null) onLateUpdate?.RemoveListener(action);
        }

        public void AddOnEnableListener(UnityAction action)
        {
            if (action != null) onEnable?.AddListener(action);
        }
        public void RemoveOnEnableListener(UnityAction action)
        {
            if (action != null) onEnable?.RemoveListener(action);
        }

        public void AddOnDisableListener(UnityAction action)
        {
            if (action != null) onDisable?.AddListener(action);
        }
        public void RemoveOnDisableListener(UnityAction action)
        {
            if (action != null) onDisable?.RemoveListener(action);
        }


        protected InputHandler GetInput() { return handler; }
        protected abstract UnityAction GetOnUpdate();

        public abstract bool IsOverrideable();
        public abstract void Reset();
        protected abstract void Initialize(string name, ref InputHandler handler); 

    }
}
