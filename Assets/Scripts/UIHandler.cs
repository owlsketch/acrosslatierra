using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public Transform playerCamera;
    public float fadeDuration = 0.4f;
    public int delay = 0;

    // UI will always be pointing towards the user
    void Start() => transform.rotation = Quaternion.LookRotation(transform.position - playerCamera.position);
    void Update() => transform.rotation = Quaternion.LookRotation(transform.position - playerCamera.position);

    // When fading in UI, position is relative to player's
    public void FadeIn()
    {
        gameObject.SetActive(true);
        StartCoroutine(Delay());
    }
       

    IEnumerator Delay ()
    {
        yield return new WaitForSeconds(delay);

        Vector3 playerPos = playerCamera.position;
        Vector3 playerDirection = playerCamera.forward;
        Quaternion playerRotation = playerCamera.rotation;
        float distance = 1;

        Vector3 newPos = playerPos + playerDirection * distance;
        newPos.Set(newPos.x, 1.0f, newPos.z);

        gameObject.transform.SetPositionAndRotation(newPos, playerRotation);

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

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, end, timeElapsed / fadeDuration);

            yield return null;
        }
           
        if (group.alpha < 0.01)
        {
            gameObject.SetActive(false);
        }

    }

}
