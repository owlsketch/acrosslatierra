using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class MenuHandler : MonoBehaviour
{
    public XRNode inputSource;
    public Canvas Menu;

    private bool isPressed = false;
    private bool lastButtonState = false;
    private bool UITriggered = false;

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        if (device.TryGetFeatureValue(CommonUsages.menuButton, out isPressed))
        {
            if (isPressed == true && lastButtonState == false)
            {
                TutorialPanelHandler UIScript = Menu.GetComponent<TutorialPanelHandler>();
                if (!UITriggered)
                {
                    UIScript.FadeIn();
                    UITriggered = true;
                }
                else
                {
                    UIScript.FadeOut();
                    UITriggered = false;
                }
            }

            lastButtonState = isPressed;
        }
    }
}
