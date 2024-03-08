using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private Collider2D[] platformColliders;
    public Transform playerTransform;

    void Start()
    {
        platformColliders = GetComponentsInChildren<Collider2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
       
        

        // Activate the collider when the player is above the platform and deactivate it otherwise
        if (playerTransform.position.y > transform.position.y + 0.45f)
        {
            foreach(Collider2D collider in platformColliders) 
            { collider.enabled = true; }
        }
        else if(playerTransform.position.y < transform.position.y)
        {
            foreach (Collider2D collider in platformColliders)
            { collider.enabled = false; }
        }
    }
}
