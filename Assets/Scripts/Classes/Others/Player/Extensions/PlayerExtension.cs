using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine
    {
        public abstract class PlayerExtension
        {
            protected readonly PlayerStateMachine owner;
            private readonly List<PlayerState> states;

            private UnityEvent onEnable;
            private UnityEvent onDisable;


            protected readonly string[] compatibleStates;


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

                this.isEnabled = false;
                this.compatibleStates = GetCompatibleStates();

                AddOnEnableListener(OnEnable);
                AddOnDisableListener(OnDisable);
            }

            protected virtual void OnEnable()
            {
                isEnabled = true;
                if (owner != null) owner.AddOnStateEnterListener(OnStateEnter);
            }

            protected virtual void OnDisable()
            {
                isEnabled = false;
                if (owner != null) owner.RemoveOnStateEnterListener(OnStateEnter);
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

            protected abstract void OnStateEnter(PlayerState state);

            public void Enable()
            {
                if(!isEnabled) onEnable.Invoke();
            }

            public void Disable()
            {
                if(isEnabled) onDisable.Invoke();
            }

            public bool Compare(string extensionName) => extensionName.ToLower() == GetExtensionName().ToLower();
            public bool IsCompatibleState(PlayerState state)
            {
                if (owner == null || state == null) return false;
                if(compatibleStates == null || compatibleStates.Length == 0) return true;
                return compatibleStates.Where(x => state.GetStateName().ToLower() == x.ToLower()).Count() > 0;
            }

            public abstract bool Compare(PlayerExtension extension);
            protected abstract string GetExtensionName();
            protected abstract string[] GetCompatibleStates();
        }

        
        public void AddExtension(PlayerExtensionModule module)
        {
            if (module == null) return;
            PlayerExtension extension = module.GetExtension();

            if (extensions != null && extension != null)
            {
                if(extensions.Where(x => x.Compare(extension)).Count() == 0)
                {
                    extensions.Add(extension);
                }
            }
        }


        public void AddExtensions(PlayerExtensionModule[] modules)
        {
            if(modules != null)
            {
                foreach(PlayerExtensionModule module in modules)
                {
                    AddExtension(module);
                }
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
