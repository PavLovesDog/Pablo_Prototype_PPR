using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO:
///     change so that background changes when scrolling
/// </summary>
public class BackgroundRepeater : MonoBehaviour
{
    [SerializeField] private Transform levelParent;
    public GameObject backgroundPrefab; // Reference to the background prefab
    public Vector2 spawnOffset = new Vector2(0, 30); // Offset for spawning the next background

    public GameObject lastSpawnedBackgroundUp;
    public GameObject currentSpawnedBackground;
    public GameObject lastSpawnedBackgroundDown;

    void Start()
    {
        // Instantiate the initial background and keep a reference to it
        GameObject initialBackground = Instantiate(backgroundPrefab, transform.position, Quaternion.identity,levelParent);
        currentSpawnedBackground = initialBackground;
        lastSpawnedBackgroundUp = initialBackground;
        lastSpawnedBackgroundDown = initialBackground;
    }

    void Update()
    {
        // Check if the camera has moved past the point of the last spawned background upwards
        if (Camera.main.transform.position.y >= lastSpawnedBackgroundUp.transform.position.y)
        {
            //update current and bottom backgrounds
            lastSpawnedBackgroundDown = currentSpawnedBackground;
            currentSpawnedBackground = lastSpawnedBackgroundUp;

            // Calculate the new spawn position above and instantiate
            Vector3 nextSpawnPositionUp = new Vector3(lastSpawnedBackgroundUp.transform.position.x, lastSpawnedBackgroundUp.transform.position.y + spawnOffset.y, lastSpawnedBackgroundUp.transform.position.z);
            GameObject newBackgroundUp = Instantiate(backgroundPrefab, nextSpawnPositionUp, Quaternion.identity);
            newBackgroundUp.transform.parent = levelParent.transform;
            // Update the reference to the last spawned background upwards
            lastSpawnedBackgroundUp = newBackgroundUp;
        }
        // Check if the camera has moved past a certain point of the last spawned background downwards
        else if (Camera.main.transform.position.y <= lastSpawnedBackgroundDown.transform.position.y)
        {
            //update current background
            lastSpawnedBackgroundUp = currentSpawnedBackground;
            currentSpawnedBackground = lastSpawnedBackgroundDown;

            // Calculate spawning pos below + instantiuate
            Vector3 nextSpawnPositionDown = new Vector3(lastSpawnedBackgroundDown.transform.position.x, lastSpawnedBackgroundDown.transform.position.y - spawnOffset.y, lastSpawnedBackgroundDown.transform.position.z);
            GameObject newBackgroundDown = Instantiate(backgroundPrefab, nextSpawnPositionDown, Quaternion.identity);
           newBackgroundDown.transform.parent = levelParent.transform;
            // Update the reference to the last spawned background downwards
            lastSpawnedBackgroundDown = newBackgroundDown;
        }
    }
}
