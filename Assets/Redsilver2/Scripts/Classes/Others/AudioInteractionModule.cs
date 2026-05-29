using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions {
    [RequireComponent(typeof(AudioSource))]
    public class AudioInteractionModule : InteractionModule {

        [Space]
        [SerializeField][Range(0.1f, 1f)] private float volumeIncrementValue;

        private bool isPaused = false;
        private float currentVolume;
        private AudioSource source;

        private IEnumerator sourceVolumeUpdateCoroutine;

        private UnityEvent<AudioClip> onPlay;
        private UnityEvent onStop, onPause, onUnPause;



        protected override void Awake() {
            base.Awake();
            onPlay    = new UnityEvent<AudioClip>();
            
            onStop    = new UnityEvent();
            onPause   = new UnityEvent();

            onUnPause = new UnityEvent();   
            source    = GetComponent<AudioSource>();

            volumeIncrementValue = 0.1f;
            currentVolume = source.volume;

            AddOnPauseListener  (() => { isPaused = true;  });
            AddOnUnPauseListener(() => { isPaused = false; });
            AddOnStopListener   (() => 
            {
                isPaused = false;
                StartSourceVolumeUpdate(true);
            });
            AddOnPlayListener   (clip => {
                if (source == null || clip == null) return;
                isPaused = false;
                
                source.clip = clip;
                source?.Play();

                StartSourceVolumeUpdate(false);
            });
        }

        public void SetLoop(bool loop) {
            if (source != null) source.loop = loop;
        }

        public void IncreaseVolume() {
            IncreaseVolume(volumeIncrementValue);
        }

        public void IncreaseVolume(float amount) {
            if (source == null) return;  
            source.volume = Mathf.Clamp(source.volume + amount, 0f, 1f);
            currentVolume = source.volume;
        }

        public void DecreaseVolume() {
            DecreaseVolume(volumeIncrementValue);
        }
        public void DecreaseVolume(float amount) {
            if (source == null) return;
            source.volume = Mathf.Clamp(source.volume - amount, 0f, 1f);
            currentVolume = source.volume;
        }

        public void UnPause() {
            if(source == null || !source.isPlaying || !isPaused) return;
            onUnPause?.Invoke();
        }

        public void Pause() {
            if (source == null || !source.isPlaying || isPaused) return;
            onPause?.Invoke();
        }

        public void Stop() {
            if(source == null || !source.isPlaying) return;
            onStop?.Invoke();
        }

        private void StopSourceVolumeUpdate()
        {
            if (sourceVolumeUpdateCoroutine != null)
                StopCoroutine(sourceVolumeUpdateCoroutine);

            sourceVolumeUpdateCoroutine = null;
        }

        private void StartSourceVolumeUpdate(bool isMuting) {
            StopSourceVolumeUpdate();
            sourceVolumeUpdateCoroutine = SourceVolumeUpdate(isMuting, 0.005f);
            StartCoroutine(sourceVolumeUpdateCoroutine);
        }

        private IEnumerator SourceVolumeUpdate(bool isMuting, float time) {
            float current = source != null ? source.volume : 0f;
            float target  = isMuting ? 0f : currentVolume;
            float t = 0f;

            while (t < time) {
                if (source != null) source.volume = Mathf.Lerp(current, target, Mathf.Clamp01(t / time));
                t += Time.deltaTime;
                yield return null;
            }

            if(source != null) {
               source.volume = target;
               if(isMuting) source?.Stop();
            }        
        }

        public void Play(AudioClip clip) {
            if (source == null || clip == null) return;
            onPlay?.Invoke(clip);
        }

        public void AddOnPauseListener(UnityAction action) {
            if (onPause != null) onPause?.AddListener(action);
        }
        public void RemoveOnPauseListener(UnityAction action) {
            if (onPause != null) onPause?.RemoveListener(action);
        }

        public void AddOnUnPauseListener(UnityAction action)
        {
            if (onUnPause != null) onUnPause?.AddListener(action);
        }
        public void RemoveOnUnPauseListener(UnityAction action)
        {
            if (onUnPause != null) onUnPause?.RemoveListener(action);
        }

        public void AddOnStopListener(UnityAction action)
        {
            if (onStop != null) onStop?.AddListener(action);
        }
        public void RemoveOnStopListener(UnityAction action)
        {
            if (onStop != null) onStop?.RemoveListener(action);
        }

        public void AddOnPlayListener(UnityAction<AudioClip> action)
        {
            if (onPlay != null) onPlay?.AddListener(action);
        }
        public void RemoveOnPlayListener(UnityAction<AudioClip> action)
        {
            if (onPlay != null) onPlay?.RemoveListener(action);
        }

        public bool IsLooping() {
            if(source == null) return false;
            return source.loop;
        }

        public bool IsPlaying()
        {
            if(source == null) return false;
            return isPaused ? true : source.isPlaying;
        }
    }
}
