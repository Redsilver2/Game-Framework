using UnityEngine.Events;

namespace RedSilver2.Framework.Inputs
{
    public abstract class InputHandler
    {
        private bool isEnabled = false;
        private UnityEvent onEnable;
        private UnityEvent onDisable;

        public  bool IsEnabled => isEnabled;


        private InputHandler() { }

        protected InputHandler(string name) 
        { 
            onEnable  = new UnityEvent();
            onDisable = new UnityEvent();
            isEnabled = false;

            AddOnDisableListener(() => { isEnabled = false; });
            AddOnEnableListener(() => { isEnabled = true; });

            InputManager.AddInputHandler(name, this);
        }
        
        protected InputHandler(string name, bool isEnabled)
        {
            onEnable = new UnityEvent();
            onDisable = new UnityEvent();

            AddOnDisableListener(() => { isEnabled = false; });
            AddOnEnableListener(() => { isEnabled = true; });

            this.isEnabled = isEnabled;
            InputManager.AddInputHandler(name, this);
        }

        public void Enable() 
        { 
            if (!isEnabled) {
                onEnable?.Invoke();
            }
        }
        public void Disable() 
        {
            if (isEnabled)  {
                onDisable?.Invoke();
            }
        }

        public void AddOnEnableListener(UnityAction action)
        {
            if (action != null) onEnable?.AddListener(action);
        }

        public void RemoveOnEnableListener(UnityAction action)
        {
            if (action != null) onEnable?.AddListener(action);
        }

        public void AddOnDisableListener(UnityAction action)
        {
            if (action != null) onDisable?.AddListener(action);
        }

        public void RemoveOnDisableListener(UnityAction action)
        {
            if (action != null) onDisable?.AddListener(action);
        }

        public abstract string GetPaths();
    }
}
