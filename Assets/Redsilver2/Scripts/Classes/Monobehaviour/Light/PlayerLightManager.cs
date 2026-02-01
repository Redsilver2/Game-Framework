using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;

namespace RedSilver2.Framework.Performance.Lights
{
    public sealed class PlayerLightManager : LightManager
    {
        protected sealed override void UpdateLights(LightOptimizer[] lightOptimizers)
        {
            if (lightOptimizers == null || lightOptimizers.Length == 0) return;

            foreach (LightOptimizer lightOptimizer in lightOptimizers)
                lightOptimizer.Optimize(PlayerController.Current, this);
        }
    }
}