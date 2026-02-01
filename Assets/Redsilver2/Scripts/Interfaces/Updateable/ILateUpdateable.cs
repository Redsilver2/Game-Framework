using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework
{
    public interface ILateUpdateable {
        void LateUpdate();
        void AddOnLateUpdateListener(UnityAction action);
        void RemoveOnLateUpdateListener(UnityAction action);
    }
}
