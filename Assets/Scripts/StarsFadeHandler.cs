using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StarsFadeHandler : MonoBehaviour
{
    void Start()
    {
        foreach (Transform child in transform)
        {
            RectTransform star = child.gameObject.GetComponent<RectTransform>();

            LeanTween.alpha(star, 1f, 1.5f).setEase(LeanTweenType.easeInQuad).setDelay(Random.Range(0.0f, 1.5f));
        }
    }
}
