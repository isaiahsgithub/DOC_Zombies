using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickUpGunPerk : MonoBehaviour
{
    [SerializeField] private Image gunPerkImage;
    [SerializeField] private Text informText;

    private bool destroyObject = false;
    private float timer = 3.5f;
    private void Awake()
    {
        //Starts with these variables disabled
        gunPerkImage.enabled = false;
        informText.enabled = false;
    }

    private void Update()
    {
        //After picking up the gun perk, a timer will play then the rotating gun will be destroyed 
        //And the text will dissappear
        if(destroyObject == true)
        {

            timer -= Time.deltaTime;
            if(timer <= 0.0f)
            {
                informText.text = "New Text";
                informText.enabled = false;
                timer = 3.5f;
                destroyObject = false;
                Destroy(this.gameObject);
            }
        }
    }

    //When walking into the rotating gun, you get the gun perk.
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "FirstPersonPlayer")
        {
            gunPerkImage.enabled = true;
            destroyObject = true;
            informText.enabled = true;
            informText.text = "Weapon Upgrade: Permanent 2x Damage. Stacks with other upgrades.";
        }
    }
}
