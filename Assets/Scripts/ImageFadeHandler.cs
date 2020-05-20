using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageFadeHandler : MonoBehaviour
{
    void Start()
    {
        RectTransform img = gameObject.GetComponent<RectTransform>();

        LeanTween.alpha(img, 1f, 1.5f).setEase(LeanTweenType.easeInQuad).setDelay(2.75f);
    }

}
