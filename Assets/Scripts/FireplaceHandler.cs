using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireplaceHandler : MonoBehaviour
{
    //make a list to track collided objects
    private List<Collider> collidedObjects = new List<Collider>();

    void Update()
    {
        // Everyframe we have a check on how many objects are actively colliding
        // Debug.Log(collidedObjects.Count);
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
}
