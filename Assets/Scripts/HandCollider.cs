using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour
{
    public GameObject sparksPrefab;

    private GameObject spawnedSparks;
    private int count = 0;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Controls"))
        {
            count++;

            if (count > 5)
            {
                spawnedSparks = Instantiate(sparksPrefab, transform);
                spawnedSparks.transform.position = spawnedSparks.transform.position + new Vector3(-0.05f, 0, 0);
                spawnedSparks.GetComponent<ParticleSystem>().Play();
            }
        }
    }
}
