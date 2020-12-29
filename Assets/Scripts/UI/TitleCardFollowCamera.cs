using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCardFollowCamera : MonoBehaviour
{
    public Transform playerCamera;
    public float distance = 50;
    // Make delay time a minimum of 1
    public float delayTime = 1.0f;

    private Vector3 playerPos;
    private Vector3 playerDirection;
    private Quaternion playerRotation;

    private Vector3 prevOrigin;
    private Vector3 currDirection;
    private Vector3 finalDirection;

    private bool waiting = false;
    private bool delayed = false;
    private bool move = false;
    private bool tweening = false;
    
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
        if (SufficientlyDistant() && !waiting) // && !tweening
        {
            //movement logic triggered if far enough to trigger movement, not waiting for a coroutine, and not currently tweening

            if (!delayed) // haven't waited the appropriate delay time
            {
                StartCoroutine(Wait()); // wait the appropriate delay time
            }
            else // waited the appropriate delay time
            {
                if (SufficientlyDistant()) // waited and still far enough
                {
                    /* TODO: Couldn't figure this part out
                    if (!move) // not yet approved to move
                    {
                        // Ensure user is no longer moving before approving move!
                        StartCoroutine(VerifyNotMoving());
                    } else // movement is approved
                    {
                        Tween(); // tween and reset all vars when tweening is complete!
                    }
                    */
                    Tween();
                }
                else // waited but no longer far enough, reset vars!
                {
                    delayed = false;
                }
            }
        }
    }

    // Measures distance from last position of title card to the user's current position
    private bool SufficientlyDistant()
    {
        currDirection = playerCamera.forward;
        Vector3 diff = prevOrigin - currDirection;

        if ((Mathf.Abs(diff.x) > 0.25 || Mathf.Abs(diff.y) > 0.25 || Mathf.Abs(diff.z) > 0.25))
        {
            return true;
        } else
        {
            return false;
        }
    }

    IEnumerator Wait()
    {

        waiting = true;
        yield return new WaitForSeconds(delayTime);

        delayed = true;
        waiting = false;
    }

    IEnumerator VerifyNotMoving()
    {
        finalDirection = playerCamera.forward;
        waiting = true;
        yield return new WaitForSeconds(1);

        waiting = false;
        currDirection = playerCamera.forward;
        Vector3 diff = finalDirection - currDirection;

        if ((Mathf.Abs(diff.x) > 0.25 || Mathf.Abs(diff.y) > 0.25 || Mathf.Abs(diff.z) > 0.25))
        {
            move = false;
        }
        else
        {
            move = true;
        }

    }

    // IEnumerator Tween()
    void Tween()
    {
        playerPos = playerCamera.position;
        playerDirection = currDirection;
        playerRotation = playerCamera.rotation;

        Vector3 newPos = playerPos + playerDirection * distance;
        
        LeanTween.move(gameObject, newPos, 1.0f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.rotate(gameObject, new Vector3(playerRotation.eulerAngles.x, playerRotation.eulerAngles.y, 0), 1.0f)
            .setEase(LeanTweenType.easeInOutCubic);

        // tweening = true;
        // yield return new WaitForSeconds(1);

        delayed = false;
        // move = false;
        // tweening = false;
        prevOrigin = playerCamera.position;
    }

}
 