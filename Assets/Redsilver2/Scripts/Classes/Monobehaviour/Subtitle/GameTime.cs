using RedSilver2.Framework.Subtitles;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework {
    [System.Serializable]
    public class GameTime {
        [SerializeField, HideInInspector] private uint hours;
        [SerializeField, HideInInspector] private uint minutes;
        [SerializeField, HideInInspector] private uint seconds;

        public uint Hours => hours;
        public uint Minutes => minutes;
        public uint Seconds => seconds;

        public GameTime()
        {
            SetSeconds(uint.MinValue);
            SetMinutes(uint.MinValue);
            SetHours  (uint.MinValue);
        }

        public GameTime(uint seconds)
        {
            SetSeconds(seconds);
            SetMinutes(uint.MinValue);
            SetHours(uint.MinValue);
        }

        public GameTime(uint minutes, uint seconds)
        {
            SetSeconds(seconds);
            SetMinutes(minutes);
            SetHours(uint.MinValue);
        }

        public GameTime(uint hours, uint minutes, uint seconds)
        {
            SetSeconds(seconds);
            SetMinutes(minutes);
            SetHours  (hours);
        }

        public void SetHours(uint hours)
        {
            this.hours = (uint)Mathf.Clamp(hours, 0, uint.MaxValue);
        }

        public void SetMinutes(uint minutes)
        {
            this.minutes = (uint)Mathf.Clamp(minutes, 0, 59);
        }

        public void SetSeconds(uint seconds)
        {
            this.seconds = (uint)Mathf.Clamp(seconds, 0, 59);
        }

        public float GetFloatConversion() {
            return GetHoursToSeconds(hours) + GetMinutesToSeconds(minutes) + seconds; 
        }

        public static float GetHoursToSeconds(uint hours) {
            return (uint)Mathf.Clamp(hours, 0, float.MaxValue) * (60 ^ 2);
        }

        public static float GetMinutesToSeconds(uint minutes) {
            return (uint)Mathf.Clamp(minutes, 0, 59) * 60f;
        }

        public sealed override string ToString()
        {
            return (hours   < 10 ? $"0{hours}"    : hours)  + ":" +
                   (minutes < 10 ? $"0{minutes}" : minutes) + ":" +
                   (seconds < 10 ? $"0{seconds}" : seconds);
        }


#if UNITY_EDITOR
        private bool showSettings;

        public void Validate()
        {
            SetSeconds(seconds);
            SetMinutes(minutes);
            SetHours(hours);
        }

        public void DrawInspector(string name) {
            if (EditorExtension.DisplayFoldout(name, ref showSettings, new Color32(0, 0, 0, 181)))
            {
                EditorExtension.DrawVerticalHelpBox(() =>
                {
                    SetHours(EditorExtension.DisplayUIntSlider("Hours", hours, 999));
                    SetMinutes(EditorExtension.DisplayUIntSlider("Minutes", minutes, 59));
                    SetSeconds(EditorExtension.DisplayUIntSlider("Seconds", seconds, 59));

                }, true);
            }
        }
#endif
    }

    public class GameTimer {
        private float timeElapsed;
        private bool isPaused;
        private CancellationTokenSource source;

        public float TimeElapsed => timeElapsed;
        public bool IsPaused => isPaused;
        public bool IsCancelled => source != null ? source.Token.IsCancellationRequested : true;

        public async void Start()
        {
            await StartAsync();
        }

        public void Start(GameTime time)
        {
            Start(time, null);
        }

        public async void Start(GameTime time, UnityAction<float, float> action) {
            Start(0f, time, action);
        }

        public void Start(float startTime, GameTime duration)
        {
            Start(startTime, duration, null);
        }

        public async void Start(float startTime, GameTime duration, UnityAction<float, float> action) {
            await StartAsync(startTime, duration != null ? duration.GetFloatConversion() : 0f, action);
        }

        public void Start(float duration) {
            Start(duration, null as UnityAction<float, float>);
        }

        public async void Start(float duration, UnityAction<float, float> action) {
            await StartAsync(0f, duration, action);
        }

        public async Awaitable StartAsync(GameTime time)
        {
            await StartAsync(time, null);
        }

        public async Awaitable StartAsync(GameTime time, UnityAction<float, float> action)
        {
            await StartAsync(0f, time, action);
        }

        public async Awaitable StartAsync(float startTime, GameTime time)
        {
            await StartAsync(startTime, time, null);
        }

        public async Awaitable StartAsync(float startTime, GameTime time, UnityAction<float, float> action)
        {
            await StartAsync(startTime, time != null ? time.GetFloatConversion() : 0f, action);
        }

        public async Awaitable StartAsync() {
            await StartAsync(float.MaxValue, null as UnityAction<float, float>);
        }

        public async Awaitable StartAsync(float duration)
        {
            await StartAsync(duration, null as UnityAction<float, float>);
        }

        public async Awaitable StartAsync(float startTime, float duration)
        {
            await StartAsync(startTime, duration, null as UnityAction<float, float>);
        }

        public async Awaitable StartAsync(UnityAction<float, float> action)
        {
            await StartAsync(float.MaxValue, action);
        }

        public async Awaitable StartAsync(float duration, UnityAction<float, float> action)
        {
            await StartAsync(0f, duration, action);
        }

        public async Awaitable StartAsync(float startTime, float duration, UnityAction<float, float> action)
        {
            source?.Cancel();
            source = new CancellationTokenSource();
            await StartAsync(startTime, duration, action, source.Token);
        }


        private async Awaitable StartAsync(float startTime, float duration, UnityAction<float, float> action, CancellationToken token)
        {
            timeElapsed = Mathf.Clamp(startTime, 0f, float.MaxValue);
            duration    = Mathf.Clamp(duration, 0f, float.MaxValue);

            while (!token.IsCancellationRequested) {
                if (timeElapsed >= duration) {
                    action?.Invoke(duration, 1f);
                    break;
                }

                action?.Invoke(timeElapsed, Mathf.Clamp01(timeElapsed / duration));
                if(!isPaused) timeElapsed += Time.deltaTime;
                await Awaitable.NextFrameAsync(token);
            }
        }

        public void Pause()  { this.isPaused = true; }
        public void Resume() { this.isPaused = false; }
        public void Stop() {
            this.isPaused = false;
            source?.Cancel();
            source = null;
        }

        public static async void Start(float duration, CancellationToken token)
        {
            await new GameTimer().StartAsync(0f, duration, null, token);
        }

        public static async void Start(float duration, UnityAction<float, float> action, CancellationToken token)
        {
            await new GameTimer().StartAsync(0f, duration, action, token);
        }

        public static async void Start(float startTime, float duration, UnityAction<float, float> action, CancellationToken token)
        {
            await new GameTimer().StartAsync(startTime, duration, action, token);
        }

        public static async void Start(float startTime, float duration, CancellationToken token)
        {
            await new GameTimer().StartAsync(startTime, duration, null, token);
        }

        public static async Awaitable StartAsync(float duration, CancellationToken token)
        {
            await new GameTimer().StartAsync(0f, duration, null, token);
        }

        public static async Awaitable StartAsync(float duration, UnityAction<float, float> action, CancellationToken token)
        {
            await new GameTimer().StartAsync(0f, duration, action, token);
        }

        public static async Awaitable StartAsync(float startTime, float duration, CancellationToken token)
        {
            await new GameTimer().StartAsync(startTime, duration, null, token);
        }
    }
}
