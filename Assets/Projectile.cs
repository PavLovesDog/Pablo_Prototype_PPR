using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CleanUpGameObject))]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public Vector2 direction = Vector2.up; // Default direction
    public float speed = 10f; // Speed of the projectile

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Projectile script requires a Rigidbody component.");
            return;
        }
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {

            // Normalize the direction to ensure it's a unit vector, then multiply by speed to get velocity
            Vector3 velocity = direction.normalized * speed;

            // Apply the velocity to the rigidbody
            rb.velocity = velocity;
        }
    }

    //Use this method to change direction of projectile in other scripts
    public void ChangeVelocityDirection(Vector2 newDirection, float speed)
    {
        direction = newDirection.normalized;
        rb.velocity = direction * speed;
    }
}
