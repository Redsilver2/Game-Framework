using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Stats
{
    public abstract class RegenerativeNumberStat : NumberStat
    {
        [SerializeField] private bool canRegenerate;

        [Space]
        [SerializeField][Range(1f, 10f)] private float regenerationWaitTime;
        [SerializeField][Range(1f, 10f)] private float regenerationSpeed;

        [Space]
        [SerializeField][Range(0.1f, 1f)] private float regenerationProgressTrigger;
        [SerializeField][Range(0.1f, 1f)] private float maxDefaultRegenerationProgress;

        private float previousProgress;
        private bool isRegeneratingValue;

        private IEnumerator regenerationCoroutine;

        private UnityEvent onRegenerationStarted;
        private UnityEvent onRegenerationFinished;

        protected override void Awake() {
            base.Awake();
            previousProgress = Progress;

            onRegenerationStarted = new UnityEvent();
            onRegenerationFinished = new UnityEvent();

            AddOnProgressChangedListener(value => {

                if (value >= previousProgress || value <= 0f) StopRegeneration();
                else if (value <= regenerationProgressTrigger && !isRegeneratingValue) StartRenegeration(maxDefaultRegenerationProgress, regenerationWaitTime);

                value = previousProgress;
            });

            AddOnRegenerationStarted(() =>
            {
                isRegeneratingValue = true;

            });

            AddOnRegenerationFinishedListener(() => {
                if (regenerationCoroutine != null) StopCoroutine(regenerationCoroutine);
                regenerationCoroutine = null;
                isRegeneratingValue = false;
            });

        


        }

        public void StartRenegeration(float desiredProgress, float waitTime) {
            if (desiredProgress >= 1f) desiredProgress = 1f;

            if (Progress < desiredProgress)
            {
                StopRegeneration();
                regenerationCoroutine = Regenerate(desiredProgress, waitTime);
                StartCoroutine(regenerationCoroutine);
            }
        }

        public void StopRegeneration() {
            if(regenerationCoroutine != null) {
                onRegenerationFinished?.Invoke();
            }
        }

        private IEnumerator Regenerate(float progress, float waitTime)
        {
            if (progress > Progress && canRegenerate) {      
                yield return StartCoroutine(WaitRegenerate(waitTime));
                onRegenerationStarted?.Invoke();

                while (Progress < progress)
                {
                    if (!canRegenerate) { yield break; }
                    float regenerationSpeed = UseWholeValue ? (int)this.regenerationSpeed : Time.deltaTime * this.regenerationSpeed;
                    SetCurrentValue(regenerationSpeed + CurrentValue);
                    yield return StartCoroutine(WaitRegenerate(UseWholeValue ? Time.deltaTime : 0f));
                }

                SetCurrentValue(MaxValue * progress);
                onRegenerationFinished?.Invoke();
            }
        }

        private IEnumerator WaitRegenerate(float waitTime)
        {
            float t = 0f;

            while (t < waitTime) {
                t += Time.deltaTime;
                yield return null;
            }

        }
        public void AddOnRegenerationStarted(UnityAction action)
        {
            if(action != null) onRegenerationStarted?.AddListener(action);  
        }

        public void RemoveOnRegenerationStarted(UnityAction action)
        {
            if (action != null) onRegenerationStarted?.RemoveListener(action);
        }

        public void AddOnRegenerationFinishedListener(UnityAction action) {

            if (action != null) onRegenerationFinished?.AddListener(action);
        }
        public void RemoveOnRegenerationFinishedListener(UnityAction action)
        {

            if (action != null) onRegenerationFinished?.RemoveListener(action);
        }
    }
}
