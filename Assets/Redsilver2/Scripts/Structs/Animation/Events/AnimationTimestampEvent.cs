using UnityEngine.Events;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace RedSilver2.Framework.Animations
{
    [System.Serializable]
    public class AnimationTimestampEvent 
    {
        [SerializeField]  private float       triggerTime;
        [SerializeField]  private UnityEvent  triggerEvent;
                          private bool        wasTriggered;

        private List<UnityAction> registeredActions;

        public float TriggerTime => triggerTime;
        public bool WasTriggered => wasTriggered;


        public AnimationTimestampEvent()
        {
            wasTriggered           = false;
            this.triggerTime       = 0f;

            this.triggerEvent      = new UnityEvent();
            this.registeredActions = new List<UnityAction>();
        }


        public AnimationTimestampEvent(float triggerTime)
        {
            wasTriggered           = false;
            this.triggerTime       = triggerTime;

            this.triggerEvent      = new UnityEvent();
            this.registeredActions = new List<UnityAction>();
        }

        public void Trigger(float time) {
            if(CanTrigger(time)) {
                wasTriggered = true;
                triggerEvent?.Invoke();
            }
        }

        public bool CanTrigger(float time)
        {
            if (!wasTriggered && triggerTime >= 0f && triggerTime <= time)
                return true;

            return false;
        }

        public void Reset()
        {
            wasTriggered = false;
        }

        public void AddAction(UnityAction action)
        {
            if (action != null && !registeredActions.Contains(action)) {
                registeredActions?.Add(action);
                triggerEvent?.AddListener(action);
            }
        }

        public void RemoveAction(UnityAction action)
        {
            if (action != null && registeredActions.Contains(action)) {
                registeredActions?.Remove(action);
                triggerEvent?.RemoveListener(action);
            }
        }

        #if UNITY_EDITOR
        public void Validate(AnimationClip clip)
        {
            triggerTime = Mathf.Clamp(triggerTime, 0f, clip != null ? clip.length : float.MaxValue);
        }
#endif

        public static AnimationTimestampEvent[] MergeArray(AnimationTimestampEvent[] timestampEvents, float time)
        {
            AnimationTimestampEvent       newEvent = Merge(timestampEvents, time);
            AnimationTimestampEvent[]     similars = timestampEvents.Where(x => x.TriggerTime == newEvent.TriggerTime).ToArray();
            List<AnimationTimestampEvent> results  = timestampEvents.Where(x => !similars.Contains(x)).ToList();

            results.Add(newEvent);
            return results.OrderBy(x => x.TriggerTime).ToArray();
        }

        public static AnimationTimestampEvent Merge(AnimationTimestampEvent[] timestampEvents, float time) {
            if (timestampEvents == null || timestampEvents.Length == 0) return null;

            AnimationTimestampEvent[] similars = timestampEvents.Where(x => x.TriggerTime == time).ToArray();
            AnimationTimestampEvent   result   = new AnimationTimestampEvent(time);

            foreach (AnimationTimestampEvent similar in similars)
                result = Merge(result, similar);

            return result;
        }

        private static AnimationTimestampEvent Merge(AnimationTimestampEvent newEvent, AnimationTimestampEvent oldEvent) {
            if(oldEvent == null) return newEvent;

            foreach(UnityAction action in oldEvent.registeredActions)
                newEvent?.AddAction(action);

            return newEvent;
        }
    }
}
