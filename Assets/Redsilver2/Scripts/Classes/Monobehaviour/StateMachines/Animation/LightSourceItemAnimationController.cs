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
            results?.Add(GetValidInfo(LightSourceItemStateMachine.TURN_LIGHT_ON_ANIMATION_NAME, infos));
            results?.Add(GetValidInfo(LightSourceItemStateMachine.TURN_LIGHT_OFF_ANIMATION_NAME, infos));
            return results.ToArray();
        }
#endif
        public void PlayTurnOnLightData() { Play(GetTurnLightOnData()); }
        public void PlayTurnOffLightData() { Play(GetTurnLightOffData()); }

        public AnimationData GetTurnLightOnData() { return GetData(LightSourceItemStateMachine.TURN_LIGHT_ON_ANIMATION_NAME); }
        public AnimationData GetTurnLightOffData() { return GetData(LightSourceItemStateMachine.TURN_LIGHT_OFF_ANIMATION_NAME); }
    }
}