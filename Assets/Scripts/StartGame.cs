using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public TutorialUIHandler initialUI;
    void Start() => initialUI.FadeIn();
}
