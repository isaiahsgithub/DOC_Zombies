using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class openPause : MonoBehaviour
{
    [SerializeField] private GameObject pauseBG;

    // Starts with the pause menu disabled
    void Start()
    {
        pauseBG.SetActive(false);
    }


    void Update()
    {
        //If player presses escape
        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            //If pause is not enabled, pause the game
           if (pauseBG.activeSelf == false)
            {

                pauseBG.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;

            }
           //If pause is enabled, unpause the game
            else
            {

                pauseBG.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
            }
        }
        
    }
}
