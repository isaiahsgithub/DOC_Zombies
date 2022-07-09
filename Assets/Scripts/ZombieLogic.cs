using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class ZombieLogic : MonoBehaviour
{
    //The player the zombie will follow
    private Transform thePlayerTarget;
    private float closeEnoughDistance = 1f;

    private AudioSource zn1;
    private AudioSource zn2;
    private AudioSource zn3;

    private Animator animator;
    private NavMeshAgent zombieAgent = null;
    private bool chasing = true;


    private float makeSoundEffectTimer = 5.0f;
    private float randomSoundTimeInterval = 0.0f;

    private Text roundCountText;  


    private void Awake()
    {
        //Gets variables needed
        roundCountText = GameObject.FindGameObjectWithTag("roundCounterTag").GetComponent<Text>();
        zn1 = GameObject.FindGameObjectWithTag("zsn1").GetComponent<AudioSource>();
        zn2 = GameObject.FindGameObjectWithTag("zsn2").GetComponent<AudioSource>();
        zn3 = GameObject.FindGameObjectWithTag("zsn3").GetComponent<AudioSource>();

        thePlayerTarget = GameObject.FindGameObjectWithTag("PlayerHitBoxTag").GetComponent<Transform>();

        zombieAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        randomSoundTimeInterval = determineTimeInterval();
        makeSoundEffectTimer = randomSoundTimeInterval;
    }

    private void Start()
    {
        //Sets the speed of the zombie for the navmeshagent
        float agentSpeed = determineSpeedOfZombie();
        zombieAgent.speed = agentSpeed;

        //If the zombie will be walking (0.2 speed), enable root motion
        if (agentSpeed == 0.2f)
        {
            animator.applyRootMotion = true;
        }

        //If the zombie won't be walking (running - 2.5 speed), do not enable root motion
        else
        {
            animator.applyRootMotion = false;
        }

        //Set the speed for the animator
        animator.SetFloat("zombieSpeed", zombieAgent.speed);
    }

    // Update is called once per frame
    void Update()
    {
        //Chase the player ever frame and check to see if a sound can be played
        chasePlayer();
        decreaseSoundTimer();
    }

    private void chasePlayer()
    {
        //If chasing the player is impossible, return
        if (!chasing)
        {
            Debug.LogError("Returned from failed chase.");
            return;
        }
        //Get the distance from the zombie agent to the player
        float distanceToTarget = Vector3.Distance(zombieAgent.transform.position, thePlayerTarget.position);

        //If the zombie is close enough to the player, try to attack the player
        if (distanceToTarget < closeEnoughDistance)
        {
            //Play Attack animation
            animator.SetTrigger("zombieAttack");
        }

        //If the zombie isn't dead, chase the player
        if (zombieAgent != null && animator.GetBool("isDead1") == false && animator.GetBool("isDead2") == false)
        {
            zombieAgent.SetDestination(thePlayerTarget.position);
        }
    }

    //Get random numbers
    float getRandomFloat(float theMin, float theMax)
    {
        return Random.Range(theMin, theMax);
    }
    int getRandomInt(int theMin, int theMax)
    {
        return Random.Range(theMin, theMax);
    }

    //Determine what the time interval a zombie can play a sound
    //Between 5 seconds and 20 secodns
    float determineTimeInterval()
    {
        return getRandomFloat(5.0f, 20.0f);
    }

    //Every 5-20 seconds, check to see if you can play a sound
    void decreaseSoundTimer()
    {
        makeSoundEffectTimer -= Time.deltaTime;
        if(makeSoundEffectTimer <= 0)
        {
            playEnemySound();
            makeSoundEffectTimer = randomSoundTimeInterval;
        }
    }


    //50% chance to randomly play the zombie sound effect.
    void playEnemySound()
    {
        int fiftyChanceForSound = getRandomInt(0, 100);
        if (fiftyChanceForSound < 50)
        {
            //If a sound effect is able to be played,
            //Randomly determine which one out of 3 will be played.
            int whichSound = getRandomInt(0, 3);
            if(whichSound == 0)
            {
                zn1.Play();
            }
            else if(whichSound == 1)
            {

                zn2.Play();
            }
            else
            {
                zn3.Play();

            }
        }
    }


    private float determineSpeedOfZombie()
    {
        int theRound = int.Parse(roundCountText.text);
        int myRNG = getRandomInt(0, 100);
        //Round 1: 100% chance for slow
        //Round 2: 90% chance
        //Round 3: 80% chance
        //Round 4: 70% chance
        //Round 5: 60% chance
        //round 6+: 0% chance
        if (theRound == 1 || theRound == 0)
        {
            return 0.2f;
        }
        else if(theRound == 2)
        {
            if(myRNG < 90)
            {
                return 0.2f;
            }
            else
            {
                return 2.5f;
            }
        }
        else if (theRound == 3)
        {
            if (myRNG < 80)
            {
                return 0.2f;
            }
            else
            {
                return 2.5f;
            }
        }
        else if (theRound == 4)
        {
            if (myRNG < 70)
            {
                return 0.2f;
            }
            else
            {
                return 2.5f;
            }
        }
        else if (theRound == 5)
        {
            if (myRNG < 60)
            {
                return 0.2f;
            }
            else
            {
                return 2.5f;
            }
        }
        else
        {
            return 2.5f;
        }
    }

}
