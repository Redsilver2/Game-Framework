using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework.Scenes.UI
{
    public abstract class LoadingIcon : LoadingScreenUI
    {
        [SerializeField] private float rotationSpeed = 5f;
        private bool isRotatingIcon;
        
        private Vector3 defaultRotation;
        private Transform _transform;

        #if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (rotationSpeed < 0f) rotationSpeed = 0f;
        }
        #endif


        protected void Awake()
        {
            _transform = GetComponent<Transform>();  
            if (_transform != null)       defaultRotation     = _transform.localEulerAngles;
           
            isRotatingIcon   = false;
        }

        private void OnSingleSceneLoadStarted(int sceneIndex)
        {
            if (_transform != null) _transform.localEulerAngles = defaultRotation;
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

            while (isRotatingIcon)
            {
                _transform.Rotate(Time.deltaTime * rotationSpeed * GetDesiredRotation());
                yield return null;
            }
        }
    }
}
