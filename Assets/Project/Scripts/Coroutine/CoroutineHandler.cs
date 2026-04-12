using System.Collections;
using UnityEngine;

public static class CoroutineHandler
{
    public static void StartCoroutine(MonoBehaviour mono, ref IEnumerator current, IEnumerator newCoroutine)
    {
        StopCoroutine(mono, ref current);
        current = newCoroutine;
        if (current != null) mono?.StartCoroutine(current);
    }


    public static void StopCoroutine(MonoBehaviour  mono, ref IEnumerator current)
    {
        if(current != null) mono?.StopCoroutine(current);    
        current = null;
    }
}
