using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;

public class TutorialInputListener : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    public bool listenForYAxis = false;
    
    public UnityEvent onJoystick;
    public UnityEvent onTrigger;
    public UnityEvent onGrip;
    
    private InputDevice targetDevice;

    void Start() => TryToInitialize();
    
    void TryToInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if (devices.Count > 0) targetDevice = devices[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetDevice.isValid)
        {
            TryToInitialize();
        }
        else
        {
            // retrieve input
            if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axisValue))
            {
                if (listenForYAxis)
                {
                    if (axisValue.x > 0.1f || axisValue.x < -0.1f || axisValue.y > 0.1f || axisValue.y < -0.1f) onJoystick.Invoke();
                } else
                {
                    // determine action based on input received
                    if (axisValue.x > 0.1f || axisValue.x < -0.1f) onJoystick.Invoke();
                }
            }

            if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                if (triggerValue > 0.1f) onTrigger.Invoke();
            }

            if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            {
                if (gripValue > 0.1f) onGrip.Invoke();
            }
        }

    }
}
