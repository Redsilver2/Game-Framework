using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    public abstract class PlayerController : MonoBehaviour
    {
         [SerializeField] private bool dontDestroyOnLoad;

         private UnityEvent onEnabled;
         private UnityEvent onDisabled;


        private static PlayerController current;
        private static List<PlayerController> instances = new List<PlayerController>();

        public static PlayerController[] Instances
        {
            get {
                if (instances == null) return new PlayerController[0];
                return instances.ToArray();
            }
        }
        public  static PlayerController Current => current;

        protected virtual void Awake() {
            if (dontDestroyOnLoad) {
                DontDestroyOnLoad(this);
            }


            current = this;
            instances.Add(this);
        }

        private void OnEnable() { onEnabled?.Invoke(); }
        private void OnDisable() { onDisabled?.Invoke(); }

        protected void OnDestroy() {
            if(instances != null)
            {
                if (instances.Contains(this)) instances.Remove(this);
            }
        }

        public static void SetCurrent(int index) {
            SetCurrent(GetController(index));
        }

        public static void SetCurrent(string controllerName) {
            SetCurrent(GetController(controllerName));
        }

        public static void SetCurrent(PlayerController controller) {
            Disable();
            current = controller;
            Enable();
        }

        public static void Disable() {
            if (current != null) current.enabled = false;
        }

        public static void Enable() {
            if (current != null) current.enabled = true;
        }

        public static void CleanControllers() {
            if(instances != null) instances = instances.Where(x => x != null).ToList();
        }


        public static PlayerController GetController(int index) {
            if(instances.Count == 0 || index < 0 || index >= instances.Count)
                return null;
            return instances[index];
        }

        public static PlayerController GetController(string controllerName)
        {
            if(instances == null || string.IsNullOrEmpty(controllerName)) return null;

            var results = instances.Where(x => x != null)
                                   .Where(x => x.name.ToLower() == controllerName.ToLower());

            if (results.Count() > 0) return results.First();
            return null;
        }
    }
}
