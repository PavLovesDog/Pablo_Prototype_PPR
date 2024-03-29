using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    /// <summary>
    /// TODO:
    ///     - have camera follow player when going off screen to the top and then back to center
    ///     
    /// </summary>
    public Transform target; // The target the camera will follow
    public float smoothSpeed = 0.125f; // Adjust this value to change how quickly the camera follows the target
    public Vector3 offset; // Offset from the target's position

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        //Clamp X position of camera
        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, 0f, 0f);

        transform.position = smoothedPosition;

        // Optionally, if you want the camera to always look at the target, uncomment the line below
        // transform.LookAt(target);
    }
}
