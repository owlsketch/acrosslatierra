// Swaps the material an object currently displays

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMaterialHandler : MonoBehaviour
{
    public Material secondaryMaterial;
    public bool oneTimeOnly;

    private Material activeMaterial;
    private Material swapMaterial;
    private bool swappedAlready = false;

    
    // need to get the current active material to store for swapping
    void Start()
    {
        activeMaterial = gameObject.GetComponent<Renderer>().material;
    }

    public void SwapMaterials()
    {
        if (swappedAlready && oneTimeOnly) return;

        swapMaterial = activeMaterial;
        gameObject.GetComponent<Renderer>().material = secondaryMaterial;
        secondaryMaterial = swapMaterial;
        swappedAlready = true;
    }
    
}
