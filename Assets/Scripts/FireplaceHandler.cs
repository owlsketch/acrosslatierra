using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireplaceHandler : MonoBehaviour
{
    public Canvas UIPopUp;
    public GameObject firePS;
    public GameObject title;
    public GameObject mask;


    //make a list to track collided objects
    private List<Collider> collidedObjects = new List<Collider>();
    private bool UITriggered = false;
    private bool trigger = false;

    void Update()
    {
        // need to ensure fireplace is active
        if (trigger && gameObject.activeInHierarchy)
        {
            TutorialPanelHandler UIScript = UIPopUp.GetComponent<TutorialPanelHandler>();
            UIScript.FadeOut();

            if (firePS.activeInHierarchy == false)
            {
                firePS.SetActive(true);
                firePS.GetComponent<ParticleSystem>().Play();
            }

            if (title.activeInHierarchy == false) title.SetActive(true);
            if (mask.activeInHierarchy == false) mask.SetActive(true);

            trigger = false;
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
            // activate UI for hand rubbing
            if (!UITriggered)
            {
                TutorialPanelHandler UIScript = UIPopUp.GetComponent<TutorialPanelHandler>();
                UIScript.FadeIn();
                UITriggered = true;
            }
            if (col.CompareTag("Trigger"))
            {
                trigger = true;
            }
        }
    }
}
