using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatHandler : MonoBehaviour
{
    public bool canFloat = true;
    public float degreesPerSec = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    Vector3 posOffset = new Vector3();
    Vector3 newPos = new Vector3();

    void Start()
    {
        // Store the starting position & rotation of the object
        posOffset = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        if (canFloat)
        {
            // Spin object around Y-Axis
            transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSec, 0f), Space.World);

            // Float up/down with Sin()
            newPos = posOffset;
            newPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

            transform.position = newPos;
        }
    }

    public void disableFloat()
    {
        if (canFloat)
        {
            canFloat = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}