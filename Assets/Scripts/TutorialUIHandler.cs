using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIHandler : MonoBehaviour
{
    public Transform playerCamera;
    public int delay = 1;
    public float duration = 0.5f;

    // UI will always be pointing towards the user
    void Start() => transform.rotation = Quaternion.LookRotation(transform.position - playerCamera.position);
    void Update() => transform.rotation = Quaternion.LookRotation(transform.position - playerCamera.position);

    // When fading in UI, position is relative to player's
    public void FadeIn()
    {
        gameObject.SetActive(true);
        updatePosition();

        CanvasGroup canvGroup = gameObject.GetComponent<CanvasGroup>();
        LeanTween.alphaCanvas(canvGroup, 1f, duration).setEase(LeanTweenType.easeInQuad).setDelay(delay);
    }

    private void updatePosition()
    {
        Vector3 playerPos = playerCamera.position;
        Vector3 playerDirection = playerCamera.forward;
        Quaternion playerRotation = playerCamera.rotation;
        float distance = 1;

        Vector3 newPos = playerPos + playerDirection * distance;
        newPos.Set(newPos.x, 1.0f, newPos.z);

        gameObject.transform.SetPositionAndRotation(newPos, playerRotation);
    }

    public void FadeOut()
    {
        var canvGroup = GetComponent<CanvasGroup>();
        LeanTween.alphaCanvas(canvGroup, 0f, duration).setEase(LeanTweenType.easeInQuad).setDelay(delay);
        gameObject.SetActive(false);
    }
}
