using RedSilver2.Framework.StateMachines;
using System.Collections.Generic;
using System.Linq;

namespace RedSilver2.Framework.Animations
{
    public class LightSourceItemAnimationController : EquippableItemAnimationController
    {
#if UNITY_EDITOR
        protected override AnimationControllerInfo[] GetValidInfos(AnimationControllerInfo[] infos) {
            List<AnimationControllerInfo> results = base.GetValidInfos(infos).ToList();
            return results.ToArray();
        }
#endif
    }
}