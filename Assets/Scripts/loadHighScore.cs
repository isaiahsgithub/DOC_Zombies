using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadHighScore : MonoBehaviour
{
    //Loads the save when this script is ran (i.e. when you go to the leaderboards)
    void Start()
    {
        loaderSaver lS = this.gameObject.GetComponentInParent<loaderSaver>();
        lS.loadGame();
    }
}
