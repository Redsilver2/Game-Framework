using RedSilver2.Framework.Player;
using UnityEngine;

namespace RedSilver2.Framework.Performance.Lights
{
    public abstract partial class LightManager : MonoBehaviour
    {
        protected sealed class LightOptimizer
        {
            private readonly Light light;

            protected LightOptimizer(Light light) {
                this.light = light;
            }

            public void Optimize() 
            {
                 Optimize(PlayerController.Current, GameManager.LightManager);
            }

            public void Optimize(PlayerController controller) 
            {
                if (controller != null)
                    Optimize(controller.gameObject, GameManager.LightManager);
            }

            public void Optimize(PlayerController controller, LightManager manager) 
            {
                if (controller != null) 
                    Optimize(controller.gameObject, manager);
            }

            public void Optimize(GameObject gameObject) 
            {
               Optimize(gameObject, GameManager.LightManager);
            }

            public void Optimize(GameObject gameObject, LightManager manager)
            {
                if (light != null && gameObject != null && manager != null)
                {
                    Optimize(Vector3.Distance(light.transform.position, gameObject.transform.position), manager);
                    manager.DebugLightOptimization(light, gameObject);
                }
                   
            }

            private void Optimize(float distance, LightManager manager) {
                if (manager != null && light != null) {
                    if (light.type == LightType.Directional) return;
                    light.enabled = manager.CanOptimize((int)distance);

                    if (light.enabled)
                        manager.GetLightOptimizationSetting((int)distance)?.Optimize(light);
                }            
            }

            public static LightOptimizer CreateAndGet(Light light)
            {
                if(light == null || light.type == LightType.Directional) return null;
                return new LightOptimizer(light);
            }
        }
    }
}
