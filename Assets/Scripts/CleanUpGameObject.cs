using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanUpGameObject : MonoBehaviour
{
    private Transform playerTransform; // Reference to the player's transform
    private float cleanupThreshold = 80f; // Distance threshold for cleanup
    private float checkFrequency = 1f; // Frequency of cleanup checks in seconds

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(CleanupCheckCoroutine());
    }

    IEnumerator CleanupCheckCoroutine()
    {
        while (true)
        {
            float distanceToPlayer = Mathf.Abs(transform.position.y - playerTransform.position.y);

            if (distanceToPlayer > cleanupThreshold)
            {
                Destroy(gameObject);
                yield break; // Stop the coroutine if the object is destroyed
            }

            yield return new WaitForSeconds(checkFrequency);
        }
    }
}
