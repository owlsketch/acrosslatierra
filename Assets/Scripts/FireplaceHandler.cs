using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireplaceHandler : MonoBehaviour
{
    public GameObject firePS;
    public GameObject title;

    //make a list to track collided objects
    private List<Collider> collidedObjects = new List<Collider>();
    private bool trigger = false;

    void Update()
    {
        if (trigger)
        {
            if (firePS.activeInHierarchy == false) firePS.SetActive(true);
            if (title.activeInHierarchy == false) title.SetActive(true);
        }

        // Everyframe we have a check on how many objects are actively colliding
        collidedObjects.Clear();
    }
    private void OnTriggerStay(Collider col)
    {
        /// to improve performance need a check to do this only when lighting fire
        if (!collidedObjects.Contains(col) && col.CompareTag("Firewood"))
        {
            collidedObjects.Add(col);
        }


    }

    private void OnTriggerEnter(Collider col)
    {
        if (collidedObjects.Count >= 3)
        {
            if (col.CompareTag("Trigger"))
            {
                trigger = true;
            }
        }
    }
}
