using RedSilver2.Framework.StateMachines;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Animations
{
    public abstract class EquippableItemAnimationController : AnimationController {

#if UNITY_EDITOR
        protected override AnimationControllerInfo[] GetValidInfos(AnimationControllerInfo[] infos) {
            List<AnimationControllerInfo> results = base.GetValidInfos(infos).ToList();
            results?.Add(GetValidInfo(EquippableItemStateMachine.EQUIP_ANIMATION_NAME, infos));
            results?.Add(GetValidInfo(EquippableItemStateMachine.UNEQUIP_ANIMATION_NAME, infos));
            results?.Add(GetValidInfo(EquippableItemStateMachine.DROP_ANIMATION_NAME, infos));
            return results.ToArray();
        }
#endif

        protected override void Awake() {
            base.Awake();
        }

        protected override void Initialize(Animator animator) {
            base.Initialize(animator);
            List<int> registeredIndex = new List<int>();

            foreach(AnimationControllerInfo info in Infos) {
                int dataIndex      = info.AnimationDataIndex;
                AnimationData data = GetData(dataIndex);

                if (data != null && !registeredIndex.Contains(dataIndex)) {
                    data?.AddOnFinishedListener(OnFinished(animator, info.Name));
                    registeredIndex?.Add(dataIndex);
                }
            }
        }

        protected virtual UnityAction OnFinished(Animator animator, string name) {
            return () => {
                if (IsInvalidAnimationTransition(name)) return;
                PlayDefaultData();
            };
        }

        private bool IsInvalidAnimationTransition(string name)
        {
            if(string.IsNullOrEmpty(name)) return true;
            name = name.ToLower();

            return name == EquippableItemStateMachine.UNEQUIP_ANIMATION_NAME.ToLower() ||
                   name == EquippableItemStateMachine.DROP_ANIMATION_NAME.ToLower();
        }

        public void PlayEquipData() {
            Play(GetEquipData());  
        }

        public void PlayUnEquipData() {
            Play(GetUnEquipData()); 
        }

        public void PlayDropData() {
            Play(GetDropData());
        }

        public bool IsPlayingEquipData()
        {
            return IsPlayingData(EquippableItemStateMachine.EQUIP_ANIMATION_NAME);
        }

        public bool IsPlayingUnEquipData()
        {
            return IsPlayingData(EquippableItemStateMachine.UNEQUIP_ANIMATION_NAME);
        }


        public bool IsPlayingDropData()
        {
            return IsPlayingData(EquippableItemStateMachine.DROP_ANIMATION_NAME);
        }


        public AnimationData GetEquipData() { return GetData(EquippableItemStateMachine.EQUIP_ANIMATION_NAME); }
        public AnimationData GetUnEquipData() { return GetData(EquippableItemStateMachine.UNEQUIP_ANIMATION_NAME); }
        public AnimationData GetDropData() { return GetData(EquippableItemStateMachine.DROP_ANIMATION_NAME); }
    }
}
