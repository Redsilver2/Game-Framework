using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Inputs.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions
{

    [System.Serializable]
    public abstract partial class InteractionHandler
    {
        [SerializeField, HideInInspector] private bool  isEnabled;
        [SerializeField, HideInInspector] private float interactionRange;

        [SerializeField, HideInInspector] private PressInputSettings pressSettings;
        [SerializeField, HideInInspector] private HoldInputSettings    holdSettings;
        [SerializeField, HideInInspector] private ReleaseInputSettings releaseSettings;

        [SerializeField, HideInInspector] private List<InteractionType> interactionTypesAllowed;

        [SerializeField, HideInInspector] private UnityEvent onEnabled;
        [SerializeField, HideInInspector] private UnityEvent onDisabled;

        [SerializeField, HideInInspector] private UnityEvent<InteractionModule> onSelected;
        [SerializeField, HideInInspector] private UnityEvent<InteractionModule> onUnselected;

        private bool isEmptySelectedInteraction;
        private  InteractionModule selectedInteraction;

        public float InteractionRange => interactionRange;
        public bool IsEnabled => isEnabled;

        public bool IsSelectingNextInteraction     => InputManager.GetKeyDown(KeyboardKey.UpArrow);
        public bool IsSelectingPreviousInteraction => InputManager.GetKeyDown(KeyboardKey.DownArrow);

        public InteractionModule    SelectedInteraction => selectedInteraction;
        public PressInputSettings   PressSettings       => pressSettings;

        public HoldInputSettings    HoldSettings    => holdSettings;
        public ReleaseInputSettings ReleaseSettings => releaseSettings;
        

        protected static InteractionHandler Current {  get; private set; }
        private readonly static List<InteractionHandler> Instances = new List<InteractionHandler>();

        private readonly static Dictionary<Collider, InteractionModule> interactionModuleInstances = new Dictionary<Collider, InteractionModule>();
        private readonly static UnityEvent<InteractionModule>           onInteractionModuleAdded   = new UnityEvent<InteractionModule>();
        private readonly static UnityEvent<InteractionModule>           onInteractionModuleRemoved = new UnityEvent<InteractionModule>();

        protected InteractionHandler() {
            AddOnUnselectedListener(interaction => {
                interaction?.Unselect(this);
                this.selectedInteraction = null;
            });

            AddOnSelectedListener(interaction => {
                this.selectedInteraction = interaction;
                interaction?.Select(this);
            });

            this.isEmptySelectedInteraction = true;
            Instances?.Add(this);
        }


        public void Enable()
        {
            if (!isEnabled)
            {
                pressSettings?.Enable();
                holdSettings?.Enable();

                releaseSettings?.Enable();
                isEnabled = true;
            }
        }

        public void Disable()
        {
            if (isEnabled)
            {
                pressSettings?.Disable();
                holdSettings?.Disable();
             
                releaseSettings?.Disable();
                SetSelectedInteraction(null);

                onDisabled?.Invoke();
                isEnabled = false;
            }
        }


        public void Update()
        {
            if (!isEnabled) return;
            InteractionModule interactionModule = GetInteractionModuleInstance(GetCollider(interactionRange));

            if (CanInteract(interactionModule) && !IsSelectedInteraction(interactionModule)) {
                isEmptySelectedInteraction = false;
                SetSelectedInteraction(interactionModule);
            }
            else if(!isEmptySelectedInteraction && interactionModule == null) {
                isEmptySelectedInteraction = true;
                SetSelectedInteraction(null);
            }
        }

        public bool CanInteract(InteractionModule module)
        {
            if (module == null || !module.IsInteractable || interactionTypesAllowed == null) return false;
            return interactionTypesAllowed.Contains(module.Type);
        }

        private void SetSelectedInteraction(InteractionModule module)
        {
            onUnselected.Invoke(selectedInteraction);
            if(module != null) onSelected?.Invoke(module);
        }

        private bool IsSelectedInteraction(InteractionModule module)
        {
            if(selectedInteraction == null || module == null) return false;
            return selectedInteraction == module;
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

        public static void SetCurrent(InteractionHandler handler) {
            DisableCurrent();
            Current = handler;
            EnableCurrent();
        }

        public static void EnableCurrent() {
            SetCurrentState(true);
        }
        public static void DisableCurrent()
        {
            SetCurrentState(false);
        }

        private static void SetCurrentState(bool isEnabled)
        {
            if (isEnabled) Current?.Enable();
            else           Current?.Disable();
        }

        public static InteractionHandler Get(int index)
        {
            if(Instances == null || Instances.Count <= 0) return null;
            return Instances[index];
        }

        public static InteractionHandler[] GetHandlers()
        {
            if (Instances == null || Instances.Count == 0) return null;
            return Instances.Where(x => x != null).ToArray();
        }
    }

    public abstract partial class InteractionHandler
    {
#if UNITY_EDITOR
        [SerializeField, HideInInspector] private bool displayInteractionHandler;

        [SerializeField, HideInInspector] private bool displayBaseSettings;
        [SerializeField, HideInInspector] private bool showInteractionTypeAllowed;


        public virtual void DrawInspector(Color foldoutColor, Color backgroundColor, Color buttonColor)
        {
            if (EditorExtension.DisplayFoldout("Interaction Handler", ref displayInteractionHandler, foldoutColor))
            {
                EditorExtension.DrawVerticalHelpBox(() =>
                {
                    EditorExtension.IncrementIndent();

                    if (EditorExtension.DisplayFoldout("Base Settings", ref displayBaseSettings, foldoutColor))
                    {
                        EditorExtension.IncrementIndent();
                        EditorExtension.Space(10f);
                        DrawBaseSettings(foldoutColor, backgroundColor, buttonColor);
                        DrawInteractionTypesAllowed(foldoutColor);

                        EditorExtension.DecrementIndent();
                    }

                    EditorExtension.DecrementIndent();

                }, backgroundColor);
            }
        }

        private void DrawInteractionTypesAllowed(Color foldoutColor)
        {


            EditorExtension.Space(10f);

            if (interactionTypesAllowed == null) return;
            else if (EditorExtension.DisplayFoldout("Interactions Type Allowed", ref showInteractionTypeAllowed, foldoutColor))
            {
                EditorExtension.IncrementIndent();

                foreach (InteractionType interaction in Enum.GetValues(typeof(InteractionType)))
                {
                    EditorExtension.Space(10f);

                    EditorExtension.DrawHorizontal(() => {
                        EditorExtension.Space(10f);
                        EditorExtension.DisplayLabel(interaction.ToString());

                        if (interactionTypesAllowed.Contains(interaction)) EditorExtension.DisplayButton("Remove", () => { interactionTypesAllowed?.Remove(interaction); });
                        else EditorExtension.DisplayButton("Add", () => { interactionTypesAllowed?.Add(interaction); });
                    });
                }

                EditorExtension.Space(10f);
                EditorExtension.DecrementIndent();
            }

        }

        protected virtual void DrawBaseSettings(Color foldoutColor, Color backgroundColor, Color buttonColor)
        {
            isEnabled        = EditorExtension.DisplayToggle("Is Enabled ", isEnabled);


            EditorExtension.Space(10f);
            interactionRange = EditorExtension.DisplayFloatSlider("Interaction Range ", interactionRange, 0f, 100f);

            EditorExtension.Space(10f);
            pressSettings = EditorExtension.DisplayCustomField("Press Settings", false, pressSettings);
          
            holdSettings  = EditorExtension.DisplayCustomField("Hold Settings", false, holdSettings);
            releaseSettings = EditorExtension.DisplayCustomField("Release Settings", false, releaseSettings);
        }
#endif
    }
}
