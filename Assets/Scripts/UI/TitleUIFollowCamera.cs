using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUIFollowCamera : MonoBehaviour
{
    public Transform playerCamera;
    public float distance = 50;
    public float delayTime = 1.0f;

    private Vector3 playerPos;
    private Vector3 playerDirection;
    private Quaternion playerRotation;

    private Vector3 prevOrigin;
    private Vector3 currDirection;

    private bool delayActive = false;
    private bool delayed = false;
    
    void Start()
    {
        playerPos = playerCamera.position;
        // indicates where UI should be placed (in front of player)
        playerDirection = playerCamera.forward;
        // indicates orientation of UI (same as that of player)
        playerRotation = playerCamera.rotation; 

        Vector3 newPos = playerPos + playerDirection * distance;
        gameObject.transform.SetPositionAndRotation(newPos, playerRotation);

        prevOrigin = playerDirection;
    }
    
    private void FixedUpdate()
    {
        currDirection = playerCamera.forward;
        Vector3 diff = prevOrigin - currDirection;
        
        if ((Mathf.Abs(diff.x) > 0.2 || Mathf.Abs(diff.y) > 0.2 || Mathf.Abs(diff.z) > 0.2))
        {
            if (!delayActive)
            {
                if (!delayed) StartCoroutine(DelayThenTween());
                if (delayed) Tween();
            }
        }
    }
    
    IEnumerator DelayThenTween()
    {

        delayActive = true;
        yield return new WaitForSeconds(delayTime);

        delayed = true;
        delayActive = false;

        // make sure the two direction still aren't equal (there's somewhere to tween to)
        currDirection = playerCamera.forward;
        Vector3 diff = prevOrigin - currDirection;

        if (Mathf.Abs(diff.x) > 0.2 || Mathf.Abs(diff.y) > 0.2 || Mathf.Abs(diff.z) > 0.2) {
            Tween();
        }

    }

    void Tween()
    {
        playerPos = playerCamera.position;
        playerDirection = currDirection;
        playerRotation = playerCamera.rotation;

        Vector3 newPos = playerPos + playerDirection * distance;
        
        LeanTween.move(gameObject, newPos, 1.0f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.rotate(gameObject, new Vector3(playerRotation.eulerAngles.x, playerRotation.eulerAngles.y, 0), 1.0f)
            .setEase(LeanTweenType.easeInOutCubic);

        prevOrigin = playerDirection;
    }

}
 