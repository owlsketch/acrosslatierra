// Initializes scripts that run once Fireplace Scene is running

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFireplace : MonoBehaviour
{
    public TutorialPanelHandler initialUI;
    void Start() => initialUI.FadeIn();
}
