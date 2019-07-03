using System;
using System.Collections;
using UnityEngine;

public class Timeout
{
    public delegate void CallbackDelegate();

    public static void SetTimeout(MonoBehaviour gameObject, CallbackDelegate callback, float delay)
    {
        gameObject.StartCoroutine(wait(delay));
        IEnumerator wait(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            callback();
        }
    }
}
