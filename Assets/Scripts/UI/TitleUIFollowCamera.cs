using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUIFollowCamera : MonoBehaviour
{
    public Transform playerCamera;
    public float distance = 10;
    public float delayTime = 1.0f;

    private Vector3 playerPos;
    private Vector3 playerDirection;
    private Quaternion playerRotation;

    private Vector3 prevDirection;
    private Vector3 currDirection;

    private bool delayActive = false;
    
    void Start()
    {
        playerPos = playerCamera.position;
        // indicates where UI should be placed (in front of player)
        playerDirection = playerCamera.forward;
        // indicates orientation of UI (same as that of player)
        playerRotation = playerCamera.rotation; 

        Vector3 newPos = playerPos + playerDirection * distance;
        gameObject.transform.SetPositionAndRotation(newPos, playerRotation);

        prevDirection = playerDirection;
    }
    
    void Update()
    {
        currDirection = playerCamera.forward;
        Vector3 diff = prevDirection - currDirection;

        if ((Mathf.Abs(diff.x) > 0.2 || Mathf.Abs(diff.y) > 0.2 || Mathf.Abs(diff.z) > 0.2) && delayActive == false)
        {
            StartCoroutine(DelayThenTween());
        }
    }

    IEnumerator DelayThenTween()
    {
        delayActive = true;
        yield return new WaitForSeconds(delayTime);

        // make sure the two direction still aren't equal (there's somewhere to tween to)
        currDirection = playerCamera.forward;
        Vector3 diff = prevDirection - currDirection;

        if (Mathf.Abs(diff.x) > 0.2 || Mathf.Abs(diff.y) > 0.2 || Mathf.Abs(diff.z) > 0.2) {
            // only want to be updating when the difference in position has occured for a while
            playerPos = playerCamera.position;
            playerDirection = currDirection;
            playerRotation = playerCamera.rotation;

            Vector3 newPos = playerPos + playerDirection * distance;
            LeanTween.move(gameObject, newPos, 1.0f).setEase(LeanTweenType.easeInOutCubic);
            LeanTween.rotate(gameObject, playerRotation.eulerAngles, 1.0f).setEase(LeanTweenType.easeInOutCubic);

            prevDirection = playerCamera.forward;
        }

        delayActive = false;
    }

}
 