using RedSilver2.Framework.StateMachines.States;
using RedSilver2.Framework.StateMachines.States.Movement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    public abstract class PlayerController : MovementStateMachineController
    {
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

        protected override void Awake() {
            base.Awake();

            StateMachine.AddOnStateEnteredListener(state => { Debug.Log("Current State: " + (state == null ? "Null" : state.GetType().ToString())); });
            StateMachine.AddOnStateExitedListener(state => { Debug.Log("Previous State: " + (state == null ? "Null" : state.GetType().ToString())); });

            StateMachine.ChangeState(MovementStateType.Idol.ToString());
            current = this;
            instances.Add(this);
        }


        private void OnDestroy() {
            if (instances.Contains(this)) instances.Remove(this);
        }

        protected sealed override MovementHandler GetMovementHandler() {
            return GetPlayerMovementHandler();
        }

        protected sealed override MovementStateMachine GetStateMachine(MovementHandler movementHandler) {
            return new MovementStateMachine(this, movementHandler);   
        }

        protected abstract PlayerMovementHandler GetPlayerMovementHandler();


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
