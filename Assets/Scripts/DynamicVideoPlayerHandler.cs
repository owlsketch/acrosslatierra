using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicVideoPlayerHandler : MonoBehaviour
{
    public UnityEngine.Video.VideoClip videoClip;
    public RenderTexture renderTexture;
    public Material videoMaterial;
    public GameObject scene;
    public GameObject[] noninteractiveObjects; // items without a meshRender need to be disabled instead of hidden

    private Material defaultSkybox;
    private UnityEngine.Video.VideoPlayer videoPlayer;
    private Coroutine co;
    private Component[] meshRenderers;


    public void InitiateVideo(int delayTime)
    {
        defaultSkybox = RenderSettings.skybox;

        videoPlayer = gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.clip = videoClip;

        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;

        videoPlayer.playOnAwake = false;
        videoPlayer.waitForFirstFrame = true;
        videoPlayer.isLooping = true;
       
        if (co != null) { StopCoroutine(co); }
        co = StartCoroutine(StartVideoCoroutine(delayTime));
    }

    IEnumerator StartVideoCoroutine(int delayTime)
    {
        // On hand swap, check if video already playing so no need to update anything
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

            foreach (GameObject item in noninteractiveObjects) {
                if (item.activeInHierarchy == true)
                {
                    item.SetActive(false);
                    if (item.TryGetComponent<ParticleSystem>(out ParticleSystem PS))
                    {
                        PS.Stop();
                    }
                }
            }

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

        foreach (GameObject item in noninteractiveObjects)
        {
            if (item.activeInHierarchy == false)
            {
                item.SetActive(true);
                if (item.TryGetComponent<ParticleSystem>(out ParticleSystem PS))
                {
                    PS.Play();
                }
            }
        }

        // stop video, return everything to previous state
        videoPlayer.Stop();
        Destroy(gameObject.GetComponent<UnityEngine.Video.VideoPlayer>());

        RenderSettings.skybox = defaultSkybox;
    }
}
