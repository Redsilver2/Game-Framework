using RedSilver2.Framework.Player.Inventories;
using RedSilver2.Framework.Player.Inventories.UI;
using RedSilver2.Framework.StateMachines.Controllers;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Interactions
{
    public abstract class InteractionHandlerModule : MonoBehaviour
    {
        [SerializeField] private float interactionRange;
        [SerializeField] private InteractionType[] allowedInteractionTypes;
        private PlayerController owner;
        private Inventory inventory;

        public float            InteractionRange => interactionRange;
        public Inventory        Inventory        => inventory;
        public PlayerController Owner            => owner;

        private void Awake()
        {
            owner = transform.root.GetComponent<PlayerController>();
            inventory = transform.root.GetComponentInChildren<Inventory>();
            SetInteractionHandler(GetInteractionHandler()); 
        }

        public bool CanInteract(InteractionModule module)
        {
            if (module == null || allowedInteractionTypes == null) return false;
            return allowedInteractionTypes.Contains(module.Type);
        }

        protected abstract void Update();
        protected abstract void SetInteractionHandler(InteractionHandler handler);
        protected abstract InteractionHandler GetInteractionHandler();     
    }
}
