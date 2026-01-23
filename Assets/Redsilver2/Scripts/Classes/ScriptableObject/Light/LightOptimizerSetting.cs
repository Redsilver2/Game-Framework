using UnityEngine;

namespace RedSilver2.Framework.Performance.Lights
{
    [CreateAssetMenu(menuName = "Light/Optimizer/Setting", fileName = "New Light Optimizer Setting")]
    public class LightOptimizerSetting : ScriptableObject
    {
        [SerializeField] UnityEngine.Rendering.LightShadowResolution shadowResolution;
        [SerializeField] private LightShadows lightShadows;
        [SerializeField] private LightShadowCasterMode shadowCasterMode;

        [Space]
        [SerializeField] private LightRenderMode lightRenderMode;

        public void Optimize(Light light)
        {
            if(light == null) return;
            light.shadowResolution = shadowResolution;
            light.shadows = lightShadows;
            light.renderMode = lightRenderMode;
            light.lightShadowCasterMode = shadowCasterMode;
        }
    }
}
