using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneTransition : MonoBehaviour
{
    public void LoadNextScene() => StartCoroutine(LoadFireplace());

    IEnumerator LoadFireplace()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Fireplace");
        while(!asyncLoad.isDone) { yield return null; }
    }
}
