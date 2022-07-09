using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class leaderboardClicks : MonoBehaviour
{
    //When clicked, this will return to the main menu
    public void returnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
