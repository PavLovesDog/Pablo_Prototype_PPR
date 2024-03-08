using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    PlayerController playerController;
    BackgroundRepeater backgroundRepeater;

    Transform GroundPoint;
    Transform player;

    [SerializeField] private float totalScore = 0;
    [SerializeField] private float scoreMultiplier = 2f;

    //textmeshpro shiz
    public TMP_Text scoreText;

    private void Start()
    {
        //set the start position to track from
        GroundPoint = transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();

        backgroundRepeater = GameObject.FindObjectOfType<BackgroundRepeater>();
    }

    private void Update()
    {
        HandleScoreMultiplier();

        if (playerController.isScoring)
        {
            totalScore += Time.deltaTime * scoreMultiplier; // Testing score based on time spent going up
            int displayScore = (int)Math.Floor(totalScore); // Convert to int for display, effectively truncating
            scoreText.text = "Score: " + displayScore.ToString();
        }
    }


    void HandleScoreMultiplier()
    {
        switch(backgroundRepeater.currArea)
        {
            case AREAS.INNER_CORE:
                scoreMultiplier = 10.0f;
                break;

            case AREAS.OUTER_CORE:
                scoreMultiplier = 12.25f;
                break;

            case AREAS.MANTLE:
                scoreMultiplier = 15.75f;
                break;

            case AREAS.CRUST:
                scoreMultiplier = 18.0f;
                break;

            case AREAS.OCEAN:
                scoreMultiplier = 20.5f;
                break;
        }
    }

}
