using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setOpacity : MonoBehaviour
{
    //This script is mainly for pause menu
    Image pauseMenuBackground;
    void Start()
    {
        //Gets pause menu
        pauseMenuBackground = GetComponent<Image>();

        //Sets pause menu image equal to the following settings:
        // 0.5f opacity (alpha)
        var tempColor = pauseMenuBackground.color;
        tempColor.a = 0.5f;
        pauseMenuBackground.color = tempColor;
    }

}
