using System.Collections;
using UnityEngine;


namespace RedSilver2.Framework.Interactions
{
    public class TimedInteractionModule : AdvancedHoldInteractionModule {

        [Space]
        [SerializeField] private float resetInteractionSpeed;
        [SerializeField] private float resetInteractionDelay;

        [Space]
        [SerializeField] private bool clearTimerValue;

        private bool isResettingInteractionTimer;
        private IEnumerator resetTimeCoroutine;

        protected override void Awake()
        {
            base.Awake();
            isResettingInteractionTimer = false;
        }

        protected override void Start()
        {
            base.Start();

            AddOnPressInteractionListener(handler =>
            {
                isResettingInteractionTimer = false;
                if (resetTimeCoroutine != null) StopCoroutine(resetTimeCoroutine);
                resetTimeCoroutine = null;
            });

            AddOnReleaseInteractionListener(handler =>
            {
                if (CanClearProgressOnRelease && !isResettingInteractionTimer && enabled && resetInteractionSpeed > 0)
                {
                    resetTimeCoroutine = StartResetInteractionTime(handler);
                    StartCoroutine(resetTimeCoroutine);
                }
            });
        }

        protected override void OnDisable() {
            if (resetTimeCoroutine != null) StopCoroutine(resetTimeCoroutine);
        }



        private IEnumerator StartResetInteractionTime(InteractionHandler handler)
        {
            float t = 0f;

            while (t < resetInteractionDelay)
            {
                t += Time.deltaTime;
                yield return null;
            }

            isResettingInteractionTimer = true;
            yield return StartCoroutine(ResetInteractionTime(handler));
        }

        private IEnumerator ResetInteractionTime(InteractionHandler handler)
        {
            while (interactionTime > 0f)
            {
                UpdateProgress(resetInteractionSpeed, handler);
                yield return null;
            }

            interactionTime = 0f;
            isResettingInteractionTimer = false;


        }
    }
}
