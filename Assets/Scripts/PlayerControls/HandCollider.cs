// When the two hands rub together, the sparks particle system is triggered!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour
{
    public GameObject sparksPrefab;

    private GameObject spawnedSparks;
    private int count = 0;
    private bool coroutineRunning = false;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Controls"))
        {
            count++;
            if (!coroutineRunning) StartCoroutine(WaitAndReset());

            if (count > 5)
            {
                spawnedSparks = Instantiate(sparksPrefab, transform);
                spawnedSparks.transform.position = spawnedSparks.transform.position + new Vector3(-0.05f, 0, 0);
                spawnedSparks.GetComponent<ParticleSystem>().Play();
            }
        }
    }

    IEnumerator WaitAndReset()
    {
        coroutineRunning = true;
        yield return new WaitForSeconds(4);

        count = 0;
        coroutineRunning = false;
        if (spawnedSparks) Destroy(spawnedSparks);
    }
}
