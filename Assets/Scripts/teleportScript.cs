using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportScript : MonoBehaviour
{
    [SerializeField] private GameObject placeToGo;
    [SerializeField] private GameObject thePlayer;


    //For teleport object, check if player collides with it
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.name == "FirstPersonPlayer")
        {
            //Need to disable the character controller for teleport
            thePlayer.GetComponent<CharacterController>().enabled = false;

            //Move the player to the other teleporter
            Vector3 moveHere = new Vector3(placeToGo.transform.position.x, placeToGo.transform.position.y, placeToGo.transform.position.z);
            thePlayer.transform.position = moveHere;

            //Re-enable character controller
            thePlayer.GetComponent<CharacterController>().enabled = true;
        }
    }
}
