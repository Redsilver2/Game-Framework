using UnityEngine;

namespace RedSilver2.Framework.Performance.Lights
{
    [System.Serializable]
    public struct LightOptimizerDistanceCheck
    {
        [SerializeField] public LightOptimizerSetting lightOptimizerSetting;
        [SerializeField] public float minDistanceCheck, maxDistanceCheck;
    }
}
