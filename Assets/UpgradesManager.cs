using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    public GameObject jumpBootsPrefab;
    public GameObject dashBootsPrefab;
    public Transform levelReference;
    public BackgroundRepeater backgroundRepeaterReference;

    public AREAS jumpBootsSpawnArea = AREAS.INNER_CORE;
    public AREAS dashBootsSpawnArea;

    private bool jumpBootsAlreadySpawned = false;
    private bool dashBootsAlreadySpawned = false;
    private float timer = 0f;
    private float timeUntilNextUpgradeSpawn;
    // Start is called before the first frame update
    void Start()
    {
        
        timeUntilNextUpgradeSpawn = Random.Range(2f, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeUntilNextUpgradeSpawn)
        {
            HandleAreas();
        }
    }

    void HandleAreas()
    {
        AREAS cArea = backgroundRepeaterReference.currArea;
        if (cArea == jumpBootsSpawnArea)
        {
            SpawnJumpBoots();
        } 
        else if (cArea == dashBootsSpawnArea)
        {
            SpawnDashBoots();
        }
    }
    //Spawn dashing boots
    void SpawnDashBoots()
    {
        if (!dashBootsAlreadySpawned) {
            Vector2 randomSpawn = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
            randomSpawn = Camera.main.ViewportToWorldPoint(randomSpawn);
            GameObject go = Instantiate(dashBootsPrefab, randomSpawn, Quaternion.identity, levelReference);
            go.name = "DashBoots";
            timeUntilNextUpgradeSpawn = Random.Range(0f, 5f);
            timer = 0;
            dashBootsAlreadySpawned = true;
        }
    }
    //spawn jumping boots
    void SpawnJumpBoots()
    {
        if (!jumpBootsAlreadySpawned)
        {
            Vector2 randomSpawn = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
            randomSpawn = Camera.main.ViewportToWorldPoint(randomSpawn);
            GameObject go = Instantiate(jumpBootsPrefab, randomSpawn, Quaternion.identity, levelReference);
            go.name = "JumpBoots";
            timeUntilNextUpgradeSpawn = Random.Range(0f, 5f);
            timer = 0;
            jumpBootsAlreadySpawned = true;
        }
    }
}

