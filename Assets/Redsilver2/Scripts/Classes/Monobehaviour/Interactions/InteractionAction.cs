using UnityEngine;

namespace RedSilver2.Framework.Interactions.Actions
{


    public abstract class InteractionAction : MonoBehaviour {
        [SerializeField] private InteractionActionType type;

        [Space]
        [SerializeField] private string interactionName;
        [SerializeField][TextArea(3, 3)] private string interactionDescription;

        [Space]
        [SerializeField] private Sprite interactionIcon;

        [Space]
        [SerializeField] private float maxInteractionTime;
        [SerializeField] private float maxInteractionResetTime;
        [SerializeField] private float interactionResetSpeed;

        [Space]
        [SerializeField] private bool canResetInteractionTime;

        private Interaction interaction;

        protected InteractionActionType Type {
            get { return type; }
            set { type = value; }
        }

#if UNITY_EDITOR
        protected virtual void OnValidate() {
            if (type != InteractionActionType.Hold) {
                maxInteractionTime = 0f;
                maxInteractionResetTime = 0f;
                interactionResetSpeed = 0f;
            }
            else {
                maxInteractionTime = Mathf.Clamp(maxInteractionTime, 0f, float.MaxValue);
                maxInteractionResetTime = Mathf.Clamp(maxInteractionResetTime, 0f, float.MaxValue);
                interactionResetSpeed = Mathf.Clamp(interactionResetSpeed, 0f, float.MaxValue);
            }
        }
#endif
        protected virtual void Awake() {
            interaction = GetInteraction();
            SetInteractionModule(GetComponent<InteractionModule>());
        }

        private void Start()
        {
            SetInteractionEvent(interaction, true);
        }

        private void OnDisable() {
            interaction?.Disable();
            SetInteractionEvent(interaction, false);
        }

        private void OnEnable() {
            interaction?.Enable();
            SetInteractionEvent(interaction, true);
        }

        public void UpdateAction(InteractionHandler handler) {
            UpdateAction(interaction as HoldInteraction);
            interaction?.SetIcon(interactionIcon);
            interaction?.Interact(handler);
        }

        private void UpdateAction(HoldInteraction interaction) {
            interaction?.SetMaxInteractionTime(maxInteractionTime);
            interaction?.SetCanResetInteractionTime(canResetInteractionTime);

            interaction?.SetMaxInteractionResetTime(maxInteractionResetTime);
            interaction?.SetInteractionResetSpeed(interactionResetSpeed);
        }

        protected abstract void SetInteractionModule(InteractionModule module);
        protected abstract void SetInteractionEvent(Interaction interaction, bool isAddingEvent);

        private Interaction GetInteraction() {
            if (type == InteractionActionType.Press) return new PressInteraction(interactionName);
            else if (type == InteractionActionType.Released) return new ReleaseInteraction(interactionName, interactionDescription);
            else return new HoldInteraction(interactionName, interactionDescription);
        }


    }
}