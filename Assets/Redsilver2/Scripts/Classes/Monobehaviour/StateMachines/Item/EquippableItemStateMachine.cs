using RedSilver2.Framework.Animations;
using RedSilver2.Framework.Items;
using RedSilver2.Framework.StateMachines.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(EquippableItem))]
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

        [Space]
        [SerializeField] private RuntimeAnimatorController animatorController;

        [Space]
        [SerializeField] private AnimationData equippedAnimation;

        [Space]
        [SerializeField] private AnimationData unequippedAnimation;

        [Space]
        [SerializeField] private AnimationData droppedAnimation;

        [Space]
        [SerializeField] private AnimationData defaultStateData;

        private float stateChangeCooldown;
        private bool canPerformActions;

        private EquippableItem item;


        private EquippableItemState currentState;
        private Animator animator; 

        private UnityEvent<EquippableItemState> onStateAdded,   onStateRemoved;
        private UnityEvent<EquippableItemState> onStateEntered, onStateExited;
        private UnityEvent<Vector3> onGroundTouched;

        private IEnumerator dropCoroutine;

        private ItemType type;
        public  ItemType Type => type;
        public Animator Animator => animator;

        public AnimationData EquippedStateData   => equippedAnimation;
        public AnimationData UnequippedStateData => unequippedAnimation;
        public AnimationData DroppedStateData    => droppedAnimation;
        public AnimationData DefaultStateData => defaultStateData;

        private readonly static Dictionary<EquippableItem, EquippableItemStateMachine> instances = new Dictionary<EquippableItem, EquippableItemStateMachine>();

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            dropCheckRange = Mathf.Clamp(dropCheckRange, 0.1f, float.MaxValue);
            dropFallSpeed = Mathf.Clamp(dropFallSpeed, 0f, float.MaxValue);

            ValidateAnimations(animatorController);
        }

        protected virtual void ValidateAnimations(RuntimeAnimatorController controller) {
            equippedAnimation?.Validate(controller);
            unequippedAnimation?.Validate(controller);
            
            droppedAnimation?.Validate(controller);
            defaultStateData?.Validate(controller);
        }

#endif

        protected override void Awake() {
            base.Awake();
            canPerformActions = false;

            animator = GetComponent<Animator>();
            item = GetComponent<EquippableItem>();

            onStateAdded = new UnityEvent<EquippableItemState>();
            onStateExited = new UnityEvent<EquippableItemState>();

            onStateEntered = new UnityEvent<EquippableItemState>();
            onStateExited = new UnityEvent<EquippableItemState>();

            onGroundTouched = new UnityEvent<Vector3>();

            equippedAnimation.AddOnStartedListener(() => { enabled = true; });
            equippedAnimation.AddOnFinishedListener(() => { 

                canPerformActions = true;
                animator?.PlayAnimation(defaultStateData);
            });

            unequippedAnimation?.AddOnStartedListener(() => { canPerformActions = false; });
            unequippedAnimation?.AddOnFinishedListener(() => { enabled = false; });

            AddOnGroundTouchedListener(OnGroundTouched);


            if (instances != null && item != null) {
                if (!instances.ContainsKey(item)) { instances?.Add(item, this); }
            }


            if (animator != null) {
                animator.runtimeAnimatorController = animatorController;
            }

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

        protected virtual void OnItemEquipped()
        {
            if (animator != null) animator.enabled = true;
            StopDropCoroutine();

            item?.SetIsInteractable(false);
            item?.SetMeshRenderersVisibility(true);

            animator?.PlayAnimation(equippedAnimation);
        }

        protected virtual void OnItemUnEquipped() {
            if (animator != null) animator.enabled = true;
            StopDropCoroutine();



            item?.SetIsInteractable(false);
            item?.SetMeshRenderersVisibility(true);
            animator?.PlayAnimation(unequippedAnimation);
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
            item?.RemoveFromInventory();
            item?.SetMeshRenderersVisibility(true);
            StartDropCoroutine();
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
            if (animator != null) animator.enabled = false;
            base.OnDisabled();
        }

        protected sealed override void OnEnabled() {
            if (animator != null) animator.enabled = true;
            base.OnEnabled();
        }

        protected virtual void OnGroundTouched(Vector3 position)
        {
            transform.position = position + Vector3.up * dropPositionYOffset;
            Debug.DrawRay(transform.position, Vector3.down, Color.green, 5f);
        }

        public override void ChangeState(State state)
        {
            if (IsEquipped()) base.ChangeState(state);
        }

        protected sealed override void OnStateEntered(UpdatableState state)
        {
            base.OnStateEntered(state);
            OnStateEntered(state as EquippableItemState);
        }
        protected sealed override void OnStateExited(UpdatableState state)
        {
            base.OnStateExited(state);
            OnStateExited(state as  EquippableItemState);
        }

        protected sealed override void OnStateAdded(UpdatableState state)
        {
            base.OnStateAdded(state);
            OnStateAdded(state as EquippableItemState);
        }
        protected sealed override void OnStateRemoved(UpdatableState state)
        {
            base.OnStateRemoved(state);
            OnStateRemoved(state as EquippableItemState);
        }

        protected virtual void OnStateAdded(EquippableItemState state)
        {
            onStateAdded?.Invoke(state);    
        }
        protected virtual void OnStateEntered(EquippableItemState state)
        {
            currentState = state;
            stateChangeCooldown = 0f;
            onStateEntered?.Invoke(state);
        }

        protected virtual void OnStateExited(EquippableItemState state)
        {
            onStateExited?.Invoke(state);
        }
        protected virtual void OnStateRemoved(EquippableItemState state)
        {
            onStateRemoved?.Invoke(state);
        }

        public void AddOnStateAddedListener(UnityAction<EquippableItemState> action)
        {
            if (action != null) onStateAdded?.AddListener(action);
        }
        public void RemoveOnStateAddedListener(UnityAction<EquippableItemState> action)
        {
            if (action != null) onStateAdded?.RemoveListener(action);
        }

        public void AddOnStateRemovedListener(UnityAction<EquippableItemState> action)
        {
            if (action != null) onStateRemoved?.AddListener(action);
        }
        public void RemoveOnStateRemovedListener(UnityAction<EquippableItemState> action)
        {
            if (action != null) onStateRemoved?.RemoveListener(action);
        }

        public void AddOnStateEnteredListener(UnityAction<EquippableItemState> action)
        {
            if (action != null) onStateEntered?.AddListener(action);
        }
        public void RemoveOnStateEnteredListener(UnityAction<EquippableItemState> action)
        {
            if (action != null) onStateEntered?.RemoveListener(action);
        }

        public void AddOnStateExitedListener(UnityAction<EquippableItemState> action)
        {
            if (action != null) onStateExited?.AddListener(action);
        }
        public void RemoveOnStateExitedListener(UnityAction<EquippableItemState> action)
        {
            if (action != null) onStateExited?.RemoveListener(action);
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

        public static EquippableItemStateMachine GetStateMachine(EquippableItem item)
        {
            if(item == null || instances == null || !instances.ContainsKey(item)) return null;
            return instances[item];
        }
    }
}
