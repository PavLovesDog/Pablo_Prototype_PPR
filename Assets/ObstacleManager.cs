using System.Collections;
using System.Collections.Generic;
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

    //timers
    public float timer;
    public float timeBetweenSpawns;

    // Start is called before the first frame update
    void Start()
    {
        backgroundRepeater = GameObject.FindObjectOfType<BackgroundRepeater>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleAreaObstacles();
    }

    void HandleAreaObstacles()
    {
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

    private void HandleCrustObstacles()
    {
        
    }

    private void HandleMantleObstacles()
    {
        
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

        timer += Time.deltaTime;

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
                //apply new velocity using the projjectile script function
                projectileScript.ChangeVelocityDirection(new Vector2(adjustedDirection, projectileScript.direction.y),
                                                         Random.Range(projectileScript.speed - 5, projectileScript.speed + 2));
            }

            timeBetweenSpawns = Random.Range(1f, 3f);
            timer = 0;
        }
    }
}
