using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.UI;
using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework
{
    public class SceneSetup : MonoBehaviour
    {
        [Space]
        [SerializeField] private PlayerController defaultPlayerController;
        [SerializeField] private float awakeWaitTime;

        [SerializeField] private UISelector defaultSelector;
        [SerializeField] private float enableSelectorWaitTime;

        private void Start() { 

            SetDefaultSelector(); 
        }

        private void SetDefaultSelector() {
            defaultSelector?.ResetIndexes();
            defaultSelector?.UpdateSelector(enableSelectorWaitTime);
        }

        private IEnumerator AwakePlayer() {
            float t = 0f;

            while(t < awakeWaitTime) {
                t += Time.deltaTime;
                yield return null;
            }
            
        }
    }
}
