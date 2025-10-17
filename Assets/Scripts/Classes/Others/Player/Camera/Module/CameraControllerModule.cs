using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RedSilver2.Framework.Player
{
    public abstract class CameraControllerModule : MonoBehaviour
    {
        private static List<CameraControllerModule> modules;

        private static CameraControllerModule current;
     
        public  static CameraControllerModule  Current => current;
        public  static CameraControllerModule[] Modules
        {
            get
            {
                if (modules == null) return new CameraControllerModule[0];
                return modules.ToArray();
            }
        }

        private void Awake() {
            SetCameraController(GetCameraController());
        }

        private void Start(){
            current = this;
        }

        protected abstract void Update();
        protected abstract void LateUpdate();

        protected abstract void OnEnable();
        protected abstract void OnDisable();

        protected abstract void SetCameraController(CameraController cameraController);
        protected abstract CameraController GetCameraController();

        public static void SetCurrent(int index) {
            SetCurrent(GetModule(index));
        }

        public static void SetCurrent(string moduleName) {
            SetCurrent(GetModule(moduleName));
        }


        public static void SetCurrent(CameraControllerModule module)
        {
            Disable();
            current = module;
            Enable();
        }

        public static void Enable() {
            if (current != null) current.enabled = true;
        }


        public static void Disable() {
            if(current != null) current.enabled = false;
        }

        public static void CleanModules() {
            if (modules != null) modules = modules.Where(x => x != null).ToList();
        }

        public static CameraControllerModule GetModule(int index) 
        {
            if(modules == null || index < 0 || index >= modules.Count) return null;
            return modules[index];
        }

        public static CameraControllerModule GetModule(string moduleName) 
        {
            if (modules == null || string.IsNullOrEmpty(moduleName)) return null;
            
            var results = modules.Where(x => x != null)
                                 .Where(x => x.name.ToLower() == moduleName.ToLower());

            if (results.Count() > 0) return results.First();
            return null;
        }
    }
}
