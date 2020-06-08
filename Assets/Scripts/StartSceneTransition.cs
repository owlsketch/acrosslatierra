using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneTransition : MonoBehaviour
{
    public int waitTime = 3;

    void Start() => StartCoroutine(LoadFireplace());

    IEnumerator LoadFireplace()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("Fireplace");
    }
}
