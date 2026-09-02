using RedSilver2.Framework.Animations;
using RedSilver2.Framework.Items;
using RedSilver2.Framework.StateMachines.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR;

namespace RedSilver2.Framework.StateMachines
{
    [RequireComponent(typeof(EquippableItem))]
    [RequireComponent(typeof(EquippableItemAnimationController))]
    public abstract class EquippableItemStateMachine : UpdatableStateMachine
    {
        [Space]
        [SerializeField] private Transform itemParent;

        [Space]
        [SerializeField] private Vector3 parentRotation;
        [SerializeField] private Vector3 parentPosition;

        [Space]
        [SerializeField] private Vector3 dropRotation;

        [Space]
        [SerializeField] private float dropPositionYOffset;

        [Space]
        [SerializeField] private float dropCheckRange;
        [SerializeField] private float dropFallSpeed;

        private float stateChangeCooldown;
        private bool canPerformActions;

        private EquippableItem item;


        private EquippableItemState currentState;
        private EquippableItemAnimationController controller;
        private UnityEvent<Vector3> onGroundTouched;

        private IEnumerator dropCoroutine;

        private ItemType type;
        public  ItemType Type => type;

        public EquippableItemAnimationController Controller => controller;

        public const string EQUIP_ANIMATION_NAME = "Equip";
        public const string UNEQUIP_ANIMATION_NAME = "UnEquip";

        public const string DROP_ANIMATION_NAME = "Drop";
        private readonly static Dictionary<EquippableItem, EquippableItemStateMachine> instances = new Dictionary<EquippableItem, EquippableItemStateMachine>();

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            dropCheckRange = Mathf.Clamp(dropCheckRange, 0.1f, float.MaxValue);
            dropFallSpeed = Mathf.Clamp(dropFallSpeed, 0f, float.MaxValue);
        }

        protected override void DisplayDefaultSettings(Color foldoutColor, Color fieldColor) {
            base.DisplayDefaultSettings(foldoutColor, fieldColor);

        }
#endif

        protected override void Awake() {
            base.Awake();
            SetAnimationController(GetComponent<EquippableItemAnimationController>());
        
            canPerformActions = false;
            item = GetComponent<EquippableItem>();

            onGroundTouched = new UnityEvent<Vector3>();

            controller?.GetEquipData()?.AddOnStartedListener(() => { enabled = true; });
            controller?.GetEquipData()?.AddOnFinishedListener(() => {
                ChangeState(null as State);
                canPerformActions = true;
                stateChangeCooldown = 0f;

            });

            controller?.GetUnEquipData()?.AddOnStartedListener(() => { canPerformActions = false; });
            controller?.GetUnEquipData()?.AddOnFinishedListener(() => { enabled = false; });

            controller?.GetDropData()?.AddOnFinishedListener(() =>
            {
                item?.RemoveFromInventory();
                item?.SetMeshRenderersVisibility(true);
                StartDropCoroutine();
            });

            AddOnGroundTouchedListener(OnGroundTouched);

            if (instances != null && item != null)
                if (!instances.ContainsKey(item)) { instances?.Add(item, this); }

            StartDropCoroutine();
        }

        protected virtual void Start() {
            item?.AddOnEquippedListener(OnItemEquipped);
            item?.AddOnUnEquippedListener(OnItemUnEquipped);

            item?.AddOnAddedListener(OnItemAdded);

            item?.AddOnRemovedListener(OnItemRemoved);
            item?.AddOnDroppedListener(OnItemDropped);

            enabled = false;
        }

        private void OnDestroy()
        {
            if (instances == null || item == null) return;
            else if (instances.ContainsKey(item)) {
                instances?.Remove(item);
            }
        }

    
        protected void SetAnimationController(EquippableItemAnimationController controller) {
            this.controller = controller;
        }

        protected virtual void OnItemEquipped()
        {
            StopDropCoroutine();

            item?.SetIsInteractable(false);
            item?.SetMeshRenderersVisibility(true);

            controller?.PlayEquipData();
        }

        protected virtual void OnItemUnEquipped() {
            StopDropCoroutine();

            item?.SetIsInteractable(false);
            item?.SetMeshRenderersVisibility(true);

            controller?.PlayUnEquipData();
        }

