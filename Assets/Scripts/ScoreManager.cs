using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    Transform GroundPoint;
    Transform Player;

    public float maxHeightReached;
    public float currentHeightReached;
    public float distToAdd;

    public float maxJumpHeight = 0;

    private void Start()
    {
        //set the start position to track from
        GroundPoint = transform;
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        //track players transform distance from origin, in positive y direction,
        //update current height
        //add it to max height
        //blah blah blah take a break and think this through

        distToAdd = Vector2.Distance(Player.position, GroundPoint.position);
        if(distToAdd > maxJumpHeight )
        {
            maxJumpHeight = distToAdd;
        }
    }


}
