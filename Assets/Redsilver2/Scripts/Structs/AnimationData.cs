using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR;

namespace RedSilver2.Framework.Animations
{
    [System.Serializable]
    public class  AnimationData 
    {
        [SerializeField] private string animationName;

        [Space]
        [SerializeField] private int animationIndex;

        [Space]
        [SerializeField] private float  crossFadeTime;
        [SerializeField] private List<AnimationTimestampEvent> timestampEvents;

        [Space]
        [SerializeField] private UnityEvent onStarted, onFinished;

        public string AnimationName => animationName;
        public float CrossFadeTime => crossFadeTime;
        public AnimationTimestampEvent[] TimestampEvents
        {
            get {
                if(timestampEvents == null) return new AnimationTimestampEvent[0];
                return timestampEvents.ToArray();
            }
        }

        public AnimationTimestampEvent GetTimestampEvent(float time) {
            var results = timestampEvents.Where(x => x.TriggerTime == time);
            if (results.Count() > 0) return results.First();
            return null;
        }

        public void AddTimestampEvent(float time, UnityAction action)
        {
            AnimationTimestampEvent timestampEvent = GetTimestampEvent(time);
           
            if (timestampEvent == null) {
                timestampEvent = new AnimationTimestampEvent(time);
                timestampEvents?.Add(timestampEvent);
            }

            timestampEvent?.AddAction(action);
        }

        public void RemoveTimestampEvent(float time, UnityAction action) {
             AnimationTimestampEvent timestampEvent = GetTimestampEvent(time);
             timestampEvent?.RemoveAction(action);
        }
        public void ResetTimeStampEvents()
        {
            if(timestampEvents == null) return; 

            foreach (AnimationTimestampEvent timestampEvent in timestampEvents)
                timestampEvent?.Reset();
        }

        public void AddOnStartedListener(UnityAction action)
        {
            if (action != null) onStarted?.AddListener(action);
        }
        public void RemoveOnStartedListener(UnityAction action)
        {
            if (action != null) onStarted?.RemoveListener(action);
        }

        public void AddOnFinishedListener(UnityAction action)
        {
            if (action != null) onFinished?.AddListener(action);
        }
        public void RemoveOnFinishedListener(UnityAction action)
        {
            if (action != null) onFinished?.AddListener(action);
        }

        public void Finish() { onFinished?.Invoke(); }
        public void Start()  { onStarted?.Invoke(); }

#if UNITY_EDITOR
        public void Validate(RuntimeAnimatorController controller)
        {
            ValidateAnimationName(controller);
            ValidateAnimationTimeStamps(AnimationManager.GetClip(controller, animationName));
        }

        private void ValidateAnimationName(RuntimeAnimatorController controller)
        {
            string[] animationNames = AnimationManager.GetClipNames(controller);

            if (animationNames == null || animationNames.Length == 0) animationIndex = -1;
            else animationIndex = Mathf.Clamp(animationIndex, 0, animationNames.Length - 1);
   
            animationName = AnimationManager.GetClipName(controller, animationIndex);
        }

        private void ValidateAnimationTimeStamps(AnimationClip current)
        {
            if (current != null && timestampEvents != null) {
                foreach (AnimationTimestampEvent timestampEvent in timestampEvents)
                    timestampEvent?.Validate(current);

                crossFadeTime = Mathf.Clamp(crossFadeTime, 0f, current.length);
            }
            else { crossFadeTime = 0f; }
        }
        #endif
    }
}
