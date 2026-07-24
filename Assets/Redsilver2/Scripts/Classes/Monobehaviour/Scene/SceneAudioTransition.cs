using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework.Scenes
{
    public sealed class SceneAudioTransition : SceneLoaderEvent
    {
        [SerializeField] private float audioFadeDuration;
        private IEnumerator updater;

        protected sealed override void SetEvents(SceneLoaderManager manager, bool isAddingEvents)
        {
            if(isAddingEvents) {
                manager?.AddOnSingleSceneLoadStartedListener(OnSingleSceneLoadStarted);
                manager?.AddOnSingleSceneLoadFinishedListener(OnSingleSceneLoadFinished);
            }
            else {
                manager?.RemoveOnSingleSceneLoadStartedListener(OnSingleSceneLoadStarted);
                manager?.RemoveOnSingleSceneLoadFinishedListener(OnSingleSceneLoadFinished);
            }
        }

        private void OnSingleSceneLoadStarted(int sceneIndex) {
            StartAudioUpdate(true);
        }

        private void OnSingleSceneLoadFinished(int sceneIndex) {
            StartAudioUpdate(false);
        }

        private void StartAudioUpdate(bool isMutingAudio)
        {
            if(updater != null) {
                StopCoroutine(updater);
                updater = null;
            }

            updater = UpdateAudio(isMutingAudio, 0.5f);
            StartCoroutine(updater);
        }

       

        private IEnumerator UpdateAudio(bool isMutingAudio, float waitTime)
        {
            float currentVolume = AudioListener.volume;
            float nextVolume    = Mathf.Clamp01(isMutingAudio ? 0f : 1f);
            float t             = 0f;

            yield return new WaitForSeconds(waitTime);

            while(t < audioFadeDuration) {
                AudioListener.volume = Mathf.Clamp(currentVolume, nextVolume, Mathf.Clamp01(t / audioFadeDuration));
                Debug.Log(Mathf.Clamp01(t / audioFadeDuration));

                t += Time.deltaTime;
                yield return null;
            }

            AudioListener.volume = nextVolume;
            Debug.Log(AudioListener.volume);
        }
    }
}
