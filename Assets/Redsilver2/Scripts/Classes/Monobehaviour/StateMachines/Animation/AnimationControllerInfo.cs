using UnityEngine;
using static UnityEngine.Analytics.IAnalytic;

namespace RedSilver2.Framework.Animations
{
    [System.Serializable]
    public struct AnimationControllerInfo 
    {
        [HideInInspector] public string Name;
        public int    AnimationDataIndex;

#if UNITY_EDITOR
        public void Validate(AnimationData[] datas)
        {
            if(datas == null || datas.Length == 0) { AnimationDataIndex = -1; }
            else{ AnimationDataIndex = Mathf.Clamp(AnimationDataIndex, 0, datas.Length - 1); }
        }
#endif 
    }
}