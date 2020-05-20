using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFadeHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RectTransform text = gameObject.GetComponent<RectTransform>();

        LeanTween.alpha(text, 1f, 1.5f).setEase(LeanTweenType.easeInQuad).setDelay(2.75f);
    }
}
