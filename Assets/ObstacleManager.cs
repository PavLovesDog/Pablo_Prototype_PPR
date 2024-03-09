using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    BackgroundRepeater backgroundRepeater;
    [SerializeField] private Transform levelParent;

    //Spawn Points
    public Transform[] leftPos;
    public Transform[] rightPos;
    public Transform[] topPos;

    //Prefabs
    public GameObject fireballPrefab;
    public GameObject[] mantleSpikePrefab;
    public GameObject[] crustSpikePrefab;
    //timers
    public float timer;
    public float timeBetweenSpawns;

    // Start is called before the first frame update
    void Start()
    {
        timeBetweenSpawns = Random.Range(2f, 6f);
        backgroundRepeater = GameObject.FindObjectOfType<BackgroundRepeater>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleAreaObstacles();
    }

    void HandleAreaObstacles()
    {
        timer += Time.deltaTime;
        switch (backgroundRepeater.currArea)
        {
            case AREAS.INNER_CORE:
                //do nothing?
                break;

            case AREAS.OUTER_CORE:
                HandleOuterCoreObstacles();
                break;

            case AREAS.MANTLE:
                HandleMantleObstacles();
                break;

            case AREAS.CRUST:
                HandleCrustObstacles();
                break;

            case AREAS.OCEAN:
                HandleOceanObstacles();
                break;
        }
    }

    private void HandleOceanObstacles()
    {

    }
    //this will be the same as HandleMantleObstacles with crustSpikePrefab instead of mantleSpikePrefab
    private void HandleCrustObstacles()
    {
        // increment timer

        int randomIndex = 0;
        Transform spawnPos;
        // if timer > timeBetweenSpawns
        if (timer >= timeBetweenSpawns)
        {
            // determine whether to spawn on sides or drop down. Random as a random change
            int sideOrTop = Random.value > 0.5f ? 0 : 1;

            // if spawn side then
            if (sideOrTop == 0)
            {
                print("side");
                // pick a left or right position
                Transform[] direction = Random.value > 0.5 ? leftPos : rightPos;
                randomIndex = Random.Range(0, direction.Length);
                spawnPos = direction[randomIndex];
                // spawn wall spike at position as a child of level (this should ensure it moves with the level
                randomIndex = Random.Range(0, crustSpikePrefab.Length);
                float facing = direction == leftPos ? -90 : 90;
                //change the spikes facing direction
                Quaternion rotation = Quaternion.AngleAxis(facing, Vector3.forward);
                GameObject go = Instantiate(crustSpikePrefab[randomIndex], spawnPos.position, rotation, levelParent);
            }
            // if spawn top then
            else if (sideOrTop == 1)
            {
                print("top");
                // pick a top position spawn point
                randomIndex = Random.Range(0, topPos.Length);
                spawnPos = topPos[randomIndex];
                // spawn spike with projectile script as a child of the level object
                randomIndex = Random.Range(0, crustSpikePrefab.Length);
                //set the spikes facing direction to down
                Quaternion rotation = new Quaternion(0, 0, 180, 1);
                GameObject go = Instantiate(crustSpikePrefab[randomIndex], spawnPos.position, rotation, levelParent);

                // set prjectile direction to down and set speed
                Projectile proj = go.GetComponent<Projectile>();
                if (proj != null)
                {
                    proj.ChangeVelocityDirection(Vector2.down, Random.Range(0.5f, 5f));
                }
            }
            // set the timeBetweenSpawns to a random number
            timeBetweenSpawns = Random.Range(2f, 6f);
            // reset timer
            timer = 0;
            // EditorApplication.isPaused = true;
        }
    }

    private void HandleMantleObstacles()
    {
        // increment timer

        int randomIndex = 0;
        Transform spawnPos;
        // if timer > timeBetweenSpawns
        if (timer >= timeBetweenSpawns)
        {
            // determine whether to spawn on sides or drop down. Random as a random change
            int sideOrTop = Random.value > 0.5f ? 0 : 1;

            // if spawn side then
            if (sideOrTop == 0)
            {
                print("side");
                // pick a left or right position
                Transform[] direction = Random.value > 0.5 ? leftPos : rightPos;
                randomIndex = Random.Range(0, direction.Length);
                spawnPos = direction[randomIndex];
                // spawn wall spike at position as a child of level (this should ensure it moves with the level
                randomIndex = Random.Range(0, mantleSpikePrefab.Length);
                float facing = direction == leftPos ? -90 : 90;
                //change the spikes facing direction
                Quaternion rotation = Quaternion.AngleAxis(facing, Vector3.forward);
                GameObject go = Instantiate(mantleSpikePrefab[randomIndex], spawnPos.position, rotation, levelParent);
            }
            // if spawn top then
            else if (sideOrTop == 1)
            {
                print("top");
                // pick a top position spawn point
                randomIndex = Random.Range(0, topPos.Length);
                 spawnPos = topPos[randomIndex];
                // spawn spike with projectile script as a child of the level object
                randomIndex = Random.Range(0, mantleSpikePrefab.Length);
                //set the spikes facing direction to down
                Quaternion rotation= new Quaternion(0, 0, 180, 1);
                GameObject go = Instantiate(mantleSpikePrefab[randomIndex], spawnPos.position, rotation, levelParent);


                // set prjectile direction to down and set speed
                Projectile proj = go.GetComponent<Projectile>();
                if (proj != null)
                {
                    proj.ChangeVelocityDirection(Vector2.down, Random.Range(0.5f, 5f));
                }
            }
            // set the timeBetweenSpawns to a random number
            timeBetweenSpawns = Random.Range(2f, 6f);
            // reset timer
            timer = 0;
           // EditorApplication.isPaused = true;
        }

    }

    private void HandleOuterCoreObstacles()
    {
        // increment timer
        // if timer > timeBetweenSpawns
        // select a left or right position at random
        // Instantiate fireball at selected position
        // if left, make sure x direction is positive
        //use method in Projectile class to invert the x direction
        // if right, make the velocity is negative
        //use method in Projectile class
        //set timeBetweenSpawns to a random number between 2 - 6
        //reset timer
        if (timer > timeBetweenSpawns)
        {
            // Select a spawn position array at random (left or right)
            Transform[] spawnPositions = Random.value > 0.5f ? leftPos : rightPos;
            // Choose a random index for the selected spawn position array
            int randomIndex = Random.Range(0, spawnPositions.Length);
            Transform spawnPosition = spawnPositions[randomIndex];

            // Instantiate the fireball, child it to level so it moves with world
            GameObject fireball = Instantiate(fireballPrefab, spawnPosition.position, Quaternion.identity, levelParent.transform);

            // Adjust the fireball's velocity direction based on the spawn side
            Projectile projectileScript = fireball.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                // If spawned on the left, make x direction velocity positive; if on the right, negative
                float adjustedDirection = spawnPositions == leftPos ? 1f : -1f;
                fireball.GetComponentInChildren<SpriteRenderer>().flipX = adjustedDirection == 1f ? true : false;
                //apply new velocity using the projjectile script function
                projectileScript.ChangeVelocityDirection(new Vector2(adjustedDirection, projectileScript.direction.y),
                                                         Random.Range(projectileScript.speed - 5, projectileScript.speed + 2));
            }

            timeBetweenSpawns = Random.Range(1f, 3f);
            timer = 0;
        }
    }
}
