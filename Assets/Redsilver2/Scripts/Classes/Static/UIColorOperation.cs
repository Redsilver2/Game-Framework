using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework
{
    public static class UIColorOperation {
        private static readonly Dictionary<CanvasGroup   , GameTimer> groupOperations    = new Dictionary<CanvasGroup, GameTimer>();
        private static readonly Dictionary<CanvasRenderer, GameTimer> rendererOperations = new Dictionary<CanvasRenderer, GameTimer>();
       
        public static void StopColorOperation(this CanvasRenderer renderer) {
            if(renderer != null && rendererOperations != null) {
                if (rendererOperations.ContainsKey(renderer)) { rendererOperations[renderer]?.Stop(); }
            }
        }

        public static void StopColorOperation(this CanvasGroup group)
        {
            if (group != null && groupOperations != null) {
                if (groupOperations.ContainsKey(group)) { groupOperations[group]?.Stop(); }
            }
        }

        private static async Awaitable StartColorOperationAsync(this CanvasRenderer renderer, float duration, UnityAction<float, float> action) {
            if (renderer != null && rendererOperations != null) {
                renderer?.StopColorOperation();

                if (!rendererOperations.ContainsKey(renderer) || rendererOperations[renderer] == null)
                    rendererOperations[renderer] = new GameTimer();

                await rendererOperations[renderer].StartAsync(duration, action);
            }
        }

        private static async Awaitable StartColorOperationAsync(this CanvasGroup group, float duration, UnityAction<float, float> action)
        {
            if (group != null && groupOperations != null) {
                group?.StopColorOperation();

                if (!groupOperations.ContainsKey(group) || groupOperations[group] == null)
                    groupOperations[group] = new GameTimer();

                await groupOperations[group].StartAsync(duration, action);
            }
        }

        public static async void FadeAlpha(this CanvasRenderer renderer, float alpha, float duration)
        {
           if(renderer != null) await renderer?.FadeAlphaAsync(alpha, duration);
        }

        public static async void FadeAlpha(this CanvasRenderer renderer, bool isVisible, float duration)
        {
            if (renderer != null) await renderer?.FadeAlphaAsync(isVisible, duration);
        }

        public static async Awaitable FadeAlphaAsync(this CanvasRenderer renderer, bool isVisible, float duration)
        {
            if (renderer != null) await renderer?.FadeAlphaAsync(isVisible ? 1f : 0f, duration);
        }

        public static async Awaitable FadeAlphaAsync(this CanvasRenderer renderer, float alpha, float duration)
        {
            alpha = Mathf.Clamp01(alpha);

            if (renderer != null) await renderer?.StartColorOperationAsync(duration, (timeElapsed, progress) =>{
                if (progress >= 0f && progress < 1f) renderer?.SetAlpha(Mathf.Lerp(renderer.GetAlpha(), alpha, progress));
                else renderer?.SetAlpha(alpha);
            });
        }

        public static async void LerpColor(this CanvasRenderer renderer, Color color, float duration)
        {
            if (renderer != null) await renderer?.LerpColorAsync(color, duration);
        }

        public static async Awaitable LerpColorAsync(this CanvasRenderer renderer, Color color, float duration)
        {
            if (renderer != null) await renderer?.StartColorOperationAsync(duration, (timeElapsed, progress) => {
                if (progress >= 0f && progress < 1f) renderer?.SetColor(Color.Lerp(renderer.GetColor(), color, progress));
                else renderer?.SetColor(color);
            });
        }


        public static async void FadeAlpha(this CanvasGroup group, float alpha, float duration)
        {
            if (group != null) await group?.FadeAlphaAsync(alpha, duration);
        }

        public static async void FadeAlpha(this CanvasGroup group, bool isVisible, float duration)
        {
            if (group != null) await group?.FadeAlphaAsync(isVisible, duration);
        }

        public static async Awaitable FadeAlphaAsync(this CanvasGroup group, bool isVisible, float duration)
        {
            if (group != null) await group?.FadeAlphaAsync(isVisible ? 1f : 0f, duration);
        }

        public static async Awaitable FadeAlphaAsync(this CanvasGroup group, float alpha, float duration)
        {
            alpha = Mathf.Clamp01(alpha);

            if (group != null) await group?.StartColorOperationAsync(duration, (timeElapsed, progress) => {
                if(group != null) {
                    if (progress >= 0f && progress < 1f) group.alpha = Mathf.Lerp(group.alpha, alpha, progress);
                    else group.alpha = alpha;
                }
            });
        }
    }
}
