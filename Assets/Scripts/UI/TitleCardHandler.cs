using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCardHandler : MonoBehaviour
{
    public void FadeIn(int duration)
    {
        CanvasGroup canvGroup = gameObject.GetComponent<CanvasGroup>();
        LeanTween.alphaCanvas(canvGroup, 1f, duration).setEase(LeanTweenType.easeInQuad);
    }

    public void FadeOut(int duration)
    {
        CanvasGroup canvGroup = gameObject.GetComponent<CanvasGroup>();
        LeanTween.alphaCanvas(canvGroup, 0f, duration).setEase(LeanTweenType.easeInQuad);
    }
}
