using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuClicks : MonoBehaviour
{
    //This script handles all of the possible button presses you can press on the main menu

    //This starts the game
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    //This quits the game (only works when game is compiled)
    public void QuitGame()
    {
        Application.Quit();
    }


    //Opens Leaderboards (which shows your highest round)
    public void LeaderBoards()
    {
        SceneManager.LoadScene("LeaderBoardScene");
    }

}
