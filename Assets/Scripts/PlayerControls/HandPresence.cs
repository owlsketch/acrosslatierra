using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    
    public bool showController = false;
    public InputDeviceCharacteristics controllerCharacteristics;
    public InputDeviceCharacteristics oppositeControllerCharacteristics;
    public List<GameObject> controllerPrefabs;
    public GameObject handModelPrefab;

    private InputDevice targetDevice;
    private InputDevice oppositeDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;

    void Start() => TryToInitialize();

    void TryToInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            }
            else
            {
                Debug.LogError("Couldn't find corresponding controller model");
            }

            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }

    void TryToAccessOppositeController()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(oppositeControllerCharacteristics, devices);
        if (devices.Count > 0) oppositeDevice = devices[0];
    }

    void UpdateHandAnimation()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        } else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }

        if (!oppositeDevice.isValid)
        {
            handAnimator.SetFloat("Distance", 1.0f);
            TryToAccessOppositeController();
        }
        else
        {
            Vector3 targetDist;
            Vector3 oppositeDist;
            targetDevice.TryGetFeatureValue(CommonUsages.devicePosition, out targetDist);
            oppositeDevice.TryGetFeatureValue(CommonUsages.devicePosition, out oppositeDist);

            float controllerDistance = Vector3.Distance(targetDist, oppositeDist);

            // https://stackoverflow.com/questions/5731863/mapping-a-numeric-range-onto-another
            // output = output_start + ((output_end - output_start) / (input_end - input_start)) * (input - input_start)
            double output = (1.0 / (0.3 - 0.05)) * (controllerDistance - 0.05);

            if (controllerDistance > 0.3)
            {
                handAnimator.SetFloat("Distance", 1.0f);
            } 
            else
            {
                handAnimator.SetFloat("Distance", (float)output);
            }
        }
    }
    
    void Update()
    {
        if (!targetDevice.isValid)
        {
            TryToInitialize();
        } else
        {
            if (showController)
            {
                spawnedHandModel.SetActive(false);
                spawnedController.SetActive(true);
            }
            else
            {
                spawnedHandModel.SetActive(true);
                spawnedController.SetActive(false);
                UpdateHandAnimation();
            }
        }

    }
}
