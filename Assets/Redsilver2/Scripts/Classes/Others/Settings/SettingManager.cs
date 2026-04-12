using System;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Settings
{
    public partial class SettingManager : MonoBehaviour
    {
        private readonly static UnityEvent<SettingManager> onAwakeHook = new UnityEvent<SettingManager>();

        private const string SENSITIVITY_Y_SETTING = " SENSITIVITY Y";
        private const string SENSITIVITY_X_SETTING = " SENSITIVITY Y";

        // public static readonly Resolution[] screenResolutions = Screen.resolutions.Distinct().Reverse().ToArray();

        private void Awake() {
            data = Load();
            onAwakeHook.Invoke(this);
        }

        public static void AddOnAwakeHookListener(UnityAction<SettingManager> action)
        {
            if (onAwakeHook != null)
                onAwakeHook.AddListener(action);
        }


        public static void RemoveOnAwakeHookListener(UnityAction<SettingManager> action)
        {
            if (onAwakeHook != null)
                onAwakeHook.RemoveListener(action);
        }


        #region Screen

        public static void SetScreenResolution(int index)
        {
            //if(index >= 0 &&  index < screenResolutions.Length) {
            //    Resolution resolution = screenResolutions[index];
            //    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            //}
        }

        public static void SetScreenResolution(Resolution resolution) {
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public static void SetFullScreenMode(FullScreenMode fullScreenMode) {
            Screen.fullScreenMode = fullScreenMode;
        }
        #endregion

        #region Shadows
        public static readonly ShadowQuality[] shadowQualities = (ShadowQuality[])Enum.GetValues(typeof(ShadowQuality));
        public static readonly ShadowProjection[] shadowProjections = (ShadowProjection[])Enum.GetValues(typeof(ShadowProjection));
        public static readonly ShadowResolution[] shadowResolutions = (ShadowResolution[])Enum.GetValues(typeof(ShadowResolution));

        public static void SetShadowQuality(int index)
        {
            if (index >= 0 && index < shadowQualities.Length)
                SetShadowQuality(shadowQualities[index]);
        }
        public static void SetShadowQuality(ShadowQuality shadowQuality)
        {
            QualitySettings.shadows = shadowQuality;
        }

        public static void SetShadowProjection(int index)
        {
            if (index >= 0 && index < shadowProjections.Length)
                SetShadowProjection(shadowProjections[index]);
        }
        public static void SetShadowProjection(ShadowProjection shadowProjection)
        {
            QualitySettings.shadowProjection = shadowProjection;
        }

        public static void SetShadowResolution(int index)
        {
            if (index >= 0 && index < shadowResolutions.Length)
                SetShadowResolution(shadowResolutions[index]);
        }
        public static void SetShadowResolution(ShadowResolution shadowResolution)
        {
            QualitySettings.shadowResolution = shadowResolution;
        }
        #endregion

        #region Sensivity
        public static float GetSensivitityY()
        {
            return 5;
        }

        public static float GetSensitivityX() {
            return 5f;
        }


        public static void SetSensivitityY(float value)
        {
            SetSensitivity(SENSITIVITY_Y_SETTING, value);
        }

        public static void SetSensitivityX(float value)
        {
            SetSensitivity(SENSITIVITY_X_SETTING, value);
        }

        private static void SetSensitivity(string settingName, float value)
        {
            // Do Something here
        }


        #endregion
    }
}


