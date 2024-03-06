//using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class PlatformManager : MonoBehaviour
{
    private bool platformGo = true; //tells the program when platforms are moving/adjusting for player
    [SerializeField]  private Transform levelParent;
    public GameObject platformPrefab;
    GameObject lastPlacedPlatform;
    public int numberOfPlatforms = 6; // No. of platforms to keep in the pool
    private float verticalSpawnSpacing = 5f; // Threshhold height for spawning platforms / Vertical spacing between platforms
    public float minVerticalSpacing = 2f; //Minimum height for platform spawn
    public float maxVerticalSpacing = 6f; //Maximum height 
    public float minXOffset = -1f; // Minimum width for platform spawn
    public float maxXOffset = 1f; // Maximum width

    public Vector2 scaleRange = new Vector2(0.8f, 1.2f); // Range of scale sizes for platforms (x-axis scaling).

    private List<GameObject> platforms = new List<GameObject>();
    
    private float nextSpawnY;

    void Start()
    {
        platforms.Add(levelParent.Find("Platform - Stone_Brick").gameObject);

        lastPlacedPlatform = platforms[0];
        
        for (int i = 0; i < numberOfPlatforms; i++)
        {
            SpawnPlatform();
        }
    }
    /// <summary>
    /// TODO:
    /// - Fix platform spawning
    /// </summary>
    void Update()
    {
        float spawnThreshold = Camera.main.transform.GetChild(0).position.y;
        if (platforms[0].transform.position.y < spawnThreshold && platformGo)
        {
            //SpawnPlatform();
            RecyclePlatform();
        }
       
        

        //spawn platforms based on camera position
        /*
        if (Camera.main.transform.position.y >= nextSpawnY - (numberOfPlatforms * verticalSpawnSpacing / 2))
        {
            SpawnPlatform();
            RecyclePlatform();
        }
        */
    }

    void SpawnPlatform()
    {
        //grab next Y position for the next platform
        nextSpawnY = Random.Range(lastPlacedPlatform.transform.position.y + minVerticalSpacing , lastPlacedPlatform.transform.position.y + maxVerticalSpacing);
        //determine position and spawn
        float platformXPosition = Random.Range(minXOffset, maxXOffset);
        GameObject newPlatform = Instantiate(platformPrefab, new Vector2(platformXPosition, nextSpawnY), Quaternion.identity,levelParent.transform);

        // Randomly adjust the platform's scale on the x-axis.
        float scaleX = Random.Range(scaleRange.x, scaleRange.y);
        newPlatform.transform.localScale = new Vector3(scaleX, newPlatform.transform.localScale.y, newPlatform.transform.localScale.z);
        newPlatform.transform.parent = levelParent.transform;

        platforms.Add(newPlatform);
        lastPlacedPlatform = newPlatform;
        //nextSpawnY += Random.Range(minVerticalSpacing, maxVerticalSpacing); // randomize Y position for nextr spawn
    }

    void RecyclePlatform()
    {
        nextSpawnY = Random.Range(lastPlacedPlatform.transform.position.y + minVerticalSpacing, lastPlacedPlatform.transform.position.y + maxVerticalSpacing);
        // Recycle the oldest platform in the queue by moving it to the next spawn position.
        GameObject recycledPlatform = platforms.First();
        platforms.Remove(platforms.First());
        recycledPlatform.transform.position = new Vector2(Random.Range(minXOffset, maxXOffset), nextSpawnY);

        // randomize the scale again for new platform
        float scaleX = Random.Range(scaleRange.x, scaleRange.y);
        recycledPlatform.transform.localScale = new Vector3(scaleX, recycledPlatform.transform.localScale.y, recycledPlatform.transform.localScale.z);

        //add back into queue and randomze spacing
        platforms.Add(recycledPlatform);
        lastPlacedPlatform = recycledPlatform;
       // nextSpawnY = Random.Range(recycledPlatform.transform.position.y + minVerticalSpacing, recycledPlatform.transform.position.y + maxVerticalSpacing);

       // nextSpawnY += Random.Range(minVerticalSpacing, maxVerticalSpacing);
    }

    //NOTE TEMP PLACEHOLDER, Will need to move platforms like a smooth physiscs jump
    public void OnPlayerJump(float jumpHeight)
    {
        foreach (GameObject platform in platforms)
        {
            // Move each platform down by jumpHeight
            platform.transform.position -= new Vector3(0, jumpHeight, 0);
        }

        // Adjust nextSpawnY as well to keep future spawns aligned
        nextSpawnY -= jumpHeight;
    }

    public void SwitchLevelPosUpdate()
    {
        
    }
    public Transform GetLevel()
    {
        return levelParent;
    }
}
