using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    //Setting up variables
    private AudioSource zombieDeathSound;

    private AudioSource ttSound;
    private AudioSource epSound;
    private Image twoTimes;
    private Image ExtremePower;

    private int maxHP = 100;
    private int curHP;
    private bool isDead = false;

    private Animator zombieAnim;

    private void Awake()
    {
        //Gets the variables and prevents sound from playing on awake.
        zombieDeathSound = GameObject.FindGameObjectWithTag("zsd").GetComponent<AudioSource>();
        zombieDeathSound.playOnAwake = false;
        zombieAnim = GetComponent<Animator>();


        twoTimes = GameObject.FindGameObjectWithTag("ttTag").GetComponent<Image>();
        ExtremePower = GameObject.FindGameObjectWithTag("epTag").GetComponent<Image>();
        ttSound = GameObject.FindGameObjectWithTag("ttSTag").GetComponent<AudioSource>();
        epSound = GameObject.FindGameObjectWithTag("epSTag").GetComponent<AudioSource>();


    }

    // Start is called before the first frame update
    void Start()
    {
        //When the script first starts, set enemy current hp equal to max hp.
        curHP = maxHP;
    }


    //A function used to allow the enemy to take damage
    public void TakeDamage(int dmg)
    {
        //Decrement current HP by dmg amount
        this.curHP -= dmg;
        if (this.curHP <= 0)
        {
            //If the enemy dies, play a random death animation and set their hp to 0
            playRandomDeath();
            isDead = true;
            this.curHP = 0;
        }
        else
        {
            //If the enemy doesn't die, ensure that the bool tracking alive status isn't altered
            isDead = false;
        }
    }

    //A getter function
    public bool getDeadStatus()
    {
        return isDead;
    }


    public void playRandomDeath()
    {
        dropItem();
        //Play the death sound effect
        zombieDeathSound.Play();
        //Disable the navmesh so it doesnt follow the player
        this.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
        this.gameObject.GetComponent<NavMeshAgent>().updatePosition = false;
        this.gameObject.GetComponent<NavMeshAgent>().enabled = false;


        //Disable capsule collider so player no longer shoots the zombie
        this.gameObject.GetComponent<Collider>().enabled = false;

        //Bug fix, for slow enemies when dead we disable the hitbox.
        foreach (Collider childCollide in this.gameObject.GetComponentsInChildren<Collider>()) {
            childCollide.enabled = false;
        }

        //Turns on root motion, so the zombie properly falls
        zombieAnim.applyRootMotion = true;

        //Randomly determine a death animation
        int whichDeathAnimation = Random.Range(1, 3);
        string deathName = "isDead" + whichDeathAnimation;
        zombieAnim.SetBool(deathName, true);

        //2 seconds after the death animation finishes, despawn the zombie
        Destroy(gameObject, zombieAnim.GetCurrentAnimatorStateInfo(0).length + 2.0f);
    }

    //Gets random numbers
    float getRandomFloat(float theMin, float theMax)
    {
        return Random.Range(theMin, theMax);
    }
    int getRandomInt(int theMin, int theMax)
    {
        return Random.Range(theMin, theMax);
    }

    //Function determines if the player is able to get a random drop
    private void dropItem() {
        int doesDrop = getRandomInt(0, 100);
        
        //5% chance to drop item
        if(doesDrop < 5)
        {
            //Determine which item drops
            if (doesDrop % 2 == 0)
            {
                //Ensures you don't get the same drop twice
                if(twoTimes.enabled == false)
                {
                    //Enables the drop, and plays a sound cue.
                    twoTimes.enabled = true;
                    ttSound.Play();
                }
            }
            else
            {
                //Ensures you don't get the same drop twice
                if(ExtremePower.enabled == false)
                {
                    //Enables the drop, and plays a sound cue.
                    ExtremePower.enabled = true;
                    epSound.Play();

                }
            }
        }
    }
}
