using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Enumn for the areas
public enum AREAS
{
    INNER_CORE,
    OUTER_CORE,
    MANTLE,
    CRUST,
    OCEAN,
    SURFACE,
    SECRET
}

public class BackgroundRepeater : MonoBehaviour
{
    [SerializeField] private AREAS currArea = AREAS.INNER_CORE; // starting 
    [SerializeField] private Transform levelParent;
    PlatformManager platformManager;
    public List<GameObject> backgroundPrefab; // Reference to the background prefab
    public List<GameObject> transitionsPrefabs;//Reference to the transition backgrounds
    private GameObject currAreaBackground;
    //public Vector2 spawnOffset = new Vector2(0, 30); // Offset for spawning the next background
    public int tilesUntilNext = 10; //background tiles need to pass until the area changes
    public float tileHeight = 30; //height for the tiles for spawning the next background tile
    [SerializeField]private int tilesCleared = 0; //keep track of how many tiles we've passed+
    private float spawnThreshold;
    public GameObject lastSpawnedBackgroundUp;
    public GameObject currentSpawnedBackground;
   // public GameObject lastSpawnedBackgroundDown;

    void Start()
    {
        //check that platformManager has been found
        platformManager = FindObjectOfType<PlatformManager>();
        if(platformManager == null)
            Debug.LogError("Could not find paltformManager");
        //tilesUntilNext = Mathf.FloorToInt((tilesUntilNext / 1.7f)*2.3f); // equation to set tileUntilNext value
        tilesUntilNext = Mathf.FloorToInt((tilesUntilNext / 1f) * 0.5f); // CHANGED FOR QUICKER TRANSITION -----TESTING
        // Instantiate the initial background and keep a reference to it
        currAreaBackground = backgroundPrefab[(int)currArea];
        GameObject initialBackground = Instantiate(currAreaBackground, transform.position, Quaternion.identity,levelParent);
        currentSpawnedBackground = initialBackground;
        lastSpawnedBackgroundUp = initialBackground;
      //  lastSpawnedBackgroundDown = initialBackground;
    }

    void Update()
    {

       // check if tiles cleared is equal to tilesUntilNext
        if( tilesCleared >= tilesUntilNext)
        {
       // advance the currArea

            currArea++;

            //  swap the prefabs for environment
            platformManager.SetAreaPlatforms((int)currArea);
            SetAreaBackground();

            tilesUntilNext = Mathf.FloorToInt(1.7f * tilesUntilNext + 2.3f);
        }

        float spawnThresholdBelow = Camera.main.transform.GetChild(0).position.y;
        float spawnThresholdAbove = Camera.main.transform.GetChild(1).position.y;
        // Check if the camera has moved past the point of the last spawned background upwards
        //if (Camera.main.transform.position.y >= lastSpawnedBackgroundUp.transform.position.y)
        if (spawnThresholdBelow + 10 >= lastSpawnedBackgroundUp.transform.position.y) //NOTE added 10 offset to always spawn the background early
        {
            //update current and bottom backgrounds
           // lastSpawnedBackgroundDown = currentSpawnedBackground;
            currentSpawnedBackground = lastSpawnedBackgroundUp;

            ////NOTE Easier to control in inspector as all backgrounds are uniform in size
            //tileHeight = (lastSpawnedBackgroundUp.transform.GetChild(0).GetComponent<SpriteRenderer>().size.y * 1.5f);

            // Calculate the new spawn position above and instantiate
            Vector3 nextSpawnPositionUp = new Vector3(lastSpawnedBackgroundUp.transform.position.x, lastSpawnedBackgroundUp.transform.position.y + tileHeight, lastSpawnedBackgroundUp.transform.position.z);

            //GameObject newBackgroundUp = Instantiate(backgroundPrefab[0], nextSpawnPositionUp, Quaternion.identity);
            GameObject newBackgroundUp = Instantiate(currAreaBackground, nextSpawnPositionUp, Quaternion.identity);
            newBackgroundUp.transform.parent = levelParent.transform;
            // Update the reference to the last spawned background upwards
            lastSpawnedBackgroundUp = newBackgroundUp;
        }
        /*
        // Check if the camera has moved past a certain point of the last spawned background downwards
        else if (spawnThresholdAbove <= lastSpawnedBackgroundDown.transform.position.y)
        {
            //update current background
            lastSpawnedBackgroundUp = currentSpawnedBackground;
            currentSpawnedBackground = lastSpawnedBackgroundDown;

            tileHeight = (lastSpawnedBackgroundUp.transform.GetChild(0).GetComponent<SpriteRenderer>().size.y);
            // Calculate spawning pos below + instantiuate
            Vector3 nextSpawnPositionDown = new Vector3(lastSpawnedBackgroundDown.transform.position.x, lastSpawnedBackgroundDown.transform.position.y - tileHeight, lastSpawnedBackgroundDown.transform.position.z);
            GameObject newBackgroundDown = Instantiate(currAreaBackground, nextSpawnPositionDown, Quaternion.identity);
           newBackgroundDown.transform.parent = levelParent.transform;
            // Update the reference to the last spawned background downwards
            lastSpawnedBackgroundDown = newBackgroundDown;
        }
        */
    }

    //chagne bakcground dpending on the currArea
    private void SetAreaBackground()
    {
        currAreaBackground = backgroundPrefab[((int)currArea)];
    }
}
