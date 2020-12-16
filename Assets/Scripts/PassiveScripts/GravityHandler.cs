using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityHandler : MonoBehaviour 
{
    Vector3 upAxis;
    Rigidbody rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        upAxis = (new Vector3(0, 0, 0) - gameObject.transform.position).normalized;
        upAxis = new Vector3(upAxis.x, upAxis.y, 0);
        rb.AddForce(upAxis * Physics.gravity.y * rb.mass);
    }
}
