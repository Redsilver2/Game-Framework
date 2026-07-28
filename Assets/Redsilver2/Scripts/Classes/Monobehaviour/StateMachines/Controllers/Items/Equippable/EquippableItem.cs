using RedSilver2.Framework.Animations;
using RedSilver2.Framework.Inventories;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Items
{
    [RequireComponent(typeof(Animator))]
    public abstract class EquippableItem : Item {

        [Space]
        [SerializeField] private RuntimeAnimatorController animatorController;

        [Space]
        [SerializeField] private AnimationData equippedAnimation;

        [Space]
        [SerializeField] private AnimationData unequippedAnimation;

        [Space]
        [SerializeField] private AnimationData droppedAnimation;

        [Space]
        [SerializeField] private Vector3 parentRotation;
        [SerializeField] private Vector3 parentPosition;


        private float animationTime;


        private bool isEquipped;
        private bool isInputActionsActivated;

        private Animator                   animator;
        private UnityEvent                 onEquipped, onUnEquipped, onUpdate;

        private IEnumerator updateCoroutine;
        public bool IsEquipped => isEquipped;
        public float AnimationTime => animationTime;

        public Vector3    OriginalParentPosition => parentPosition;
        public Quaternion OriginalParentRotation => Quaternion.Euler(parentRotation);
      


        #if UNITY_EDITOR
        protected override void OnValidate()
        {
            ValidateAnimationDatas(animatorController);
        }

        protected virtual void ValidateAnimationDatas(RuntimeAnimatorController animatorController)
        {
            equippedAnimation?.Validate(animatorController);
            unequippedAnimation?.Validate(animatorController);
            droppedAnimation?.Validate(animatorController);
        }

        #endif


        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();

            if (animator) {
                animator.enabled = false;
                animator.runtimeAnimatorController = animatorController;
            }

            isEquipped              = false;
            isInputActionsActivated = false;
           
            onEquipped              = new UnityEvent();  
            onUnEquipped            = new UnityEvent();     
           
            onUpdate                = new UnityEvent();
            updateCoroutine         = UpdateCoroutine();

            AddOnEquippedListener(OnEquipped);  
            AddOnUnEquippedListener(OnUnEquipped);
           
            AddOnUpdateListener(OnUpdate);
            AddOnAddedListener(() =>
            {
                transform.localPosition = parentPosition;
                transform.localRotation = Quaternion.Euler(parentRotation);
            });

            equippedAnimation?.AddOnStartedListener(()   =>  {
                StartCoroutine(updateCoroutine);
                SetMeshRenderersVisibility(true);
                Debug.Log("?!");
            });

            unequippedAnimation?.AddOnFinishedListener(() => {
                StopCoroutine(updateCoroutine);
                SetMeshRenderersVisibility(false);
            });
        }

        public void Equip()
        {
            if (!isEquipped) onEquipped?.Invoke();
        }

        public void UnEquip()
        {
            if (isEquipped) onUnEquipped?.Invoke();
        }

        protected override IEnumerator DropCoroutine() {
            float t = 0f;     
            if(droppedAnimation != null) PlayAnimation(droppedAnimation.AnimationName, droppedAnimation.CrossFadeTime);


            while (droppedAnimation != null)  {
                if (t >= droppedAnimation.CrossFadeTime) {
                    if (!AnimationManager.IsCurrentClipPlaying(animator, droppedAnimation.AnimationName)) break;
                }
                else { t += Time.deltaTime; }

                yield return null;
            }

            if (animator != null) animator.enabled = false;
            yield return StartCoroutine(base.DropCoroutine());
        }

        protected override void RemoveFromInventory(Inventory inventory)
        {
            isEquipped = false;
            isInputActionsActivated = false;

            StopCoroutine(updateCoroutine);
            base.RemoveFromInventory(inventory);
        }

        protected virtual void OnEquipped() {
            isEquipped = true;
            if (animator != null) animator.enabled = true;
            PlayAnimation(equippedAnimation);
        }

        protected virtual void OnUnEquipped()
        {
            isEquipped = false;
            if (animator != null) animator.enabled = true;
            PlayAnimation(unequippedAnimation);
        }

        protected virtual void OnUpdate() {

        }

        private IEnumerator UpdateCoroutine() {
            while (true) {
                onUpdate?.Invoke();
                yield return null;
            }
        }

        public void PlayAnimation(AnimationData data)
        {
            animator?.CrossFadeAnimation(data);
            if(data != null) SetPositionUpdaterTimer(data.AnimationName, data.CrossFadeTime);
        }


        public void PlayAnimation(string animationName, float crossFadeTime)
        {
            animator?.CrossFadeAnimation(animationName, crossFadeTime);
            SetPositionUpdaterTimer(animationName, crossFadeTime);
        }

        private void SetPositionUpdaterTimer(string animationName, float crossFadeTime) {
            float lenght = AnimationManager.GetClipLenght(animator, animationName);

            if (lenght > 0f) animationTime = lenght + crossFadeTime;
            else animationTime = 0f;
        }

        public void AddOnEquippedListener(UnityAction action)
        {
            if(action != null) onEquipped?.AddListener(action);
        }

        public void RemoveOnEquippedListener(UnityAction action)
        {
            if(action != null)  onEquipped?.RemoveListener(action);
        }


        public void AddOnUnEquippedListener(UnityAction action)
        {
            if (action != null) onUnEquipped?.AddListener(action);
        }

        public void RemoveOnUnEquippedListener(UnityAction action)
        {
            if (action != null)  onUnEquipped?.RemoveListener(action); 
        }

        public void AddOnUpdateListener(UnityAction action)
        {
            if (action != null) onUpdate?.AddListener(action);
        }

        public void RemoveOnUpdateListener(UnityAction action) {
            if (action != null) onUpdate?.RemoveListener(action);
        }

        public virtual void AddAction(EquippableItemAction action) {

        }

        public virtual void RemoveAction(EquippableItemAction action)
        {

        }

    }
}
