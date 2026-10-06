using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace RedSilver2.Framework {
    public sealed class CoroutineManager : MonoBehaviour {
        private static CoroutineManager instance;
        private const string INSTANCE_NAME = "COROUTINE MANAGER";

        private void Awake() {
            if(instance == null || instance == this) {
               instance        = this;      
               gameObject.name = INSTANCE_NAME;
             
               gameObject.transform.position = Vector3.zero;
               DontDestroyOnLoad(instance);
            }
            else if(instance != this) { Destroy(gameObject); }
        }

        private static CoroutineManager GetInstance()
        {
            if (instance == null) {
                instance = new GameObject(INSTANCE_NAME).GetOrAddComponent<CoroutineManager>();
            }

            return instance;
        }

        public static Coroutine Start(IEnumerator enumerator)
        {
            return enumerator == null ? null : GetInstance()?.StartCoroutine(enumerator);
        }

        public static void Stop(IEnumerator enumerator) 
        {
            GetInstance()?.StopCoroutine(enumerator);
        }
    }
}
