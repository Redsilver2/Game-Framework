using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework.Scenes.UI
{
    public abstract class LoadingSpinningIcon : LoadingScreenImage
    {
        [Space]
        [SerializeField] private float rotationSpeed = 5f;
        
        private bool isRotatingIcon;      
        private Vector3 defaultRotation;

        #if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (rotationSpeed < 0f) rotationSpeed = 0f;
        }
        #endif


        protected override void Awake() {
            base.Awake();

            defaultRotation = transform.localEulerAngles; 
            isRotatingIcon  = false;
        }

        private void OnSingleSceneLoadStarted(int sceneIndex)
        {
            transform.localEulerAngles = defaultRotation;
            StartCoroutine(StartIconSpin());
        }

        private void OnSingleSceneLoadFinished(int sceneIndex)
        {
            isRotatingIcon = false;
        }

        protected override void DisableEvent(SceneLoaderManager sceneLoader)
        {
            if(sceneLoader != null)
            {
                sceneLoader.RemoveOnSingleSceneLoadFinishedListener(OnSingleSceneLoadFinished);
                sceneLoader.RemoveOnSingleSceneLoadStartedListener(OnSingleSceneLoadStarted);
            }
        }

        protected override void EnableEvent(SceneLoaderManager sceneLoader)
        {
            if (sceneLoader != null)
            {
                sceneLoader.AddOnSingleSceneLoadFinishedListener(OnSingleSceneLoadFinished);
                sceneLoader.AddOnSingleSceneLoadStartedListener(OnSingleSceneLoadStarted);
            }
        }

        protected abstract Vector3 GetDesiredRotation();

        private IEnumerator StartIconSpin()
        {
            isRotatingIcon = true;

            transform.localEulerAngles = defaultRotation;

            while (isRotatingIcon)
            {
                transform.Rotate(Time.deltaTime * rotationSpeed * Vector3.forward);
                yield return null;
            }
        }


    }
}
