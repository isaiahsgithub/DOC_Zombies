using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class interact : MonoBehaviour
{
    //Assigns variables
    [SerializeField] private Transform mainCamera;
    [SerializeField] private float range = 2f;
    [SerializeField] private Text informMsg;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private Text playerPoints;
    string objectName = "";

    [SerializeField] private Image doubleHP;
    [SerializeField] private Image tripleDMG;


    private void Awake()
    {
        //Starts with the perks and the inform text disabled
        doubleHP.enabled = false;
        tripleDMG.enabled = false;
        informMsg.enabled = false;
    }

    private void Update()
    {
        //Checks if the player is 2m away from an interactable object
        RaycastHit hit;
        if(Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, range, interactLayer))
        {
            
            //Checks the tag of each interactable object, their tag contains their price value.
            
            //Preset values for doors
            if(hit.transform.gameObject.tag == "750" || hit.transform.gameObject.tag == "1250" || hit.transform.gameObject.tag == "1500" || hit.transform.gameObject.tag == "5000")
            {
                informMsg.enabled = true;
                objectName = "door";
                
            }

            //Preset value for HP perk
            else if(hit.transform.gameObject.tag == "2500" && doubleHP.enabled == false)
            {
                informMsg.enabled = true;
                objectName = "double health perk";
            }

            //Preset value for damage perk
            else if(hit.transform.gameObject.tag == "3000" && tripleDMG.enabled == false)
            {
                informMsg.enabled = true;
                objectName = "triple damage perk";
            }


            //If the informMsg has been enabled (ensuring you can only buy something once), give the player
            //the option to buy the item
            if(informMsg.enabled == true)
            {
                informMsg.text = "Right click to buy " + objectName + " for " + hit.transform.gameObject.tag + " points.";
                buyObject(hit.transform.gameObject, objectName);
            }
            
        }
        else
        {
            informMsg.enabled = false;
        }
        
    }


    //Buy the object function
    void buyObject(GameObject objectToBuy, string objectName)
    {

        //If the player presses right click (Fire2) - they will buy the item
        if (Input.GetButtonDown("Fire2"))
        {
            //If the player has enough money to buy
            if(int.Parse(playerPoints.text) >= int.Parse(objectToBuy.gameObject.tag))
            {
                //Decrease the players points by the price tag of the item
                playerPoints.text = (int.Parse(playerPoints.text) - int.Parse(objectToBuy.gameObject.tag)).ToString();
                
                //For all door objects, change their Y position to -700 (used for zombie spawns)
                if(objectName == "door")
                {
                    Vector3 newPosition = new Vector3(objectToBuy.transform.position.x, -700f, objectToBuy.transform.position.z); ;
                    objectToBuy.transform.position = newPosition;
                }

                //Enables the perks and plays the fridge open animation
                else if(objectName == "double health perk")
                {
                    objectToBuy.transform.gameObject.GetComponent<Animator>().SetTrigger("buyDrinkHP");
                    doubleHP.enabled = true;
                }
                else if(objectName == "triple damage perk")
                {
                    objectToBuy.transform.gameObject.GetComponent<Animator>().SetTrigger("buyDrink");
                    tripleDMG.enabled = true;
                }
            }
        }
    }
}
