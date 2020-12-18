// Character movement when gravity isn't uniform

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomContinuousMovement : MonoBehaviour
{
    public XRNode inputSource;
    public float speed = 1.0f;
    public LayerMask groundLayer;
    
    private XRRig rig;
    private Vector2 inputAxis;
    private Vector3 upAxis;
    private float fallingSpeed;
    private bool movementAllowed = true;


    void Start()
    {
        rig = GetComponent<XRRig>();

        // Vector *pointing* towards origin. We remove the z axis because we
        // don't want to be pushed from the origin in the z axis, only in x/y.
        upAxis = (new Vector3(0, 0, 0) - rig.transform.position).normalized;
        upAxis = new Vector3(upAxis.x, upAxis.y, 0);

        rig.MatchRigUp(upAxis);
    }

    public void AllowMovement(bool state) => movementAllowed = state;

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }

    private void FixedUpdate()
    {
        upAxis = (new Vector3(0, 0, 0) - rig.transform.position).normalized;
        upAxis = new Vector3(upAxis.x, upAxis.y, 0);
        rig.MatchRigUp(upAxis);

        // Movement relative to the local forward direction of the camera
        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.localEulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);
        if (movementAllowed)
        {
            transform.Translate(direction * Time.fixedDeltaTime * speed);
        }

        // v(t) = 0 if grounded, else v(t) = v0 + at.
        bool isGrounded = DetermineIfGrounded();
        if (isGrounded)
            fallingSpeed = 0;
        else
            fallingSpeed += Physics.gravity.y * Time.fixedDeltaTime;

        // since up is a vector in world space, gravity needs to be applied in world space
        transform.Translate(rig.transform.up * fallingSpeed * Time.fixedDeltaTime, Space.World);
        
        MaintainAboveGround();
    }

    bool DetermineIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(rig.cameraInRigSpacePos);
        float rayLength = rig.cameraInRigSpaceHeight;

        // the length should really be the height of the character, but if ray is too short, collisions aren't detected, so minimum set
        // likewise we choose .375 because if the user is falling from a very high height, the acceleration may cause the collision to be missed
        if (rayLength < .375f) {
            rayStart = transform.position + rig.transform.up * .375f;
            rayLength = .375f;
        }
        
        bool hasHit = Physics.Raycast(rayStart, -rig.transform.up, out RaycastHit hitInfo, rayLength, groundLayer);
        Debug.DrawRay(rayStart, -rig.transform.up * .375f, Color.magenta);
        

        return hasHit;
    }

    bool MaintainAboveGround()
    {
        Vector3 rayStart = transform.position + rig.transform.up * 1f;
        float rayLength = 1f;

        bool hasHit = Physics.Raycast(rayStart, -rig.transform.up, out RaycastHit hitInfo, rayLength, groundLayer);

        if (hitInfo.distance != 0 && hitInfo.distance < 0.975f)
        {
            //as the player moves around the curve, the raycast slowly sinks, so need to manually push the rig up as well if it sinks too much
            float yDisplacement = 0.975f - hitInfo.distance;
            transform.Translate(rig.transform.up * yDisplacement, Space.World);
        }

        return hasHit;
    }
}
