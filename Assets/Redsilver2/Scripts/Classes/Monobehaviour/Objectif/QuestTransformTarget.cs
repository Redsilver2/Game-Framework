using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Quests.Targets {
    [System.Serializable]
    public sealed class QuestTransformTarget : QuestTarget
    {
        private readonly List<Transform> targets; 
        public QuestTransformTarget(string targetName) : base(targetName) { 
            targets = new List<Transform>(); 
        }

        public void Register(Transform transform)
        {
            if(targets == null || transform == null || targets.Contains(transform)) return;
            targets?.Add(transform);
        }

        public void Unregister(Transform transform)
        {
            if(targets == null || transform == null || !targets.Contains(transform)) return;
            targets?.Remove(transform);
        }

        public Transform[] GetTransforms() {
            if(targets == null) return new Transform[0];    
            return targets.ToArray(); 
        }
    }
}
