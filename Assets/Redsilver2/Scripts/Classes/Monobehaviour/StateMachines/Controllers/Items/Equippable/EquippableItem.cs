using RedSilver2.Framework.Animations;
using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Items;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Items
{
    public abstract class EquippableItem : Item {

        [Space]
        [SerializeField] private AnimationData equippedAnimationData;
        [SerializeField] private AnimationData unequippedAnimationData;

        [Space]
        [SerializeField] private Vector3 parentRotation;
        [SerializeField] private Vector3 parentPosition;

        private bool    isEquipped;

        private float   actionDelay;
        private bool    canResetActions;

        private float   animationLenght;

        private Vector3 originalRotation;

        private Animator                   animator;
        private UnityEvent                 onEquipped, onUnEquipped, onUpdated;
        private List<EquippableItemAction> actions;


        public bool  IsEquipped => isEquipped;
        public float AnimationLenght => animationLenght;
        public Animator Animator => animator;


        #if UNITY_EDITOR
        protected virtual void OnValidate()
        {
           // ValideAnimationDatas(equippedAnimationDatas);
            //ValideAnimationDatas(unequippedAnimationDatas);
        }

        protected void ValideAnimationDatas(AnimationData[] animationDatas)
        {
            if(animationDatas != null) {
                foreach (AnimationData animationData in animationDatas)
                    animationData?.Validate(animator);
            }
        }

        #endif


        protected override void Awake()
        {
            base.Awake();

            animator                      = GetComponentInChildren<Animator>();
            if(animator) animator.enabled = false;
           
            isEquipped       = false;
            originalRotation = transform.localEulerAngles;

            onEquipped       = new UnityEvent();
            onUnEquipped     = new UnityEvent();
            
            onUpdated         = new UnityEvent();
            actions          = new List<EquippableItemAction>();

            AddOnRemovedListener   (() => {
                if (animator != null) animator.enabled = true;
                PlayAnimation(unequippedAnimationData);
                isEquipped = false;
            });

            AddOnEquippedListener  (() => {
                if (animator != null) animator.enabled = true;
             
                SetMeshRenderersVisibility(true);     
                PlayAnimation(equippedAnimationData);

                foreach (var action in actions)
                    action?.EnableActions();

                enabled = true;
                isEquipped = true;
            });

            AddOnUnEquippedListener(() => {
                if (animator != null) animator.enabled = true;

                foreach (var action in actions)
                    action?.DisableActions();

                PlayAnimation(unequippedAnimationData);
                enabled = false;
                isEquipped = false;
            });

            AddOnUpdatedListener(() =>
            {
                actionDelay = Mathf.Clamp(actionDelay -= Time.deltaTime, 0f, float.MaxValue);

                if(actionDelay <= 0f) {
                    ResetActions();
                    UpdateActions();
                }
            });
        }

        private void Update()
        {
            onUpdated?.Invoke();
        }

        public void Equip()
        {
            if (!isEquipped) onEquipped?.Invoke();
        }

        public void UnEquip()
        {
            if (isEquipped) onUnEquipped?.Invoke();
        }

        public void Drop() {
            SetTransformParent(null);
        }

        private void UpdateActions()
        {
            if (actions == null) return;

            foreach (var action in actions.Where(x => x.CanUpdate()))
            {
                if (action == null) continue;
                action.UpdateActions(ref actionDelay, out bool isExecuted);

                Debug.Log(isExecuted);
                if (isExecuted) { canResetActions = true; break; }
            }
        }

        private void ResetActions()
        {
            if (canResetActions && actions != null)
            {
                foreach (var action in actions) action?.ResetActions();
                canResetActions = false;
            }
        }

        private IEnumerator DropCoroutine()
        {
            while (true) { 

                yield return null;
            }
        }

        public void PlayAnimation(AnimationData data)
        {
            if(data == null) return;
            AnimationClip clip = animator.GetAnimationClip(data.AnimationName);

            if(clip == null) return;
            animationLenght = clip.length + data.CrossFadeTime;

            animator?.CrossFadeAnimation(data);
        }



        public virtual void SetTransformParent(Transform parent) {
            transform.SetParent(parent);

            if (parent != null)
            {
                transform.localEulerAngles = parentRotation;
                transform.localPosition    = parentPosition;
            }
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

        public void AddOnUpdatedListener(UnityAction action)
        {
            if (action != null) onUpdated?.AddListener(action);
        }

        public void RemoveOnUpdatedListener(UnityAction action)
        {
            if (action != null) onUpdated?.RemoveListener(action);
        }

        public virtual void AddAction(EquippableItemAction action) {
            if(action != null && actions != null) {
                if (!actions.Contains(action))
                    actions?.Add(action);
            }
        }

        public virtual void RemoveAction(EquippableItemAction action)
        {
            if (action != null && actions != null) {
                if (actions.Contains(action)) 
                    actions?.Remove(action);
            }
        }

    }
}
