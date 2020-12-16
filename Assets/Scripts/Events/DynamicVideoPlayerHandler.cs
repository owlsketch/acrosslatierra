// Handles Video Player 360 Videos
// Automatically instantiates and deletes videoplayers for memory management

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicVideoPlayerHandler : MonoBehaviour
{
    public UnityEngine.Video.VideoClip videoClip;
    public RenderTexture renderTexture;
    public Material videoMaterial;

    public Camera skyboxCamera;
    public Camera overlayCamera;

    public GameObject player;

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
            
            player.GetComponent<CustomContinuousMovement>().ToggleMovement();
            overlayCamera.enabled = false;
            
            skyboxCamera.transform.up = new Vector3(0, 0, 0);
            
            // update the interactable's layer to be part of the body layer
            Transform parent = gameObject.transform.parent.transform;
            parent.gameObject.layer = 12;
            foreach (Transform child in parent)
            {
                child.gameObject.layer = 12;
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
        
        player.GetComponent<CustomContinuousMovement>().ToggleMovement();
        overlayCamera.enabled = true;

        // update the interactable's layer to be part of the body layer
        Transform parent = gameObject.transform.parent.transform;
        parent.gameObject.layer = 11;
        foreach (Transform child in parent)
        {
            child.gameObject.layer = 11;
        }

        // stop video, return everything to previous state
        videoPlayer.Stop();
        Destroy(gameObject.GetComponent<UnityEngine.Video.VideoPlayer>());

        RenderSettings.skybox = defaultSkybox;
    }
}
