using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enableDrops : MonoBehaviour
{
    //Creates variables that will be our drops
    private Image twoTimes;
    private Image ExtremePower;

    private void Awake()
    {
        //Finds the images for the drops via the hierarchy and their tags
        twoTimes = GameObject.FindGameObjectWithTag("ttTag").GetComponent<Image>();
        ExtremePower = GameObject.FindGameObjectWithTag("epTag").GetComponent<Image>();

        //Since this is in awake, these two images will not be enabled.
        twoTimes.enabled = false;
        ExtremePower.enabled = false;
    }
}
