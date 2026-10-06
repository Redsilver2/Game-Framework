using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UnityExtension
{
    public static async Awaitable<int[]> GetCharsIndexes(this string text, char _char) {
        List<int> results = new List<int>();
        if (string.IsNullOrEmpty(text)) results.ToArray();

        char[] chars = text.ToCharArray();
        await Awaitable.BackgroundThreadAsync();

        for (int i = 0; i < chars.Length; i++)
            if (chars[i] == _char) results?.Add(i);

        await Awaitable.MainThreadAsync();
        return results.ToArray();
    }

    public static bool IsUInt(this string value) {
        return uint.TryParse(value, out uint result);
    }

    public static bool IsInt(this string value)
    {
        return int.TryParse(value, out int result);
    }

    public static bool IsFloat(this string value) {
        return float.TryParse(value, out float result);
    }

    public static bool IsBool(this string value)
    {
        string result = value.ToLower();

        if (result == "t" || result == "f" || result == "true" || result == "false")
            return true;

        return false;
    }


    public static string RemoveWhiteSpaces(this string value)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        return String.Concat(value.Where(x => !char.IsWhiteSpace(x)));
    }

    public static T[] GetEnumValues<T>(T value) where T : Enum {
        if (value == null) return null;
        return (T[])Enum.GetValues(typeof(T)); 
    }

    public static T Instantiate<T>(T value) where T : UnityEngine.Object
    {
        return UnityEngine.Object.Instantiate(value);
    }

    public static float Set(this float current, float value)
    {
        current = value;
        return current;
    }
    public static bool HasReachedTarget(this float current, float target) => current >= target;

    public static void SetAlpha(this CanvasRenderer renderer, float progression, float currentAlpha, float alphaTarget) {
        progression = Mathf.Clamp01(progression);

        if (renderer != null)
        {
            if (progression >= 1f) renderer.SetAlpha(alphaTarget);
            else renderer.SetAlpha(Mathf.Lerp(currentAlpha, alphaTarget, progression));
        }
    }

    public static void SetAlpha(this CanvasRenderer renderer, float progression, float alphaTarget) {
        if (renderer != null) renderer.SetAlpha(progression, renderer.GetAlpha(), alphaTarget);
    }
}
