using RedSilver2.Framework.StateMachines;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RedSilver2.Framework.Animations
{
    public class ConsumableItemAnimationController : EquippableItemAnimationController {
#if UNITY_EDITOR
        protected override AnimationControllerInfo[] GetValidInfos(AnimationControllerInfo[] infos)
        {
            List<AnimationControllerInfo> results = base.GetValidInfos(infos).ToList();
            results?.Add(GetValidInfo(ConsumableItemStateMachine.CONSUME_ANIMATION_NAME, infos));
            return results.ToArray();
        }
#endif

        public void PlayConsumeData() { Play(GetConsumeData()); }
        public AnimationData GetConsumeData() { return GetData(ConsumableItemStateMachine.CONSUME_ANIMATION_NAME); }

    }
}
