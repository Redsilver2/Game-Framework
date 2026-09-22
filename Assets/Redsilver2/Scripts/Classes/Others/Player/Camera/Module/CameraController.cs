using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RedSilver2.Framework.Player
{
    [System.Serializable]
    public abstract partial class CameraController
    {
        [Space]
        [SerializeField, HideInInspector] private Camera camera;

        [SerializeField, HideInInspector] private string cameraName;
        [SerializeField, HideInInspector] private bool enabled;
        public Camera Camera => camera;

        private static List<CameraController> modules;
        private static CameraController current;
        public  static CameraController  Current => current;

        public static CameraController[] Modules
        {
            get
            {
                if (modules == null) return new CameraController[0];
                return modules.ToArray();
            }
        }

        public CameraController() {
            cameraName   = string.Empty;
            this.enabled = false;
        }

        public void SetCamera(Camera camera)
        {
            this.camera = camera;
        }

        public void Update() {
            if(camera != null) Update(camera);
        }

        public void LateUpdate() {
            if (camera != null) LateUpdate(camera);
        }

        protected abstract void Update(Camera camera);
        protected abstract void LateUpdate(Camera camera);  

        public bool IsActifCameraController(CameraController controller) {
            if (current == null) return false;
            return current.Equals(controller);
        }

        public static void SetCursorVisibility(bool isVisible)
        {
            Cursor.lockState = isVisible ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = isVisible;
        }

        public static void SetCurrent(int index) {
            SetCurrent(GetModule(index));
        }

        public static void SetCurrent(string moduleName) {
            SetCurrent(GetModule(moduleName));
        }


        public static void SetCurrent(CameraController module)
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

        public static CameraController GetModule(int index) 
        {
            if(modules == null || index < 0 || index >= modules.Count) return null;
            return modules[index];
        }

        public static CameraController GetModule(string moduleName) 
        {
            if (modules == null || string.IsNullOrEmpty(moduleName)) return null;
            
            var results = modules.Where(x => x != null)
                                 .Where(x => !string.IsNullOrEmpty(x.cameraName))
                                 .Where(x => x.cameraName.ToLower().Equals(moduleName.ToLower()));

            if (results.Count() > 0) return results.First();
            return null;
        }
    }

    public abstract partial class CameraController {
#if UNITY_EDITOR
        [SerializeField] private bool showCameraFoldout;
        [SerializeField, HideInInspector] private bool showBaseSettings;

        public void DrawInspector(Color foldoutColor, Color buttonColor, Color backgroundColor) {

            if (EditorExtension.DisplayFoldout("Camera Controller", ref showCameraFoldout, foldoutColor)) {
                EditorExtension.DrawVerticalHelpBox(() => {
                    if (EditorExtension.DisplayFoldout("Base Settings", ref showBaseSettings, foldoutColor)) {
                        ShowBaseSettings(foldoutColor, buttonColor, backgroundColor);
                    }
                }, backgroundColor, true);
            }
        }

        protected virtual void ShowBaseSettings(Color foldoutColor, Color buttonColor, Color backgroundColor) {
            EditorExtension.Space(10f);
            enabled = EditorExtension.DisplayToggle("Enable ", enabled);
            camera  = EditorExtension.DisplayCustomField("Camera ", true, camera);
        }
#endif
    }
}