        protected virtual void OnItemAdded() {
            enabled = true;
            StopDropCoroutine();

            if (item != null) {
                if (!item.IsEquipped) {
                    item?.SetIsInteractable(false);
                    item?.SetMeshRenderersVisibility(false);
                }
            }
        }

        protected virtual void OnItemRemoved() {
            enabled = false;
            item?.SetIsInteractable(true);
            item?.SetMeshRenderersVisibility(true);
        }

        protected virtual void OnItemDropped()
        {
            controller?.PlayDropData();
        }

        protected sealed override bool CanAddState(UpdatableState state) {
            return base.CanAddState(state) && CanAddState(state as EquippableItemState);
        }

        protected virtual bool CanAddState(EquippableItemState state) {
            return state != null ? true : false;
        }
        protected override void OnUpdate() {
            base.OnUpdate();
            OnUpdate(item);
        }

        protected override void OnLateUpdate()
        {
            base.OnLateUpdate();
            
            if(itemParent != null) {
                itemParent.localPosition = Vector3.Lerp(itemParent.localPosition, parentPosition, Time.deltaTime);
                itemParent.localRotation = Quaternion.Slerp(itemParent.localRotation, Quaternion.Euler(parentRotation), Time.deltaTime);
            }
        }

        protected virtual void OnUpdate(EquippableItem item)
        {
            if (currentState != null && item != null) {
                if (item.IsEquipped && stateChangeCooldown < currentState.Cooldown) {
                    stateChangeCooldown = Mathf.Clamp(stateChangeCooldown + Time.deltaTime, 0f, currentState.Cooldown);
                }
            }
        }

        protected sealed override void OnDisabled() {
            if (controller != null) controller.enabled = false;
            base.OnDisabled();
        }

        protected sealed override void OnEnabled() {
            if (controller != null) controller.enabled = true;
            base.OnEnabled();
        }

        protected virtual void OnGroundTouched(Vector3 position)
        {
            transform.position = position + Vector3.up * dropPositionYOffset;
            Debug.DrawRay(transform.position, Vector3.down, Color.green, 5f);
        }

        public sealed override void ChangeState(State state)
        {
            if (IsEquipped()) base.ChangeState(state);
        }

        protected override void OnStateEntered(State state)
        {
            base.OnStateEntered(state);
            currentState = state as EquippableItemState;
            stateChangeCooldown = 0f;
        }

        public void AddOnGroundTouchedListener(UnityAction<Vector3> action)
        {
            if (action != null) onGroundTouched?.AddListener(action);
        }
        public void RemoveOnGroundTouchedListener(UnityAction<Vector3> action)
        {
            if (action != null) onGroundTouched?.RemoveListener(action);
        }

        public bool IsEquipped()
        {
            if(item == null) return false;
            return item.IsEquipped;
        }

        public bool IsCooldownOver() {
            if (!canPerformActions) return false;
            else if (currentState == null) return true;

            return stateChangeCooldown >= currentState.Cooldown; 
        }

        private void UpdateDrop()
        {
            transform.SetParent(null);
            if(itemParent != null) itemParent.localRotation = Quaternion.Slerp(itemParent.localRotation, Quaternion.Euler(dropRotation), Time.deltaTime * 10f);
            transform.localPosition += Time.deltaTime * (Vector3.down * dropFallSpeed);
        }

        protected virtual IEnumerator DropCoroutine()
        {
            while (true) {
                bool isHittingGround = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, dropCheckRange);
                UpdateDrop();

                if (isHittingGround) {
                    onGroundTouched?.Invoke(hit.point);
                    break;
                }
               
                Debug.DrawRay(transform.position, Vector3.down, Color.red);
                yield return null;
            }
        }


        protected void StartDropCoroutine()
        {
            StopDropCoroutine();

            dropCoroutine = DropCoroutine();
            StartCoroutine(dropCoroutine);
        }

        protected void StopDropCoroutine()
        {
            if (dropCoroutine != null) StopCoroutine(dropCoroutine);
            dropCoroutine = null;
        }

        public EquippableItemAnimationController GetEquippableItemAnimationController() {
            return controller;
        }

        public static EquippableItemStateMachine GetStateMachine(EquippableItem item)
        {
            if(item == null || instances == null || !instances.ContainsKey(item)) return null;
            return instances[item];
        }
    }
}
