//using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject platformPrefab;
    public int numberOfPlatforms = 6; // No. of platforms to keep in the pool
    private float verticalSpawnSpacing = 5f; // Vertical spacing between platforms
    public float minVerticalSpacing = 2f; //Minimum height for platform spawn
    public float maxVerticalSpacing = 6f; //Maximum height 
    public float minXOffset = -1f; // Minimum width for platform spawn
    public float maxXOffset = 1f; // Maximum width

    public Vector2 scaleRange = new Vector2(0.8f, 1.2f); // Range of scale sizes for platforms (x-axis scaling).

    private Queue<GameObject> platforms = new Queue<GameObject>();
    private float nextSpawnY;

    void Start()
    {
        nextSpawnY = transform.position.y;
        for (int i = 0; i < numberOfPlatforms; i++)
        {
            SpawnPlatform();
        }
    }

    void Update()
    {
        //spawn platforms based on camera position
        if (Camera.main.transform.position.y >= nextSpawnY - (numberOfPlatforms * verticalSpawnSpacing / 2))
        {
            SpawnPlatform();
            RecyclePlatform();
        }
    }

    void SpawnPlatform()
    {
        //determine position and spawn
        float platformXPosition = Random.Range(minXOffset, maxXOffset);
        GameObject newPlatform = Instantiate(platformPrefab, new Vector2(platformXPosition, nextSpawnY), Quaternion.identity);

        // Randomly adjust the platform's scale on the x-axis.
        float scaleX = Random.Range(scaleRange.x, scaleRange.y);
        newPlatform.transform.localScale = new Vector3(scaleX, newPlatform.transform.localScale.y, newPlatform.transform.localScale.z);

        platforms.Enqueue(newPlatform);
        nextSpawnY += Random.Range(minVerticalSpacing, maxVerticalSpacing); // randomize Y position for nextr spawn
    }

    void RecyclePlatform()
    {
        // Recycle the oldest platform in the queue by moving it to the next spawn position.
        GameObject recycledPlatform = platforms.Dequeue();
        recycledPlatform.transform.position = new Vector2(Random.Range(minXOffset, maxXOffset), nextSpawnY);

        // randomize the scale again for new platform
        float scaleX = Random.Range(scaleRange.x, scaleRange.y);
        recycledPlatform.transform.localScale = new Vector3(scaleX, recycledPlatform.transform.localScale.y, recycledPlatform.transform.localScale.z);

        //add back into queue and randomze spacing
        platforms.Enqueue(recycledPlatform);
        nextSpawnY += Random.Range(minVerticalSpacing, maxVerticalSpacing);
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
}
