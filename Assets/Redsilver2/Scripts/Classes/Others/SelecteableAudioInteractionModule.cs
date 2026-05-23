using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions
{
    public class SelecteableAudioInteractionModule : AudioInteractionModule
    {
        [Space]
        [SerializeField] private AudioClip[] clips;

        [Space]
        [SerializeField] private bool automaticallyPlaySound;
        [SerializeField] private bool canWrapCurrentIndex;

        private int currentIndex;
        private int previousIndex;

        private IEnumerator playUpdateCoroutine;
        private UnityEvent<int> onCurrentIndexChanged;

        protected override void Awake()
        {
            base.Awake();
          
            currentIndex = 0;
            previousIndex = 0;

            onCurrentIndexChanged = new UnityEvent<int>();

            AddOnStopListener(() => { StopPlayUpdate(); });
            AddOnPlayListener(StartPlayUpdate);
        }

        public void SetAutomaticallyPlaySound(bool automaticallyPlaySound)
        {
            this.automaticallyPlaySound = automaticallyPlaySound;
        }

        public void SetCanWrapCurrentIndex(bool canWrapCurrentIndex)
        {
            this.canWrapCurrentIndex = canWrapCurrentIndex;
        }

        private void StopPlayUpdate() {
            if (playUpdateCoroutine != null) StopCoroutine(playUpdateCoroutine);
            playUpdateCoroutine = null;
        }

        private void StartPlayUpdate(AudioClip clip) {
            StopPlayUpdate();
            if (clips == null || clip == null) return;

            if (clips.Contains(clip)) {
                currentIndex = GetClipIndex(clip);
                playUpdateCoroutine = PlayUpdate();

                StartCoroutine(playUpdateCoroutine);
            }

        }

        private IEnumerator PlayUpdate() {
            while (IsPlaying()) yield return null; 
            if (automaticallyPlaySound) PlayNext();
        }

        public void AddOnCurrentIndexChangedListener(UnityAction<int> action) {
            if (action != null) onCurrentIndexChanged?.AddListener(action);
        }

        public void RemoveOnCurrentIndexChangedListener(UnityAction<int> action)
        {
            if (action != null) onCurrentIndexChanged?.RemoveListener(action);
        }

        public void PlayNext()
        {
            if (clips == null) return;
            currentIndex = Mathf.Clamp(currentIndex++, 0, clips.Length);

            if (currentIndex == clips.Length)  currentIndex = canWrapCurrentIndex ? 0 : clips.Length - 1;
            if (currentIndex != previousIndex) onCurrentIndexChanged?.Invoke(currentIndex);

            Play(currentIndex);
        }

        public void PlayPrevious()
        {
            if (clips == null) return;
            currentIndex = Mathf.Clamp(currentIndex--, -1, clips.Length - 1);
          
            if (currentIndex == -1) currentIndex = canWrapCurrentIndex ? clips.Length - 1 : 0;
            if (currentIndex != previousIndex) onCurrentIndexChanged?.Invoke(currentIndex);

            Play(currentIndex);
        }

        public void Play(int index) {
            Play(GetClip(index));
        }

        private int GetClipIndex(AudioClip clip)
        {
            if (clips == null || clip == null) return 0;

            for(int i = 0; i < clips.Length; i++) {
                if (clips[i] == null) continue;
                else if (clips[i].Equals(clip)) return i;
            }

            return 0;
        }

        public AudioClip GetClip(int index)
        {
            if (clips == null || clips.Length < 0 || index < 0 || index >= clips.Length)
                return null;

            return clips[index];
        }
    }
}
