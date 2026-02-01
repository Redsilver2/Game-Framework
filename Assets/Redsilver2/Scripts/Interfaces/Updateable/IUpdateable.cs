using UnityEngine.Events;

namespace RedSilver2.Framework {
    public interface IUpdateable {
        void Update();
        void AddOnUpdateListener(UnityAction action);
        void RemoveOnUpdateListener(UnityAction action);
    }

}