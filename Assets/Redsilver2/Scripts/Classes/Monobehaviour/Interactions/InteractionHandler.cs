using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Inputs.Settings;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions
{
    public abstract class InteractionHandler : MonoBehaviour
    {
        [SerializeField] private string handlerName;
        [SerializeField] private float  interactionRange;

        [Space]
        [SerializeField] private PressInputSettings   pressSettings;
        [SerializeField] private HoldInputSettings    holdSettings;
        [SerializeField] private ReleaseInputSettings releaseSettings;

        [Space]
        [SerializeField] private InteractionType[] allowedInteractionTypes;
        private bool isEmptySelectedInteraction;

        private          InteractionModule        selectedInteraction;

        private UnityEvent<InteractionModule> onSelected;
        private UnityEvent<InteractionModule> onUnselected;

        public float InteractionRange => interactionRange;
        public bool IsSelectingNextInteraction => InputManager.GetKeyDown(KeyboardKey.UpArrow);
        public bool IsSelectingPreviousInteraction => InputManager.GetKeyDown(KeyboardKey.DownArrow);

        public InteractionModule SelectedInteraction => selectedInteraction;

        public PressInputSettings   PressSettings   => pressSettings;
        public HoldInputSettings    HoldSettings    => holdSettings;
        public ReleaseInputSettings ReleaseSettings => releaseSettings;

        protected static InteractionHandler Current {  get; private set; }
        private readonly static List<InteractionHandler> Instances = new List<InteractionHandler>();

        private readonly static Dictionary<Collider, InteractionModule> interactionModuleInstances = new Dictionary<Collider, InteractionModule>();
        private readonly static UnityEvent<InteractionModule> onInteractionModuleAdded             = new UnityEvent<InteractionModule>();
        private readonly static UnityEvent<InteractionModule> onInteractionModuleRemoved           = new UnityEvent<InteractionModule>();

        protected virtual void Awake() {
            this.isEmptySelectedInteraction = true;

            this.onSelected   = new UnityEvent<InteractionModule>();
            this.onUnselected = new UnityEvent<InteractionModule>();

            AddOnUnselectedListener(interaction => {
                interaction?.Unselect(this);
                this.selectedInteraction = null;
            });

            AddOnSelectedListener(interaction => {
                this.selectedInteraction = interaction;
                interaction?.Select(this);
            });

            Instances?.Add(this);
        }

        protected virtual void Start()
        {
            pressSettings?.Enable();
            holdSettings?.Enable();
            releaseSettings?.Enable();
        }

        private void OnEnable()
        {
            pressSettings?.Enable();
            holdSettings?.Enable();
            releaseSettings?.Enable();
        }

        private void OnDisable()
        {
            pressSettings?.Disable();
            holdSettings?.Disable();
            releaseSettings?.Disable();

            SetSelectedInteraction(null);
        }


        public void Update()
        {
            InteractionModule interactionModule = GetInteractionModuleInstance(GetCollider(interactionRange));

            if (CanInteract(interactionModule) && !IsSelectedInteraction(interactionModule)) {
                isEmptySelectedInteraction = false;
                SetSelectedInteraction(interactionModule);
            }
            else if(!isEmptySelectedInteraction) {
                isEmptySelectedInteraction = true;
                SetSelectedInteraction(null);
            }
        }

        public bool CanInteract(InteractionModule module)
        {
            if (module == null || allowedInteractionTypes == null) return false;
            return allowedInteractionTypes.Contains(module.Type);
        }

        private void SetSelectedInteraction(InteractionModule module)
        {
            onUnselected.Invoke(selectedInteraction);
            if(module != null) onSelected?.Invoke(module);
        }

        private bool IsSelectedInteraction(InteractionModule module)
        {
            if(selectedInteraction == null || module == null) return false;
            return selectedInteraction.Equals(module);
        }

        public void AddOnSelectedListener(UnityAction<InteractionModule> action) {
            if (action != null) onSelected?.AddListener(action);    
        }

        public void RemoveOnSelectedListener(UnityAction<InteractionModule> action) {
            if (action != null) onSelected?.RemoveListener(action);
        }

        public void AddOnUnselectedListener(UnityAction<InteractionModule> action)
        {
            if (action != null) onUnselected?.AddListener(action);
        }

        public void RemoveOnUnselectedListener(UnityAction<InteractionModule> action)
        {
            if (action != null) onUnselected?.RemoveListener(action);
        }


        protected abstract Collider GetCollider(float interactionRange);

        public static void AddOnInteractionModuleAddedListener(UnityAction<InteractionModule> action) {
            if(action != null) onInteractionModuleAdded?.AddListener(action);
        }

        public static void RemoveOnInteractionModuleAddedListener(UnityAction<InteractionModule> action) {
            if (action != null) onInteractionModuleAdded?.RemoveListener(action);
        }


        public static void AddOnInteractionModuleRemovedListener(UnityAction<InteractionModule> action)
        {
            if (action != null) onInteractionModuleRemoved?.AddListener(action);
        }

        public static void RemoveOnInteractionModuleRemovedListener(UnityAction<InteractionModule> action)
        {
            if (action != null) onInteractionModuleRemoved?.RemoveListener(action);
        }

        public static InteractionModule GetInteractionModuleInstance(Collider collider)
        {
            if (interactionModuleInstances == null || collider == null) return null;
            if (interactionModuleInstances.ContainsKey(collider)) return interactionModuleInstances[collider];  
            return null;
        }

        public static void AddInteractionModuleInstance(Collider collider, InteractionModule module)
        {
            if(collider != null && module != null && interactionModuleInstances != null)
            {
                if (!interactionModuleInstances.ContainsKey(collider)) {
                    interactionModuleInstances.Add(collider, module);
                    onInteractionModuleAdded?.Invoke(module);
                }
            }
        }

        public static void RemoveInteractionModuleInstance(Collider collider)
        {
            if (collider != null && interactionModuleInstances != null)
            {
                if (interactionModuleInstances.ContainsKey(collider)) {
                    onInteractionModuleRemoved?.Invoke(interactionModuleInstances[collider]);
                    interactionModuleInstances.Remove(collider);
                }
            }
        }

        public bool IsPressed()
        {
            return pressSettings == null ? false : pressSettings.GetValue();
        }
        public bool IsHeld()
        {
            return holdSettings == null ? false : holdSettings.GetValue();
        }
        public bool IsReleased()
        {
            return releaseSettings == null ? false : releaseSettings.GetValue();
        }

        public static bool IsCurrent(InteractionHandler handler)
        {
            if(Current == null) return false;
            return Current.Equals(handler);
        }

        public static void SetCurrent(int index)
        {
            SetCurrent(Get(index));
        }
        public static void SetCurrent(string name)
        {
            SetCurrent(Get(name));
        }
        public static void SetCurrent(Transform transform)
        {
            SetCurrent(Get(transform));
        }

        public static void SetCurrent(InteractionHandler handler) {
            Disable();
            Current = handler;
            Enable();
        }

        public static void Enable() {
            SetEnabledState(true);
        }
        public static void Disable()
        {
            SetEnabledState(false);
        }

        private static void SetEnabledState(bool isEnabled)
        {
            if (Current != null) Current.enabled = isEnabled;
        }

        public static InteractionHandler Get(int index)
        {
            if(Instances == null || Instances.Count <= 0) return null;
            return Instances[index];
        }
        public static InteractionHandler Get(string name)
        {
            if (Instances == null || Instances.Count <= 0 || string.IsNullOrEmpty(name)) return null;
            return Instances.Where(x => x != null)
                            .Where(x => !string.IsNullOrEmpty(x.handlerName))
                            .Where(x => x.handlerName.ToLower().Equals(name.ToLower()))
                            .FirstOrDefault();
        }
        public static InteractionHandler Get(Transform transform) {
            if (Instances == null || Instances.Count == 0) return null;
            return Instances.Where(x => x != null).Where(x => x.transform.Equals(transform)).FirstOrDefault();
        }

        public static InteractionHandler[] GetHandlers()
        {
            if (Instances == null || Instances.Count == 0) return null;
            return Instances.Where(x => x != null).ToArray();
        }
    }
}
