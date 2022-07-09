using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootGun : MonoBehaviour
{
    [Header("Gun Specifics")]
    [SerializeField] private AudioSource shootSound;
    [SerializeField] private float shootDistance = 1000f;
    [SerializeField] private GameObject theGun;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulHolPrefab;
    [SerializeField] private GameObject muzzleFlashPrefab;

    [Header("Targets")]
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] LayerMask wallLayers;
    [SerializeField] private Transform camera;

    [Header("Points and Drops")]
    [SerializeField] private Text myPoints;
    [SerializeField] private Image twoTimes;
    [SerializeField] private Image extremePower;
    [SerializeField] private Image tripleDMGPerk;
    [SerializeField] private Image gunDmgPerk;
    public int pointMultiplier = 1;
    public int dmgMultiplier = 1;
    public int dmgPerkMultiplier = 1;
    public int gunPerkDmgMultiplier = 1;
    public float ttTimer = 30f;
    public float epTimer = 30f;

    bool shot = false;
    float theTime = 2.0f;
    GameObject newBullet;
    private Animator animator;

    //Get components of gun
    private void Awake()
    {
        animator = theGun.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check to see if any bonuses are active (from zombie drops)
        checkForDropBonus();
        checkforPerk();


        //Shoot the gun
        if (Input.GetButtonDown("Fire1"))
        {
            shot = true;
            animator.SetTrigger("Fire");

           
            Shoot();
        }

        //If you shot, after a set amount of time (2.0 seconds) all the bullets will be deleted (to improve performance)
        if(shot == true)
        {
            //Deletes all bullets
            theTime -= Time.deltaTime;
            if(theTime <= 0)
            {
                foreach(GameObject deletedBullet in GameObject.FindGameObjectsWithTag("shotBullet"))
                {
                    Destroy(deletedBullet);
                }
                shot = false;
                theTime = 2.0f;
            }
        }



    }

    private void Shoot()
    {

        makeMuzzleFlash();

        //Play sound effect
        shootSound.Play();
        RaycastHit hit;

        //Create a bullet to come out of gun
        makeBullet();
        
        //If hit enemy
        if (Physics.Raycast(camera.position, camera.forward, out hit, shootDistance, enemyLayers))
        {
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            //Bug fix: when zombie moving slow, its hands will register as a hitbox.
            if(enemyHealth == null)
            {
                enemyHealth = hit.collider.GetComponentInParent<EnemyHealth>();
            }
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(10*dmgMultiplier* dmgPerkMultiplier* gunPerkDmgMultiplier);
                //100 points per kill - 10 points per hit.
                if(enemyHealth.getDeadStatus() == true)
                {
                    addPoints(100*pointMultiplier);
                }
                else
                {
                    addPoints(10*pointMultiplier);
                }
            }
        }

        //If hit wall
        else if(Physics.Raycast(camera.position, camera.forward, out hit, shootDistance, wallLayers))
        {
            //Make bullet hole decal on wall
            Instantiate(bulHolPrefab, hit.point + (0.01f * hit.normal), Quaternion.LookRotation(-1 * hit.normal, hit.transform.up));
        }
    }

    private void makeBullet()
    {
        //Creates a bullet out of the bullet prefab, with the guns position as its starting position
        newBullet = Instantiate(bullet, theGun.transform.position, Quaternion.identity);

        //This part was taken from the simple shoot code
        newBullet.GetComponent<Rigidbody>().AddForce(camera.transform.forward * 500f);
        //The rest was my own

        //Sets the parent to an empty object, so that it does not move with the camera and player.
        newBullet.transform.SetParent(GameObject.FindGameObjectWithTag("emptyBullet").GetComponent<Transform>());
        newBullet.tag = "shotBullet";
        newBullet.layer = 8;
    }

    //When shooting, play a muzzle flash from the gun
    private void makeMuzzleFlash()
    {
        //Part of this muzzle flash was taken from simpleshoot.cs (the included script with the gun)
        Vector3 tempVector = new Vector3(theGun.transform.position.x, theGun.transform.position.y + 0.1f, theGun.transform.position.z);
        GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, tempVector, theGun.transform.rotation);
        Destroy(muzzleFlash, 0.2f);
    }


    private void addPoints(int increment)
    {
        //Get points from points bar text
        string p = myPoints.text;

        //Increase points by set increment
        int intP = int.Parse(p);
        intP += increment;

        //Update the text
        myPoints.text = intP.ToString();
    }


    private void checkForDropBonus()
    {
        //If two times points is enabled
        if(twoTimes.enabled == true)
        {
            //Set point multiplier to 2
            pointMultiplier = 2;
            ttTimer -= Time.deltaTime;

            //Have this bonus for 30 seconds, then disable it
            if(ttTimer <= 0)
            {
                twoTimes.enabled = false;
                ttTimer = 30f;
            }
        }

        //If there is no bonus, don't change pointsmultiplier (so it does points * 1)
        else
        {
            pointMultiplier = 1;
        }

        //If extreme power i enabled
        if(extremePower.enabled == true)
        {
            //Set the damage multiplier to be 1000 (ensuring a one shot kill)
            dmgMultiplier = 1000;
            epTimer -= Time.deltaTime;

            //Have this bonus for 30 seconds, then disable it
            if(epTimer <= 0)
            {
                extremePower.enabled = false;
                epTimer = 30f;
            }
        }

        //If there is no bonus, don't change dmgMultiplier (so it does damage * 1)
        else
        {
            dmgMultiplier = 1;
        }
    }

    //Checks if you have any perks that enable additional damage
    //If you do, increase the damage multiplier
    //If you don't, don't increase the damage multiplier
    private void checkforPerk()
    {
        if (tripleDMGPerk.enabled == true)
        {
            dmgPerkMultiplier = 3;
        }
        else
        {
            dmgPerkMultiplier = 1;
        }

        if(gunDmgPerk.enabled == true)
        {
            gunPerkDmgMultiplier = 2;
        }
        else
        {
            gunPerkDmgMultiplier = 1;
        }
    }
}
