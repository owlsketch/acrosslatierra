using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanelHandler : MonoBehaviour
{
    public Transform playerCamera;
    public bool follow;

    // best to make fadeInDelay >= fadeOutDuration to avoid having one panel
    // appearing before another has dissapeared
    public float fadeInDelay = 1f;
    public float fadeInDuration = 1f;
    public float fadeOutDelay = 0f;
    public float fadeOutDuration = 1f;

    private Vector3 prevPosition;
    private float yDifference;
    
    void Start() {
        transform.rotation = Quaternion.LookRotation(transform.position - playerCamera.position);
        
        yDifference = .3125f;
        if (follow) prevPosition = playerCamera.position;
    }
    
    private void FixedUpdate()
    {
        if (follow)
        {
            Vector3 delta = playerCamera.position - prevPosition;
            transform.rotation = Quaternion.LookRotation(transform.position - playerCamera.position);
            transform.position = new Vector3(transform.position.x + delta.x, playerCamera.position.y + yDifference, transform.position.z + delta.z);
            prevPosition = playerCamera.position;
        } else
        {
            transform.rotation = Quaternion.LookRotation(transform.position - playerCamera.position);
            transform.position = new Vector3(transform.position.x, playerCamera.position.y + yDifference, transform.position.z);
        }
    }

    // When fading in UI, position is relative to player's
    public void FadeIn()
    {
        gameObject.SetActive(true);
        updatePositionAndRotation();

        CanvasGroup canvGroup = gameObject.GetComponent<CanvasGroup>();
        LeanTween.alphaCanvas(canvGroup, 1f, fadeInDuration).setEase(LeanTweenType.easeInQuad).setDelay(fadeInDelay);
    }

    private void updatePositionAndRotation()
    {
        Vector3 playerPos = playerCamera.position;
        Vector3 playerDirection = playerCamera.forward;
        Quaternion playerRotation = playerCamera.rotation;

        float distance = 1;
        yDifference = gameObject.transform.position.y - playerPos.y;

        Vector3 newPos = playerPos + playerDirection * distance;
        newPos.Set(newPos.x, playerPos.y + yDifference, newPos.z);

        gameObject.transform.SetPositionAndRotation(newPos, playerRotation);
    }
    

    public void FadeOut()
    {
        // TODO: While this fadeout is occuring, we need to somehow disable the input listener that triggers
        // this event from triggering it again. Nothing breaks, but the alpha is reset mid tween. Not good.
        // maybe set an event listener on the tween itself to ensure only one is running at any given moment.
        var canvGroup = GetComponent<CanvasGroup>();
        LeanTween.alphaCanvas(canvGroup, 0f, fadeOutDuration).setEase(LeanTweenType.easeInQuad).setDelay(fadeOutDelay);
        LeanTween.delayedCall(gameObject, fadeOutDuration + fadeOutDelay + 1f, ()=> { gameObject.SetActive(false); });
    }
    
}
