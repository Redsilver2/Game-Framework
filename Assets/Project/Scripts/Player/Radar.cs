using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Radar : MonoBehaviour {

    [SerializeField] private Camera _camera;
    [SerializeField] private Image  radarBackground;

    private uint radarUses;
    private bool isActivated;
    private bool isWaitDelayFinished;

    private IEnumerator openCoroutine;

    private const string CAMERA_MODULE_NAME = "RADAR_CAMERA";

    private void Awake() {
        radarUses = 3;
        openCoroutine = OpenCoroutine();
        SetCameraState(false);
    }

    private void Update()
    {
        UpdateRadarInput();
    }

    private void UpdateRadarInput()
    {
        if (InputManager.GetKeyDown(KeyboardKey.R)) {
            if ((radarUses == 0 && !isActivated) || !isWaitDelayFinished) {
                return;
            }

            SetIsActivated(!isActivated, 0f);
            if (isActivated) radarUses--;
        }
    }

    public void SetIsActivated(bool isActivated, float waitDelay)
    {
        this.isActivated = isActivated;
       
        if (isActivated) {
            Enable();
        }
        else {
            Disable(waitDelay);
        }
    }

    private void Enable()
    {
        CameraControllerModule.Disable();
        PlayerController.Disable();

        StartCoroutine(openCoroutine);  
    }

    public void Disable(float waitDelay) {

        StopCoroutine(openCoroutine);
        StartCoroutine(WaitDelayCoroutine(waitDelay));

        SetCameraState(false);

        CameraControllerModule.SetCurrent("PLAYER_CAMERA");
        PlayerController.Enable();
    }

    private void SetCameraState(bool isEnabled)
    {
        if (_camera != null) _camera.enabled = isEnabled;
    }

    private void SetCameraFieldOfView(float fieldOfView)
    {
        if (_camera != null) _camera.fieldOfView = fieldOfView;
    }

    private void SetRadarBackgroundFillValue(float value)
    {
        value = Mathf.Clamp01(value);
        if (radarBackground != null) radarBackground.fillAmount = value;
    }

    private IEnumerator WaitDelayCoroutine(float seconds)
    {
        if (seconds > 0f) {
            isWaitDelayFinished = false;
            yield return StartCoroutine(ProgressiveCoroutine(seconds, null));
        }

        isWaitDelayFinished = true;
    }

    private IEnumerator OpenCoroutine()
    {
        yield return StartCoroutine(ProgressiveCoroutine(2.5f, value => { SetRadarBackgroundFillValue(value); }));

        SetCameraState(true);
        CameraControllerModule.SetCurrent(CAMERA_MODULE_NAME);

        yield return StartCoroutine(ProgressiveCoroutine(1f, value => { SetCameraFieldOfView(Mathf.Lerp(10f, 70f, value)); }));
    }

    private IEnumerator ProgressiveCoroutine(float transitionTime, UnityAction<float> action)
    {
        float t = 0f;

        action?.Invoke(0f);

        while (t < transitionTime){
            t += Time.deltaTime;
            action?.Invoke(t / transitionTime);
            yield return null;
        }

        action?.Invoke(1f);
    }
   


}
