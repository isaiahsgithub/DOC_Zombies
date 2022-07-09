using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    [SerializeField] Animator zombieAnim;


    float invincibilityTimeSlot = 0.5f;
    bool isInvincible = false;


    private void Update()
    {
        //You have a 0.5s invicibility period after being hit
        //So you can only be double hit as a maximum.
        if (isInvincible == true)
        {
            decreaseInvincibilityTimer();
        }
    }

    private void OnTriggerEnter(Collider objectHit)
    {
        //Only hit the player while the "Zombie Attack" animation is playing
        if(zombieAnim.GetCurrentAnimatorStateInfo(0).IsName("ZombieAttack"))
        {
            //If the zombie hits the player
            if(objectHit.gameObject.tag == "PlayerHitBoxTag")
            {
                //Checks if you can hit the player
                PlayerHealth playersHP = objectHit.GetComponentInParent<PlayerHealth>();
                if(playersHP != null && isInvincible == false)
                {
                    //Calls the function that handles when the player is attacked and allows player to be 
                    //invincible (to ensure they don't get instantly downed) for 0.5seconds
                    isInvincible = true;
                    playersHP.WhenZombieHit();
                }


            }
        }
    }

    //Decreases invincibility timer
    private void decreaseInvincibilityTimer()
    {
        invincibilityTimeSlot -= Time.deltaTime;
        if (invincibilityTimeSlot <= 0)
        {
            isInvincible = false;
            invincibilityTimeSlot = 0.5f;
        }
    }
}
