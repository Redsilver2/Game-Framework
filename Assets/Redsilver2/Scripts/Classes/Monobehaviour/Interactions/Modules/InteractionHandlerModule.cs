using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Interactions
{
    public abstract class InteractionHandlerModule : MonoBehaviour
    {
        [SerializeField] private float interactionRange;

        [Space]
        [SerializeField] private PressInputSettings   pressInteractionSettings;
        [SerializeField] private HoldInputSettings    holdInteractionSettings;
        [SerializeField] private ReleaseInputSettings releaseInteractionSettings;

        [Space]
        [SerializeField] private InteractionType[] allowedInteractionTypes;
        private PlayerController owner;

        public float            InteractionRange => interactionRange;
        public PlayerController Owner            => owner;

        private void Awake()
        {
            owner = transform.root.GetComponent<PlayerController>();
            SetInteractionHandler(GetInteractionHandler());
        }

        private void Start()
        {
            pressInteractionSettings?.Enable();
            holdInteractionSettings?.Enable();
            releaseInteractionSettings?.Enable();   
        }

        public bool CanInteract(InteractionModule module)
        {
            if (module == null || allowedInteractionTypes == null) return false;
            return allowedInteractionTypes.Contains(module.Type);
        }

        protected abstract void Update();
        protected abstract void SetInteractionHandler(InteractionHandler handler);
        protected abstract InteractionHandler GetInteractionHandler();

        public bool IsPressed()
        {
            if (pressInteractionSettings == null) return false;
            return pressInteractionSettings.GetConfiguration().Value;
        }

        public bool IsHeld()
        {
            if (holdInteractionSettings == null) return false;
            return holdInteractionSettings.GetConfiguration().Value;
        }

        public bool IsReleased() {
            if (releaseInteractionSettings == null) return false;
            return releaseInteractionSettings.GetConfiguration().Value;
        }
    }
}
