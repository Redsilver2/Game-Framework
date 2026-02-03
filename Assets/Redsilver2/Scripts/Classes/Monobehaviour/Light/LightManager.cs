using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Performance.Lights {
    public abstract partial class LightManager : MonoBehaviour
    {
        [SerializeField] private float lightOptimizationDelay = 0.5f;
        [SerializeField] private bool  isDynamicallyOptimizingLight;

        [Space]
        [SerializeField] private bool canDebugLightOptimization;

        [Space]
        [SerializeField] private LightOptimizerDistanceCheck[] lightOptimizationDistanceChecks;




        private Dictionary<Light, LightOptimizer> ligthOptimizations;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            ClampLightOptimizerDistances();
        }

        private void ClampLightOptimizerDistances()
        {
            for (int i = 0; i < lightOptimizationDistanceChecks.Length; i++) {
                if      (i == 0) ClampLightOptimizerDistance(ref lightOptimizationDistanceChecks[i], 0f, Mathf.Infinity);
                else if (i == lightOptimizationDistanceChecks.Length - 1) ClampLightOptimizerDistance(ref lightOptimizationDistanceChecks[i], lightOptimizationDistanceChecks[i - 1].maxDistanceCheck, Mathf.Infinity);
                else ClampLightOptimizerDistance(ref lightOptimizationDistanceChecks[i], lightOptimizationDistanceChecks[i - 1], lightOptimizationDistanceChecks[i + 1]);
            }
        }

        private void ClampLightOptimizerDistance(ref LightOptimizerDistanceCheck  distanceCheck, float minDistance, float maxDistance) {
            distanceCheck.minDistanceCheck = minDistance;
            distanceCheck.maxDistanceCheck = Mathf.Clamp(distanceCheck.maxDistanceCheck, distanceCheck.minDistanceCheck, maxDistance);
        }

        private void ClampLightOptimizerDistance(ref LightOptimizerDistanceCheck distanceCheck01, LightOptimizerDistanceCheck distanceCheck02, LightOptimizerDistanceCheck distanceCheck03)
        {
            distanceCheck01.minDistanceCheck = distanceCheck02.maxDistanceCheck;

            if (distanceCheck01.maxDistanceCheck < distanceCheck01.minDistanceCheck) distanceCheck01.maxDistanceCheck = distanceCheck01.minDistanceCheck;
            else distanceCheck01.maxDistanceCheck = distanceCheck03.minDistanceCheck;
        }


#endif

        private void Awake() {  
            ligthOptimizations = new Dictionary<Light, LightOptimizer>();
            SetupSceneEvents();
            ResetLightOptimizers();
            StartCoroutine(UpdateLights());
        }

        public void AddLightOptimizers(Light[] lights) {
            if(lights != null)
              foreach (Light light in lights) AddLightOptimizer(light);
        }

        public void AddLightOptimizer(Light light) {
            if (light != null && !ligthOptimizations.ContainsKey(light)) {
                LightOptimizer lightOptimizer = LightOptimizer.CreateAndGet(light);
                if(lightOptimizer != null) ligthOptimizations.Add(light, lightOptimizer);
            }
        }

        private void SetupSceneEvents()
        {
           GameManager.SceneLoaderManager.AddOnSceneAssetAddedListener(sceneData => {
               if (sceneData == null) return;
               sceneData.AddOnLoadFinishedListener(ResetLightOptimizers);
           });
        }

        private IEnumerator UpdateLights() {
            float time = 0f;
            bool canTriggerFirstOptimization = true;

            while (isDynamicallyOptimizingLight) {

                if (canTriggerFirstOptimization || time >= lightOptimizationDelay)
                    UpdateLights(GetLightOptimizers(), ref time, ref canTriggerFirstOptimization);

                time += Time.deltaTime;
                yield return null;
            }
        }

        private void UpdateLights(LightOptimizer[] lightOptimizers, ref float updateTime, ref bool canTriggerFirstOptimization) {
            canTriggerFirstOptimization = false;
            updateTime = 0f;
            UpdateLights(lightOptimizers);
        }

        protected abstract void UpdateLights(LightOptimizer[] lightOptimizers);

        private void ResetLightOptimizers() {
            ligthOptimizations.Clear();
            AddLightOptimizers(FindObjectsByType<Light>(FindObjectsSortMode.None));
        }

        public void DebugLightOptimization(Light light, GameObject target) {
            if(light != null && target != null) {
                if (ligthOptimizations.ContainsKey(light)) {
                    Debug.DrawLine(light.transform.position, target.transform.position, light.enabled ? Color.green : Color.red);
                }
            }
        }

        public bool CanOptimize(int distance) {
            if (lightOptimizationDistanceChecks.Length > 0)
                return distance < lightOptimizationDistanceChecks.Last().maxDistanceCheck;
            return false;
        }

        public LightOptimizerSetting GetLightOptimizationSetting(int distance) {
            if (lightOptimizationDistanceChecks == null || lightOptimizationDistanceChecks.Length == 0) return null;
            return lightOptimizationDistanceChecks.Where(x => distance >= x.minDistanceCheck && distance < x.maxDistanceCheck)
                                                        .FirstOrDefault().lightOptimizerSetting; ;
        }

        private LightOptimizer[] GetLightOptimizers() {
            Light[] lights = ligthOptimizations.Keys.Where(x => x != null).ToArray();
            Dictionary<Light, LightOptimizer> newResults = new Dictionary<Light, LightOptimizer>();
            Dictionary<Light, LightOptimizer> oldResults = ligthOptimizations;

            foreach (Light light in lights) newResults.Add(light, oldResults[light]);
            ligthOptimizations = newResults;

            return ligthOptimizations.Values.ToArray(); 
        }
    }
}
