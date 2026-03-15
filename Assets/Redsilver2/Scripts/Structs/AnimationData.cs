using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Animations
{
    [System.Serializable]
    public class  AnimationData 
    {
        [SerializeField] private string animationName;
        [SerializeField] private float  crossFadeTime;
        [SerializeField] private List<AnimationTimestampEvent> timestampEvents;

        public string AnimationName => animationName;
        public float CrossFadeTime => crossFadeTime;
        public AnimationTimestampEvent[] TimestampEvents
        {
            get
            {
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

        #if UNITY_EDITOR
        public void Validate(Animator animator)
        {
            MergeList();

            if (animator == null || timestampEvents.Count == 0f) return;
            AnimationClip clip = animator.GetAnimationClip(animationName); 
          
            if (clip == null) return;
            foreach (AnimationTimestampEvent timestampEvent in timestampEvents)
                timestampEvent?.Validate(clip);
        }

        private void MergeList() { 
             List<float> triggerTimes = new List<float>();

            for (int i = 0; i < timestampEvents.Count; i++)
                triggerTimes.Add(timestampEvents[i].TriggerTime);

            foreach(float triggerTime in triggerTimes.Distinct())
                timestampEvents = AnimationTimestampEvent.MergeArray(TimestampEvents, triggerTime).ToList();
        }
        #endif
    }
}
