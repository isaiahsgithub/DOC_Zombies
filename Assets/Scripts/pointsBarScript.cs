using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pointsBarScript : MonoBehaviour
{
    Image pointsBar;
    
    void Start()
    {
        //Gets the points bar
        pointsBar = GetComponent<Image>();

        //Sets the points bar settings
        var tempColor = pointsBar.color;
        tempColor.a = 0.95f;
        pointsBar.color = tempColor;
    }
}
