using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleRemainderHandler : MonoBehaviour
{
    public float delay = 4.5f;
    public float fadeInTime = 2.0f;

    void Start()
    {
        CanvasGroup canvas = gameObject.GetComponent<CanvasGroup>();
        LeanTween.alphaCanvas(canvas, 1f, fadeInTime).setEase(LeanTweenType.easeInQuad).setDelay(delay);
    }

}
