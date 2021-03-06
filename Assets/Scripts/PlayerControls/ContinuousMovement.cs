﻿// Character movement for uniform gravity

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{
    public XRNode inputSource;
    public float speed = 1.0f;
    public float gravity = -9.81f;
    public LayerMask groundLayer;

    private Vector2 inputAxis;
    private float fallingSpeed;
    private CharacterController character;
    private XRRig rig; 
    private AudioSource footsteps;
    private bool movementAllowed = true;


    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
        footsteps = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    } 

    public void AllowMovement(bool state) => movementAllowed = state;

    private void FixedUpdate()
    {
        CapsuleFollowHeadset();

        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);

        if (movementAllowed)
        {
            if (Mathf.Abs(inputAxis.x) > 0.001 || Mathf.Abs(inputAxis.y) > 0.001)
            {
                if (fallingSpeed > -1)
                {
                    if (!footsteps.isPlaying) footsteps.Play();
                } else
                {
                    if (footsteps.isPlaying) footsteps.Stop();
                }
                
                character.Move(direction * Time.fixedDeltaTime * speed);
            } else
            {
                if (footsteps.isPlaying) footsteps.Stop();
            }
        } else
        {
            if (footsteps.isPlaying) footsteps.Stop();
        }


        bool isGrounded = DetermineIfGrounded();
        // v(t) = 0 if grounded, else v(t) = v0 + at.
        if (isGrounded)
            fallingSpeed = 0;
        else 
            fallingSpeed += gravity * Time.fixedDeltaTime;

        character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }

    void CapsuleFollowHeadset()
    {
        // .2m for forehead
        character.height = rig.cameraInRigSpaceHeight + 0.2f;

        // transform camera pos from world space to local space
        // need this to always place the capsule horizontally at camera pos
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height/2 + character.skinWidth, capsuleCenter.z);
    }

    bool DetermineIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLength = character.center.y + 0.01f;

        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }
}
