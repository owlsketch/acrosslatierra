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


    void Start()
    {
        rig = GetComponent<XRRig>();

        upAxis = (new Vector3(0, 0, 0) - rig.transform.position).normalized;
        upAxis = new Vector3(upAxis.x, upAxis.y, 0);
        rig.MatchRigUp(upAxis);
    }

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
       
        transform.Translate(direction * Time.fixedDeltaTime * speed);
        
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
    
    // TODO: Does this even still apply? Regardless, solution without is good enough for now.
    // void CapsuleFollowHeadset() {} 

    bool DetermineIfGrounded()
    {
        // ray starts at camera position + .125 to account for radius of sphere cast
        Vector3 rayStart = transform.TransformPoint(rig.cameraInRigSpacePos) + Quaternion.Euler(rig.transform.up) * new Vector3(0, 0.125f, 0);
        
        // the length should really be the height of the character, but if ray is too short, collisions aren't detected, so minimum set
        // likewise we choose .375 because if the user is falling from a very high height, the acceleration may cause the collision to be missed
        float rayLength = rig.cameraInRigSpaceHeight;
        if (rayLength < .375f) { rayLength = .375f; }

        bool hasHit = Physics.SphereCast(rayStart, .25f, -rig.transform.up, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }

    void MaintainAboveGround()
    {
        Vector3 rayStart = transform.position;
        float rayLength = .1f;

        bool hasHit = Physics.Raycast(rayStart, -rig.transform.up, out RaycastHit hitInfo, rayLength, groundLayer);
        
        if (hitInfo.distance != 0 && hitInfo.distance < 0.1f)
        {
            //as the player moves around the curve, the sphere cast slowly sinks, so need to manually push the rig up as well if it sinks too much
            float yDisplacement = 0.1f - hitInfo.distance;
            transform.Translate(new Vector3(0, yDisplacement, 0));
        }
    }
}
