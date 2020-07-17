using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TitleStarsHandler : MonoBehaviour
{
    public float spawnRange = 2.5f;
    public float duration = 2.5f;

    void onEnable()
    {
        foreach (Transform child in transform)
        {
            RectTransform star = child.gameObject.GetComponent<RectTransform>();

            LeanTween.alpha(star, 1f, duration).setEase(LeanTweenType.easeInQuad).setDelay(Random.Range(0.0f, spawnRange));
        }
    }
}
