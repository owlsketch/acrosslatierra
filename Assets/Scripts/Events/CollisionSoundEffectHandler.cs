using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSoundEffectHandler : MonoBehaviour
{

    private AudioSource effect;

    private void Start() => effect = GetComponent<AudioSource>();

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 vel = collision.relativeVelocity;
        if (vel.x > 1.75 || vel.y > 1.75 || vel.z > 1.75)
        {
            if (collision.gameObject.layer == 10) effect.Play(); // hit the ground layer
        }
    }
}
