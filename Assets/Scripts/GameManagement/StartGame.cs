using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public TutorialPanelHandler initialUI;
    void Start() => initialUI.FadeIn();
}
