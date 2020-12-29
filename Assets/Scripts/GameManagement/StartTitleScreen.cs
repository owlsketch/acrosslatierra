// Initializes scripts that run once TitleScreen Scene is running

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTitleScreen : MonoBehaviour
{
    public int fadeInTime = 3;
    public TitleCardHandler titleCard;

    void Start() => titleCard.FadeIn(fadeInTime);
}
