using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFadeHandler : MonoBehaviour
{
    public float duration = 0.4f;

    public void FadeIn()
    {
        var canvGroup = GetComponent<CanvasGroup>();
        StartCoroutine(ExecuteFade(canvGroup, canvGroup.alpha, 1));
    }

    public void FadeOut()
    {
        var canvGroup = GetComponent<CanvasGroup>();
        StartCoroutine(ExecuteFade(canvGroup, canvGroup.alpha, 0));
    }


    public IEnumerator ExecuteFade(CanvasGroup group, float start, float end)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, end, timeElapsed / duration);

            yield return null;
        }

    }
}
