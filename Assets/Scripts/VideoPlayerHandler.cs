using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayerHandler : MonoBehaviour
{
    public Material videoMaterial;
    public GameObject scene;
    public Terrain terrain;

    private Component[] meshRenderers;
    private UnityEngine.Video.VideoPlayer videoPlayer;
    private Material defaultSkybox;
    private Coroutine co;

    public void InitiateVideo(int delayTime)
    {
        videoPlayer = gameObject.GetComponent<UnityEngine.Video.VideoPlayer>();
        defaultSkybox = RenderSettings.skybox;
       
        if (co != null) { StopCoroutine(co); }
        co = StartCoroutine(StartVideoCoroutine(delayTime));
    }

    IEnumerator StartVideoCoroutine(int delayTime)
    {
        // On swap, check if video already playing so no need to update anything
        if (!videoPlayer.isPlaying)
        {
            // as long as coroutine isn't stopped (object is held on to) load scene occurs
            // min of 2 seconds before video played. Prepare() may make wait time longer.
            videoPlayer.Prepare();
            yield return new WaitForSeconds(delayTime);
            while (!videoPlayer.isPrepared) { yield return null; }

            // no longer render meshes
            if (scene != null)
            {
                meshRenderers = scene.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer renderer in meshRenderers)
                {
                    renderer.enabled = false;
                }
            }

            if (terrain) { terrain.enabled = false; }

            // once ready, then update everything
            RenderSettings.skybox = videoMaterial;
            videoPlayer.Play();
        }
    }

    public void TerminateVideo(int delayTime)
    {
        if (co != null) { StopCoroutine(co); }
        co = StartCoroutine(StopVideoCoroutine(delayTime));
    }

    IEnumerator StopVideoCoroutine(int delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (scene != null)
        {
            foreach (MeshRenderer renderer in meshRenderers)
            {
                renderer.enabled = true;
            }
        }
        
        if (terrain) { terrain.enabled = false; }

        // stop video, return everything to previous state
        RenderSettings.skybox = defaultSkybox;
        videoPlayer.Stop();
    }
}
