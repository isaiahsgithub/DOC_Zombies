using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clickPauseButton : MonoBehaviour
{
    [SerializeField] private Button unpauseButton;

    //Gets the button
    private void Start()
    {
        //When the button is clicked, it will perform the unpauseGame() function.
        Button btn = unpauseButton.GetComponent<Button>();
        btn.onClick.AddListener(unpauseGame);
    }

    public void unpauseGame()
    {
        //By unpausing the game, we hide the pause menu, re-lock the cursor and enable time
        this.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
}
