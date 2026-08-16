using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Animations
{
    [RequireComponent(typeof(Animator))]
    public class AnimationController : MonoBehaviour {
        [SerializeField] private RuntimeAnimatorController controller;

        [Space]
        [SerializeField] private AnimationControllerInfo[] infos;

        [Space]
        [SerializeField] private AnimationData[] datas;

        private Animator animator;

        public AnimationControllerInfo[] Infos => infos;
        public AnimationData[] Datas => datas;

        public const string DEFAULT_ANIMATION_NAME    = "Default";

#if UNITY_EDITOR
        protected virtual void OnValidate() {
            ValidateDatas(controller, ref datas);
            ValidateInfos(ref infos);
        }

        private void ValidateInfos(ref AnimationControllerInfo[] infos)
        {
            if(datas == null) {
                if(infos == null || infos.Length != 0) infos = new AnimationControllerInfo[0];
                return;
            }
            else if(datas == null) { infos = new AnimationControllerInfo[0]; }

            infos = GetValidInfos(infos);

            for(int i = 0; i < infos.Length; i++)
                infos[i].Validate(datas);
        }

        private void ValidateDatas(RuntimeAnimatorController controller, ref AnimationData[] datas) {
            if(controller == null) {
                if(datas == null || datas.Length != 0) datas = new AnimationData[0];
                return;
            }
            else if(datas == null) { datas = new AnimationData[0]; }

            datas = GetValidDatas(controller.GetClips(), datas);

            foreach (AnimationData data in datas)
                data?.Validate(controller);
        }

        protected virtual AnimationControllerInfo[] GetValidInfos(AnimationControllerInfo[] infos)
        {
            List<AnimationControllerInfo> results = new List<AnimationControllerInfo>();
            results?.Add(GetValidInfo(DEFAULT_ANIMATION_NAME, infos));
            return results.ToArray();
        }

        protected AnimationControllerInfo GetValidInfo(string infoName, AnimationControllerInfo[] infos) {
            if (infos == null || string.IsNullOrEmpty(infoName))
                return default;

            infoName = infoName.ToLower();

            foreach(AnimationControllerInfo info in infos)
                if(info.Name.ToLower() == infoName) return info;

            AnimationControllerInfo result = new AnimationControllerInfo();
            result.Name = infoName;

            return result;
        }

        private AnimationData[] GetValidDatas(AnimationClip[] clips, AnimationData[] datas) {
            List<AnimationData> results = new List<AnimationData>();
            if (clips == null) return results.ToArray();

            for(int i = 0; i < clips.Length; i++) 
                results?.Add(GetValidData(i, datas));

            return results.ToArray();
        }

        private AnimationData GetValidData(int index, AnimationData[] datas) {
           AnimationData data = null;

           if (datas == null || index < 0 || index >= datas.Length) {
               data = new AnimationData();
               data?.SetIndex(index);
           }
           else {
               datas[index]?.SetIndex(index);
               data = datas[index];
           }

           return data;
        }
#endif

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            Initialize(animator);
        }

        protected virtual void Initialize(Animator animator) { }

        protected void Play(AnimationData data) {
            if(datas == null || animator == null || controller == null || !datas.Contains(data)) return;
            animator.runtimeAnimatorController = controller;
            animator?.CrossFadeAnimation(data);
        }

        public void PlayDefaultData()
        {
            Play(GetDefaultData());
        }

        protected bool IsPlayingData(string dataName) {
           if(string.IsNullOrEmpty(dataName) || animator == null) return false;
           return animator.IsCurrentClipPlaying(dataName.ToLower());
        }

        protected AnimationData GetData(int index) {
            if (datas == null || index < 0 || index >= datas.Length) return null;
            return datas[index];
        }

        protected AnimationData GetData(string infoName) {
            if(infos == null || string.IsNullOrEmpty(infoName)) return null;
            infoName = infoName.ToLower();

            foreach(AnimationControllerInfo info in infos)
                if (info.Name.ToLower() == infoName) return GetData(info.AnimationDataIndex);

            return null;
        }


        public AnimationData GetDefaultData() {  return GetData(DEFAULT_ANIMATION_NAME); }
    }
}
