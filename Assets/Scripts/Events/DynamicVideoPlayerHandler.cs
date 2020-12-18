// Handles Video Player 360 Videos
// Automatically instantiates and deletes videoplayers for memory management

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class DynamicVideoPlayerHandler : MonoBehaviour
{
    public UnityEngine.Video.VideoClip videoClip;
    public RenderTexture renderTexture;
    public Material videoMaterial;

    [Space]

    public GameObject player;
    [Tooltip("The container of the skybox camera, which will be rotated accordingly when displaying 360 videos.")]
    public GameObject skyboxContainer;
    [Tooltip("The camera that renders the scene, which will be hidden when displaying 360 videos.")]
    public Camera overlayCamera;
    
    private Material defaultSkybox;
    private UnityEngine.Video.VideoPlayer videoPlayer;
    private Coroutine co;

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
        // Does this mean if we are persistently swapping hands before video is prepared, scene wont occur?
        if (!videoPlayer.isPlaying)
        {
            // as long as coroutine isn't stopped (object is held on to) load scene occurs

            videoPlayer.Prepare();
            
            yield return new WaitForSeconds(delayTime); // wait a minimum of delayTime before displaying video
            while (!videoPlayer.isPrepared) { yield return null; } // if video still isn't ready, then continue waiting

            player.GetComponent<CustomContinuousMovement>().AllowMovement(false); // disable player movement
            overlayCamera.enabled = false; // remove scene rendering camera
            
            skyboxContainer.transform.Rotate(new Vector3(0, 0, 90), Space.Self);

            // update the interactable's layer to be part of the body layer
            Transform interactable = gameObject.transform.parent.transform;
            foreach (Transform child in interactable) { child.gameObject.layer = 12; }
            interactable.gameObject.layer = 12;

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
        
        player.GetComponent<CustomContinuousMovement>().AllowMovement(true);
        overlayCamera.enabled = true;
        
        skyboxContainer.transform.Rotate(new Vector3(0, 0, 90), Space.Self);

        Transform interactable = gameObject.transform.parent.transform;
        foreach (Transform child in interactable) { child.gameObject.layer = 11; }
        interactable.gameObject.layer = 11;
        
        videoPlayer.Stop();
        Destroy(gameObject.GetComponent<UnityEngine.Video.VideoPlayer>());

        // TODO: Certify texture replacement. Still seem to be triggering non-replacement!
        RenderSettings.skybox = defaultSkybox;
    }
}
