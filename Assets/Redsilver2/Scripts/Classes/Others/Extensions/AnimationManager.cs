using RedSilver2.Framework.Animations;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public static class AnimationManager {
    private static readonly Dictionary<Animator, string> currentAnimations = new Dictionary<Animator, string>();

    private static void ResetAnimationTimestampEvents(AnimationTimestampEvent[] timestampEvents)
    {
        if (timestampEvents != null)
        {
            foreach (var timestampEvent in timestampEvents)
                timestampEvent?.Reset();
        }
    }

    private static void TriggerAnimationTimestampEvents(AnimationTimestampEvent[] timestampEvents, float timeElapsed)
    {
        if (timestampEvents != null)
        {
            foreach (var timestampEvent in timestampEvents) {
                if (timestampEvent == null || timestampEvent.WasTriggered) continue;
                timestampEvent?.Trigger(timeElapsed);
            }
        }
    }

    public static async Awaitable PlayAnimationAsync(this Animator animator, AnimationData data)
    {
        if (data == null) return;
        await PlayAnimationAsync(animator, GetClip(animator, data.AnimationName), () => { data?.Start(); }, () => { data?.Finish(); }, data.TimestampEvents);
    }
    public static async Awaitable PlayAnimationAsync(this Animator animator, AnimationClip clip, AnimationTimestampEvent[] timestampEvents)
    {
        await PlayAnimationAsync(animator, clip, null, null, timestampEvents);
    }
    public static async Awaitable PlayAnimationAsync(this Animator animator, AnimationClip clip, UnityAction onStarted, UnityAction onFinished)
    {
        await PlayAnimationAsync(animator, clip, onStarted, onFinished);
    }
    public static async Awaitable PlayAnimationAsync(this Animator animator, AnimationClip clip, UnityAction action, bool isOnStartedAction)
    {
        if (isOnStartedAction) await PlayAnimationAsync(animator, clip, action, null);
        else await PlayAnimationAsync(animator, clip, null, action);
    }
    public static async Awaitable PlayAnimationAsync(this Animator animator, AnimationClip clip, UnityAction action, AnimationTimestampEvent[] timestampEvents, bool isOnStartedAction)
    {
        if (isOnStartedAction) await PlayAnimationAsync(animator, clip, action, null, timestampEvents);
        else await PlayAnimationAsync(animator, clip, null, action, timestampEvents);
    }
    public static async Awaitable PlayAnimationAsync(this Animator animator, AnimationClip clip, UnityAction onStarted, UnityAction onFinished, AnimationTimestampEvent[] timestampEvents)
    {
        if (clip == null || !ContainsClip(animator, clip) || IsCurrentClipPlaying(animator, clip)) return;
        animator?.Play(clip.name);
        await AwaitPlayAnimation(animator, clip, onStarted, onFinished, timestampEvents, 0f);
    }


    public static void PlayAnimation(this Animator animator, AnimationData data)
    {
        if(data == null) return;
        PlayAnimation(animator, GetClip(animator, data.AnimationName), () => { data?.Start(); }, () => { data?.Finish(); }, data.TimestampEvents);
    }
    public static void PlayAnimation(this Animator animator, AnimationClip clip, AnimationTimestampEvent[] timestampEvents)
    {
        PlayAnimation(animator, clip, null, null, timestampEvents);
    }
    public static void PlayAnimation(this Animator animator, AnimationClip clip, UnityAction onStarted, UnityAction onFinished) {
        PlayAnimation(animator, clip, onStarted, onFinished);
    }
    public static void PlayAnimation(this Animator animator, AnimationClip clip, UnityAction action, bool isOnStartedAction)
    {
        if (isOnStartedAction) PlayAnimation(animator, clip, action, null);
        else PlayAnimation(animator, clip, null, action);
    }
    public static void PlayAnimation(this Animator animator, AnimationClip clip, UnityAction action, AnimationTimestampEvent[] timestampEvents, bool isOnStartedAction)
    {
        if (isOnStartedAction) PlayAnimation(animator, clip, action, null, timestampEvents);
        else PlayAnimation(animator, clip, null, action, timestampEvents);
    }
    public static async void PlayAnimation(this Animator animator, AnimationClip clip, UnityAction onStarted, UnityAction onFinished, AnimationTimestampEvent[] timestampEvents) {
        if (clip == null || !ContainsClip(animator, clip) || IsCurrentClipPlaying(animator, clip)) return;
        animator?.Play(clip.name);
        await AwaitPlayAnimation(animator, clip, onStarted, onFinished, timestampEvents, 0f);
    }

    public static async Awaitable CrossFadeAnimationAsync(this Animator animator, AnimationData data)
    {
        if (data == null) return;
        await CrossFadeAnimationAsync(animator, GetClip(animator, data.AnimationName), () => { data?.Start(); }, () => { data?.Finish(); }, data.TimestampEvents, data.CrossFadeTime);
    }
    public static async Awaitable CrossFadeAnimationAsync(this Animator animator, AnimationClip clip, float crossFadeTime)
    {
        await CrossFadeAnimationAsync(animator, clip, null, null, null, crossFadeTime);
    }
    public static async Awaitable CrossFadeAnimationAsync(this Animator animator, AnimationClip clip, AnimationTimestampEvent[] timestampEvents, float crossFadeTime)
    {
        await CrossFadeAnimationAsync(animator, clip, null, null, timestampEvents, crossFadeTime);
    }
    public static async Awaitable CrossFadeAnimationAsync(this Animator animator, AnimationClip clip, UnityAction onStarted, UnityAction onFinished, float crossFadeTime)
    {
        await CrossFadeAnimationAsync(animator, clip, onStarted, onFinished, crossFadeTime);
    }
    public static async Awaitable CrossFadeAnimationAsync(this Animator animator, AnimationClip clip, UnityAction action, bool isOnStartedAction, float crossFadeTime)
    {
        if (isOnStartedAction) await CrossFadeAnimationAsync(animator, clip, action, null, crossFadeTime);
        else await CrossFadeAnimationAsync(animator, clip, null, action, crossFadeTime);
    }
    public static async Awaitable CrossFadeAnimationAsync(this Animator animator, AnimationClip clip, UnityAction action, AnimationTimestampEvent[] timestampEvents, bool isOnStartedAction, float crossFadeTime)
    {
        if (isOnStartedAction) await CrossFadeAnimationAsync(animator, clip, action, null, timestampEvents, crossFadeTime);
        else await CrossFadeAnimationAsync(animator, clip, null, action, timestampEvents, crossFadeTime);
    }
    public static async Awaitable CrossFadeAnimationAsync(this Animator animator, AnimationClip clip, UnityAction onStarted, UnityAction onFinished, AnimationTimestampEvent[] timestampEvents, float crossFadeTime)
    {
        if (clip == null || !ContainsClip(animator, clip) || IsCurrentClipPlaying(animator, clip)) return;
        float t = 0f;

        crossFadeTime = Mathf.Clamp(crossFadeTime, 0f, float.MaxValue);
        animator?.CrossFade(clip.name, crossFadeTime);

        while (t < crossFadeTime)
        {
            t += Time.deltaTime;
            await Awaitable.NextFrameAsync();
        }

        await AwaitPlayAnimation(animator, clip, onStarted, onFinished, timestampEvents, 0f);
    }

    public static void CrossFadeAnimation(this Animator animator, AnimationData data)
    {
        if (data == null) return;
        CrossFadeAnimation(animator, GetClip(animator, data.AnimationName), () => { data?.Start(); }, () => { data?.Finish(); }, data.TimestampEvents, data.CrossFadeTime);
    }
    public static void CrossFadeAnimation(this Animator animator, string animationName, float crossFadeTime)
    {
        CrossFadeAnimation(animator, GetClip(animator, animationName), crossFadeTime);
    }
    public static void CrossFadeAnimation(this Animator animator, AnimationClip clip, float crossFadeTime)
    {
        CrossFadeAnimation(animator, clip, null, null, null, crossFadeTime);
    }
    public static void CrossFadeAnimation(this Animator animator, AnimationClip clip, AnimationTimestampEvent[] timestampEvents, float crossFadeTime)
    {
        CrossFadeAnimation(animator, clip, null, null, timestampEvents, crossFadeTime);
    }
    public static void CrossFadeAnimation(this Animator animator, AnimationClip clip, UnityAction onStarted, UnityAction onFinished, float crossFadeTime)
    {
        CrossFadeAnimation(animator, clip, onStarted, onFinished, crossFadeTime);
    }
    public static void CrossFadeAnimation(this Animator animator, AnimationClip clip, UnityAction action, bool isOnStartedAction, float crossFadeTime)
    {
        if (isOnStartedAction) CrossFadeAnimation(animator, clip, action, null, crossFadeTime);
        else CrossFadeAnimation(animator, clip, null, action, crossFadeTime);
    }
    public static void CrossFadeAnimation(this Animator animator, AnimationClip clip, UnityAction action, AnimationTimestampEvent[] timestampEvents, bool isOnStartedAction, float crossFadeTime)
    {
        if (isOnStartedAction) CrossFadeAnimation(animator, clip, action, null, timestampEvents, crossFadeTime);
        else CrossFadeAnimation(animator, clip, null, action, timestampEvents, crossFadeTime);
    }
    public static async void CrossFadeAnimation(this Animator animator, AnimationClip clip, UnityAction onStarted, UnityAction onFinished, AnimationTimestampEvent[] timestampEvents, float crossFadeTime)
    {
        if (clip == null || !ContainsClip(animator, clip) || IsCurrentClipPlaying(animator, clip)) return;

        crossFadeTime = Mathf.Clamp(crossFadeTime, 0f, float.MaxValue);
        animator?.CrossFadeInFixedTime(clip.name, crossFadeTime);

        await AwaitPlayAnimation(animator, clip, onStarted, onFinished, timestampEvents, crossFadeTime);
    }



    private static async Awaitable AwaitPlayAnimation(this Animator animator, AnimationClip clip, UnityAction onStarted, UnityAction onFinished, AnimationTimestampEvent[] timestampEvents, float crossFadeTime) {
        float t = 0f;
        crossFadeTime = Mathf.Clamp(crossFadeTime, 0f, float.MaxValue);

        onStarted?.Invoke();
        SetCurrentClipPlaying(animator, clip);

        while (t < crossFadeTime) {
            t += Time.deltaTime;
            await Awaitable.NextFrameAsync();
        }

        ResetAnimationTimestampEvents(timestampEvents);
        await AwaitPlayAnimation(animator, clip, timestampEvents);

        if (IsCurrentClipPlaying(animator, clip.name)) {
            onFinished?.Invoke();
            SetCurrentClipPlaying(animator, string.Empty);
        }
    }

    private static async Awaitable AwaitPlayAnimation(this Animator animator, AnimationClip clip, AnimationTimestampEvent[] timestampEvents)
    {
        await AwaitPlayAnimation(animator, clip != null ? clip.name : string.Empty, timestampEvents);
    }

    private static async Awaitable AwaitPlayAnimation(this Animator animator, string name, AnimationTimestampEvent[] timestampEvents) {
        float t = 0f;
        AnimationClip clip = GetClip(animator, name);
        
        while (clip != null) {
            TriggerAnimationTimestampEvents(timestampEvents, t);

            if (t >= clip.length || !IsCurrentClipPlaying(animator, name)) break; 
            t += Time.deltaTime;
            await Awaitable.NextFrameAsync();
        }
    }

    public static string GetCurrentClipPlayingName(this Animator animator)
    {
        if(currentAnimations == null || animator == null || !currentAnimations.ContainsKey(animator)) 
            return string.Empty;

        return currentAnimations[animator];
    }

    public static AnimationClip GetCurrentClipPlaying(this Animator animator)
    {
        string name = GetCurrentClipPlayingName(animator);
        if(string.IsNullOrEmpty(name)) return null;

        return GetClip(animator, name);
    }


    public static float GetCurrentClipPlayingLenght(this Animator animator)
    {
        AnimationClip clip = GetCurrentClipPlaying(animator);
        return clip == null ? 0f :  clip.length;
    }

    private static void SetCurrentClipPlaying(this Animator animator, AnimationClip clip) {
        SetCurrentClipPlaying(animator, clip != null ? clip.name : string.Empty);
    }
    private static void SetCurrentClipPlaying(this Animator animator, string name) {
        if (currentAnimations == null || animator == null) return;
        else if (!currentAnimations.ContainsKey(animator)) currentAnimations?.Add(animator, string.Empty);
        currentAnimations[animator] = name;
    }

    public static bool IsCurrentClipPlaying(this Animator animator)  {
        string clipName = GetCurrentClipPlayingName(animator);
        if (string.IsNullOrEmpty(clipName)) return false;
        return clipName != string.Empty;
    }
    public static bool IsCurrentClipPlaying(this Animator animator, AnimationClip clip)
    {
        return IsCurrentClipPlaying(animator, clip != null ? clip.name : string.Empty);
    }
    public static bool IsCurrentClipPlaying(this Animator animator, string name) {
        if (currentAnimations == null || animator == null || string.IsNullOrEmpty(name)) return false;
        else if (currentAnimations.ContainsKey(animator)) {
            string clipName = currentAnimations[animator];
           
            if(string.IsNullOrEmpty(clipName) || string.IsNullOrEmpty(clipName)) return false;
            else return clipName.ToLower() == name.ToLower();
        } 
        else return false;
    }

    public static bool IsPlaying(this Animator animator, AudioClip clip) {
        return IsPlaying(animator, clip != null ? clip.name : string.Empty);
    }
    public static bool IsPlaying(this Animator animator, string name) {
        if (animator == null || string.IsNullOrEmpty(name)) return false;
        name = name.ToLower();

        foreach (string clipName in GetClipNames(animator)) {
            if (name != clipName.ToLower()) continue;

            for (int i = 0; i < animator.layerCount; i++) {
                Debug.Log(clipName + " - " + animator.GetCurrentAnimatorStateInfo(i).IsName(clipName));

                if (animator.GetCurrentAnimatorStateInfo(i).IsName(clipName))
                    return true;
            }
        }

        return false;
    }

    public static bool ContainsClip(this Animator animator, AnimationClip clip) {
        if(animator == null) return false;
        return ContainsClip(animator.runtimeAnimatorController, clip != null ? clip.name : string.Empty);
    }
    public static bool ContainsClip(this RuntimeAnimatorController controller, AnimationClip clip)
    {
        return ContainsClip(controller, clip != null ? clip.name : string.Empty);
    }


    public static bool ContainsClip(this Animator animator, string name) {
        if (animator == null) return false;
        return ContainsClip(animator.runtimeAnimatorController, name);
    }

    public static bool ContainsClip(this RuntimeAnimatorController controller, string name)
    {
        if (string.IsNullOrEmpty(name)) return false;
        name = name.ToLower();

        foreach (string clipName in GetClipNames(controller))
            if (name == clipName.ToLower()) return true;

        return false;
    }

    public static float GetClipLenght(this Animator animator, string name)
    {
        if (animator == null) return 0f;
        return GetClipLenght(animator.runtimeAnimatorController, name);
    }

    public static float GetClipLenght(this RuntimeAnimatorController controller, string name)
    {
        AnimationClip clip = GetClip(controller, name);
        return clip != null ? clip.length : 0f;
    }

    public static string GetClipName(this Animator animator, int index)
    {
        if (animator == null) return string.Empty;
        return GetClipName(animator.runtimeAnimatorController, index);
    }

    public static string GetClipName(this RuntimeAnimatorController controller, int index)
    {
        string[] names = GetClipNames(controller);
        if(index < 0 || index >= names.Length) return string.Empty;
        return names[index];

    }

    public static string[] GetClipNames(this Animator animator) {
        if(animator == null) return new string[0];
        return GetClipNames(animator.runtimeAnimatorController);
    }

    public static string[] GetClipNames(this RuntimeAnimatorController controller)
    {
        AnimationClip[] clips = GetClips(controller);
        List<string> results = new List<string>();

        foreach (AnimationClip clip in clips)
            results?.Add(clip == null ? string.Empty : clip.name);

        return results.ToArray();
    }

    public static AnimationClip GetClip(this Animator animator, string name) {
        if(animator == null) return null;
        return GetClip(animator.runtimeAnimatorController, name);
    }

    public static AnimationClip GetClip(this RuntimeAnimatorController controller, string name)
    {
        AnimationClip[] clips = GetClips(controller);
        if (string.IsNullOrEmpty(name)) return null;

        name = name.ToLower();

        foreach (AnimationClip clip in clips) {
            if (clip == null || clip.name.ToLower() != name) continue;
            return clip;
        }

        return null;
    }

    public static AnimationClip[] GetClips(this Animator animator, List<string> names)
    {
        return GetClips(animator, names != null ? names.ToArray() : null);
    }

    public static AnimationClip[] GetClips(this Animator animator, string[] names) {
        if(animator == null) return new AnimationClip[0];
        return GetClips(animator.runtimeAnimatorController, names);
    }

    public static AnimationClip[] GetClips(this RuntimeAnimatorController controller, List<string> names)
    {
        return GetClips(controller, names != null ? names.ToArray() : null);
    }

    public static AnimationClip[] GetClips(this RuntimeAnimatorController controller, string[] names) {
        List<AnimationClip> results = new List<AnimationClip>();
        if (names == null) return results.ToArray();

        foreach (string name in names)
        {
            AnimationClip clip = GetClip(controller, name);
            if (clip == null) continue;

            results?.Add(clip);
        }

        return results.ToArray();
    }

    public static AnimationClip[] GetClips(this Animator animator) {
        if (animator == null) return new AnimationClip[0];
        return GetClips(animator.runtimeAnimatorController);
    }

    public static AnimationClip[] GetClips(this RuntimeAnimatorController controller) {
        if(controller == null) return new AnimationClip[0];
        return controller.animationClips;
    }

}
