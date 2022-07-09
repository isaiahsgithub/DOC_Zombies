using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private Slider playerHealthBar;
    public int playerMaxHP = 200;
    public int playerCurrentHP;
    private bool isDead;
    private bool isHit = false;
    private bool fadeOut = false;
    private float regainTimer = 7.5f;

    [SerializeField] Image hpPerk;
    private bool keepChecking = true;

    [SerializeField] Image bloodSplatter1;
    [SerializeField] Image bloodSplatter2;
    Color tempOpacity;
    loaderSaver lS;

    private void Awake()
    {
        //Start with the blood splatters being at 0 opacity
        var startOpacity = bloodSplatter1.color;
        startOpacity.a = 0.0f;
        bloodSplatter1.color = startOpacity;
        bloodSplatter2.color = startOpacity;


        tempOpacity = bloodSplatter1.color;
        tempOpacity.a = 0.5f;
    }


    void Start()
    {
        //Get the loader saver and set HP to be full
        lS = this.gameObject.GetComponent<loaderSaver>();
        playerCurrentHP = playerMaxHP;
    }

    public void WhenZombieHit()
    {
        //Decrease HP
        isHit = true;
        this.playerCurrentHP -= 75;

        //Display bloodsplatter
        getImage();

        //Lower HP slider
        updateSlider();

        //If the player dies then begin dead function
        if(this.playerCurrentHP <= 0)
        {
            hasDied();
        }
        else
        {
            isDead = false;
        }
    }

    //Dead function
    void hasDied()
    {
        //Unlock mouse
        Cursor.lockState = CursorLockMode.None;

        //Save game
        lS.saveGame();
        isDead = true;
        this.playerCurrentHP = 0;

        //Go to leaderboards
        SceneManager.LoadScene("LeaderBoardScene");
    }

    private void Update()
    {
        //Once identified that you have the perk, no need to keep checking it
        if(keepChecking == true)
        {

            checkForHPMultiplierPerk();
            
        }
        //After 7.5 seconds, the player will regain HP
        if (isHit == true)
        {
            regainTimer-= Time.deltaTime;
            if(regainTimer <= 0)
            {
                fadeOut = true;
                isHit = false;
                regainTimer = 7.5f;
            }
        }

        //Gradually increase slider (for nice effect)
        if(isHit == false && playerCurrentHP != playerMaxHP)
        {
            playerCurrentHP += 1;
            updateSlider();
        }
        //Gradually fade out the blood splatters
        if(fadeOut == true)
        {
            tempOpacity.a = tempOpacity.a - 0.01f;
            if (bloodSplatter1.color.a != 0.0)
            {
                bloodSplatter1.color = tempOpacity;
            }
            if(bloodSplatter2.color.a != 0.0)
            {
                bloodSplatter2.color = tempOpacity;

            }
        }
        //If the fade out is complete
        if(bloodSplatter1.color.a <= 0.0 && bloodSplatter2.color.a <= 0.0 && fadeOut == true)
        {
            tempOpacity.a = 0.0f;
            bloodSplatter1.color = tempOpacity;
            bloodSplatter2.color = tempOpacity;
            fadeOut = false;
            tempOpacity.a = 0.5f;
        }


        //If the player somehow falls out of bounds (shouldn't be possible, but bug checking) they die.
        if(this.gameObject.transform.position.y < -200)
        {
            hasDied();
        }
    }


    //Sets the player HP slider value equal to the current HP
    private void updateSlider()
    {

        playerHealthBar.value = playerCurrentHP;
    }

    //When determining which bloodsplatter to show, check by opacities 
    //0 opacity means that it is not currently active
    private void getImage()
    {
        if(bloodSplatter1.color.a == 0.0)
        {
            showImage(bloodSplatter1);
        }
        else if(bloodSplatter2.color.a == 0)
        {
            showImage(bloodSplatter2);
        }
    }

    //Show the image (by altering its opacity)
    private void showImage(Image myImage)
    {
        myImage.color = tempOpacity;
    }

    private void checkForHPMultiplierPerk()
    {
        //If you have double HP perk, double your HP
        if(hpPerk.enabled == true)
        {
            playerHealthBar.maxValue = 400;
            playerMaxHP = 400;
            keepChecking = false;

        }
        //If you don't have the perk, don't change the HP
        else
        {
            playerHealthBar.maxValue = 200;
        }
    }

}
