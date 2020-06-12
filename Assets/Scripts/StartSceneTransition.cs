using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneTransition : MonoBehaviour
{
    public int minWaitTime = 3;

    void Start() => StartCoroutine(LoadFireplace());

    IEnumerator LoadFireplace()
    {
        yield return new WaitForSeconds(minWaitTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Fireplace");
        while(!asyncLoad.isDone) { yield return null; }
    }
}
