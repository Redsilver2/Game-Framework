using RedSilver2.Framework.Interactions;
using RedSilver2.Framework.Inventories;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Items
{
    public abstract class Item : InteractionModule
    {

        [Space]
        [SerializeField] private Vector3 dropRotation;

        [Space]
        [SerializeField] private float dropPositionYOffset;

        [Space]
        [SerializeField] private float dropCheckRange;
        [SerializeField] private float dropFallSpeed;

        private MeshRenderer[] renderers;
        private UnityEvent     onAdded, onRemoved, onDisabled, onEnabled;

        private IEnumerator dropCoroutine;
        private Inventory   owner;

        private ItemType type;
        public  ItemType ItemType => type;



#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            dropCheckRange = Mathf.Clamp(dropCheckRange, 0.1f, float.MaxValue);
            dropFallSpeed  = Mathf.Clamp(dropFallSpeed, 0f, float.MaxValue);
        }
#endif

        protected override void Awake() 
        {
            base.Awake();
            SetInteractionType(InteractionType.Item);

            onAdded       = new UnityEvent();
            onRemoved     = new UnityEvent();
            
            onDisabled    = new UnityEvent();
            onEnabled     = new UnityEvent();

            type          = GetItemType();
            renderers     = transform.GetComponentsInChildren<MeshRenderer>();

            AddOnAddedListener  (() => {
                StopDropCoroutine();
                SetIsInteractable(false);

                SetInteractionColliderVisibility(false);
            });
            AddOnRemovedListener(() => {
                Debug.Log("Sure");
                StartDropCoroutine();
                SetIsInteractable(true);

                SetInteractionColliderVisibility(true);
            });
        }

        protected override void OnEnable() {
            base.OnEnable();
            owner?.AddOnItemRemovedListner(OnRemoved);

            onEnabled?.Invoke();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            owner?.RemoveOnItemRemovedListner(OnRemoved);

            onDisabled?.Invoke();
        }

        public virtual void Take(Inventory inventory)
        {
            if (owner != inventory) RemoveFromInventory();
            inventory?.AddItem(this);

            if (inventory != null) {
                if (inventory.ContainsItem(this)) {
                    owner = inventory;
                    SetIsInteractable(false);

                    onAdded?.Invoke();
                    owner?.AddOnItemRemovedListner(OnRemoved);
                }              
            }
        }

        public virtual void RemoveFromInventory() {
            if (owner != null) {
                Inventory current = owner;
                owner = null;

                RemoveFromInventory(current);

            }
        }

        protected virtual void RemoveFromInventory(Inventory inventory) {
            if(inventory != null){
                SetIsInteractable(true);
                inventory?.RemoveItem(this);

                if (!inventory.ContainsItem(this)) onRemoved?.Invoke();
            }
        }

        protected virtual void OnRemoved(Item item)
        {
            owner?.RemoveOnItemRemovedListner(OnRemoved);
        }

        protected void SetInteractionColliderVisibility(bool isVisible) {
            Collider collider = Collider;
            if(collider != null) collider.enabled = isVisible;
        }

        public void SetMeshRenderersVisibility(bool isVisible) {
            foreach (MeshRenderer renderer in renderers.Where(x => x != null))
                renderer.enabled = isVisible;
        }

        public bool IsInInventory()
        {
            return owner == null ? false : owner.ContainsItem(this);
        }

        public MeshRenderer GetMeshRenderer(string name)
        {
            var results = renderers.Where(x => x != null).Where(x => x.name.ToLower().Equals(name.ToLower()));
            return results.Count() > 0 ? results.First() : null;
        }

        protected virtual IEnumerator DropCoroutine() {
            Debug.Log("1.");

            while (true) {
                bool isHittingGround = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, dropCheckRange);
                Debug.Log("?? " + hit.collider);
                if (owner != null) break;

                Debug.Log("What??");
                transform.SetParent(null);

                transform.localRotation = Quaternion.Euler(dropRotation);
                transform.localPosition += Time.deltaTime * (Vector3.down * dropFallSpeed);
              
                if (isHittingGround) {
                    transform.position = hit.point + Vector3.up * dropPositionYOffset;
                    Debug.DrawRay(transform.position, Vector3.down, Color.green);
                    break;
                }
                else Debug.DrawRay(transform.position, Vector3.down, Color.red);

                yield return null;
            }

            Debug.Log("2.");
        }

        protected void StartDropCoroutine()
        {
            StopDropCoroutine();

            dropCoroutine = DropCoroutine();
            StartCoroutine(dropCoroutine);
        }

        protected void StopDropCoroutine()
        {
            if(dropCoroutine != null) StopCoroutine(dropCoroutine);
            dropCoroutine = null; 
        }


        public void AddOnAddedListener(UnityAction action) {
            if (action != null) onAdded?.AddListener(action);
        }

        public void RemoveOnAddedListener(UnityAction action)
        {
            if (action != null) onAdded?.RemoveListener(action);
        }

        public void AddOnRemovedListener(UnityAction action) {
            if(action != null) onRemoved?.AddListener(action);
        }

        public void RemoveOnRemovedListener(UnityAction action)
        {
            if (action != null) onRemoved?.RemoveListener(action);
        }

        public void AddOnDisabledListener(UnityAction action)
        {
            if (action != null) onDisabled?.AddListener(action);
        }

        public void RemoveOnDisabledListener(UnityAction action)
        {
            if (action != null) onDisabled?.RemoveListener(action);
        }

        public void AddOnEnabledListener(UnityAction action)
        {
            if (action != null) onEnabled?.AddListener(action);
        }

        public void RemoveOnEnabledListener(UnityAction action)
        {
            if (action != null) onEnabled?.RemoveListener(action);
        }

        protected abstract ItemType GetItemType();
    }
}
