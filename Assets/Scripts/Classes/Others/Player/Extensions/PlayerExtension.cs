using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine
    {
        public abstract class PlayerExtension
        {
            protected readonly PlayerStateMachine owner;
            private   readonly List<PlayerState> states;

            private UnityEvent onEnable;
            private UnityEvent onDisable;

            protected bool wasEventAdded;

            private bool isEnabled = false;
            public bool IsEnabled => isEnabled;

            protected PlayerState[] States
            {
                get
                {
                    if (states == null) return null;
                    return states.ToArray();
                }
            }

            protected PlayerExtension(PlayerStateMachine owner)
            {
                this.owner = owner;
                this.states = new List<PlayerState>();

                this.onEnable = new UnityEvent();
                this.onDisable = new UnityEvent();

                this.wasEventAdded = false;
                this.isEnabled = false;
                SetExtension();
            }

            private void SetExtension()
            {
                if (owner != null)
                {
                    owner.AddOnStateAddedListener(OnStateAdded);
                    owner.AddOnStateRemovedListener(OnStateRemoved);

                    AddOnDisableListener(OnDisable);
                    AddOnEnableListener(OnEnable);
                }
            }

            public void AddOnEnableListener(UnityAction action)
            {
                if (onEnable != null && action != null) onEnable.AddListener(action);
            }
            public void RemoveOnEnableListener(UnityAction action)
            {
                if (onEnable != null && action != null) onEnable.RemoveListener(action);
            }

            public void AddOnDisableListener(UnityAction action)
            {
                if (onDisable != null && action != null) onDisable.AddListener(action);
            }
            public void RemoveOnDisableListener(UnityAction action)
            {
                if (onDisable != null && action != null) onDisable.RemoveListener(action);
            }

            protected bool ContainsState(PlayerState state)
            {
                if (state == null || states == null) return false;
                return states.Contains(state);
            }

            protected virtual void OnStateAdded(PlayerState state)
            {
                if (states != null && state != null)
                {
                    if (!states.Contains(state))
                    {
                        states.Add(state);
                    }
                }
            }

            protected virtual void OnStateRemoved(PlayerState state)
            {
                if (states != null && state != null)
                {
                    if (states.Contains(state))
                    {
                        states.Remove(state);
                    }
                }
            }

            public void SetStateEvent(PlayerState state)
            {
               if(state != null) OnStateAdded(state);   
            }

            public void Enable()
            {
                onEnable.Invoke();
            }

            public void Disable()
            {
                onDisable.Invoke();
            }

            protected virtual void OnEnable()
            {
                if (!isEnabled) isEnabled = true;
            }

            protected virtual void OnDisable()
            {
                if (isEnabled) isEnabled = false;
            }

            public bool Compare(string extensionName) => extensionName.ToLower() == GetExtensionName().ToLower();   
            protected abstract string GetExtensionName();
        }

         

        public void AddExtension(PlayerExtension extension)
        {
            if (extensions != null && extension != null)
            {
                if(extensions.Where(x => x.GetType() == extension.GetType()).Count() == 0)
                {
                    extensions.Add(extension);
                    SetPlayerExtensionForStates(extension);
                }
            }
        }


        public void AddExtensions(PlayerExtension[] extensions)
        {
            if(extensions != null)
            {
                foreach(PlayerExtension extension in extensions)
                {
                    AddExtension(extension);
                }
            }
        }

        private void SetPlayerExtensionForStates(PlayerExtension extension)
        {
            if (extension != null && states != null)
            {
                foreach(PlayerState state in states.Values) extension.SetStateEvent(state);
            }
        }

        private void SetPlayersExtensionForState(PlayerState state)
        {
            if (state != null && states != null && extensions != null)
            {
                foreach (PlayerExtension extension in extensions) { extension.SetStateEvent(state); }
            }
        }

        public PlayerExtension GetExtension(string extensionName)
        {
            if(string.IsNullOrEmpty(extensionName) || extensions == null) return null;
            var results = extensions.Where(x => x.Compare(extensionName));

            if (results.Count() > 0) return results.First();
            return null;
        }

        public PlayerExtension[] GetExtensions(string[] extensionNames)
        {
            List<PlayerExtension> results = new List<PlayerExtension>();

            if (extensionNames != null)
            {
                foreach (var extensionName in extensionNames)
                {
                    PlayerExtension extension = GetExtension(extensionName);
                    if(extension != null) results.Add(extension);
                }
            }

            return results.ToArray();
        }


    }
}
