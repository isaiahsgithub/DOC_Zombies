using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;



public class highScore
{
    public string round;
    

    public highScore(string r)
    {
        this.round = r;
    }


    public string getRound()
    {
        return this.round;
    }

}

public class loaderSaver : MonoBehaviour
{
    private string cR;
    public static string loadedString;
    private Text roundText;


    public void refreshObjects()
    {
        roundText = GameObject.FindGameObjectWithTag("roundCounterTag").GetComponent<Text>();
    }

    public void refreshVariables()
    {
        cR = (roundText.text).ToString();
    }

    public void saveGame()
    {
        refreshObjects();
        refreshVariables();
        highScore theHighScore = new highScore(cR);

        //If there is a record of a high score
        if (File.Exists(Application.persistentDataPath + "/highscore.json"))
        {
            //If the current score is greater than the previously saved score 
            //Save it (overwritting the previous save)
            //Note: False is set as this is not a normal load
            loadJSON(Application.persistentDataPath + "/highscore.json", false);
            if (int.Parse(loadedString) < int.Parse(cR))
            {
                saveAsJSON(Application.persistentDataPath + "/highscore.json", theHighScore);
            }
            else
            {
                Debug.Log("Not saving - there is a higher score available");
            }
        }
        //If this is the first time saving
        else
        {
            saveAsJSON(Application.persistentDataPath + "/highscore.json", theHighScore);
        }
        
    }

    public static void saveAsJSON(string savePath, highScore hS)
    {
        string json = JsonUtility.ToJson(hS);
        File.WriteAllText(savePath, json);
    }

    public void loadGame()
    {
        loadJSON(Application.persistentDataPath + "/highscore.json", true);
    }

    public static void loadJSON(string savePath, bool normalLoad)
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            highScore s = JsonUtility.FromJson<highScore>(json);
            loadedString = s.getRound();

            //If this is a normal load 
            //i.e. not a save check load
            if (normalLoad == true)
            {
                Text highScoreText = GameObject.FindGameObjectWithTag("highestRoundTag").GetComponent<Text>();
                highScoreText.text = "Highest Round: " + loadedString;
            }
        }
        else
        {
            //If unable to load the save
            Debug.LogError("Unable to load the file: " + savePath);
        }
    }

}
