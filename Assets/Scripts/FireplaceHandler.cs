using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireplaceHandler : MonoBehaviour
{
    public GameObject fire;
    public GameObject title;

    //make a list to track collided objects
    private List<Collider> collidedObjects = new List<Collider>();

    void Update()
    {
        if (collidedObjects.Count >= 3)
        {
            if (fire.activeInHierarchy == false) fire.SetActive(true);
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
}
