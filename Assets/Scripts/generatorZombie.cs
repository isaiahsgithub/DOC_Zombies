using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class generatorZombie : MonoBehaviour
{

    [Header("Enemy")]
    [SerializeField] public GameObject enemyPrefab;

    [Header("Generation Specifics")]
    [SerializeField] private Text theRound;
    private int numMobs = 8;
    private int mobRemain = 0;
    private int currentRound;
    [SerializeField] private Text zombieRemainText;

    [Header("Generation Location")]
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private GameObject[] requirements;

    private void Update()
    {
        //Checks how many enemies remain
        int mobRemain = transform.childCount;
        zombieRemainText.text = "Remaining Zombies: " + mobRemain;
        if (mobRemain <= 0)
        {
            //If you kill all the zombies for a given round, you've completed the round. Increment the round
            currentRound = int.Parse(theRound.text) + 1;
            theRound.text = currentRound.ToString();

            //Preset number of mobs per round. Spawns in all the zombies and sets their parent equal to the generator.
            numMobs = 8 + (3*currentRound);
            for (int i=0; i<numMobs; i++)
            {
                GameObject newEnemy = Instantiate(enemyPrefab, whichSpawnLocs(), Quaternion.identity);
                newEnemy.transform.SetParent(transform);

            }
        }
    }
    private int getRandomIntSpawnLoc()
    {
        return Random.Range(0, spawnLocations.Length);
    }


    //These two functions checks what are the possible spawn points a zombie can spawn in.
    //If no doors are open, then they can only spawn in SpawnLoc[0]
    //If door 1 is open, they can spawn in SpawnLoc[0], SpawnLoc[1], and SpawnLoc[2]
    //If door 2 is open, they can spawn in all spawns. (SpawnLoc[0] - SpawnLoc[3])
    //When a door is opened, it is moved to -699f in the Y axis. This is so we can check 
    //if a zombie can be spawned in a certain location.
    private bool isDoorOpen(int placeToCheck)
    {
        if(placeToCheck == 0)
        {
            return true;
        }
        if(placeToCheck == 1 || placeToCheck == 2)
        {
            if(requirements[0].transform.position.y <= -699)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        if(placeToCheck == 3)
        {
            if(requirements[1].transform.position.y <= -699)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private Vector3 whichSpawnLocs()
    {
        //If no door's are open, then the zombie can only spawn in SpawnLoc[0]
        bool anyOpen = false;
        foreach(GameObject curDor in requirements)
        {
            if(curDor.transform.position.y <= -699f)
            {
                anyOpen = true;
            }

        }

        //If there are no open doors, don't bother randomly trying to find a spawn loc, just return
        //spawnloc[0]
        if(anyOpen == false)
        {
            return spawnLocations[0].transform.position;
        }

        //Max 10000 iterations for each zombie spawn, if somehow it times out, it will return 
        //spawnloc[0]
        //This searches for an available spawnlocation for a zombie.
        Vector3 mySpawnVector = spawnLocations[0].transform.position;
        int i = 0;
        while (i < 10000)
        {
            int index = getRandomIntSpawnLoc();
            if(isDoorOpen(index) == true)
            {
                mySpawnVector = spawnLocations[index].transform.position;
                break;
            }

            i += 1;
        }
        return mySpawnVector;
    }


}
