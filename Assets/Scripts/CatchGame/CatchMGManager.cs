using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CatchMGManager : MicroGameManager
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject goodItemPrefab;
    [SerializeField] private GameObject badItemPrefab;
    [SerializeField] private Transform[] spawnPoints;
    
    private float difficulty;
    private float gameDuration = 10f;
    private float timer;
    private int score = 0;
    private bool gameActive = false;
    private int requiredScore;

    private float spawnInterval = 1f; //base frequency of spawns
    private int totalGoodSpawned = 0;
    private float itemBaseFallSpeed = 4f; //base fall speed for difficulty 1


    public override void Initialize(InputManager im){
        inputManager = im;
        BindInput();

        difficulty =  GameManager.Instance.Difficulty;
       

        //adjust difficulty scaled stuff

        moveSpeed = 4f + difficulty * 0.5f;
        spawnInterval = Mathf.Max(0.25f, 1.2f - (difficulty * 0.1f));

        LoadScene();
    }

    private void Update()
    {
        if (!gameActive) return;
        timer += Time.deltaTime;
        if (timer >= gameDuration){
            EndGame();
        }
    }

    public override void LoadScene(){
        //set up the scene to play here, so place the player and the spawners
        player.transform.position = new Vector3(0,-3.5f, 0f);
        timer = 0f;
        score = 0;
        gameActive = true;
        totalGoodSpawned = 0;

        GenerateSpawnPoints(5);

        StartCoroutine(SpawnItems());
    }

    private void GenerateSpawnPoints(int count)
    {
        // Clear existing list
        spawnPoints = new Transform[count];

        // Use camera bounds to determine visible width
        float screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;

        // Define top Y coordinate where items spawn
        float topY = Camera.main.orthographicSize + 1f;

        // Evenly space across visible range
        for (int i = 0; i < count; i++)
        {
            float xPos = Mathf.Lerp(-screenHalfWidth + 1f, screenHalfWidth - 1f, i / (float)(count - 1));
            GameObject point = new GameObject($"SpawnPoint_{i}");
            point.transform.position = new Vector3(xPos, topY, 0f);
            spawnPoints[i] = point.transform;
        }
    }


    public override void UnloadScene(){
        StopAllCoroutines();
        gameActive = false;
        foreach (var obj in GameObject.FindGameObjectsWithTag("Item")){
            Destroy(obj);
        }
    }

    
    protected override void BindInput()
    {
        inputManager.OnAPressed.AddListener(() => {
            Debug.Log("A Pressed Listener");
            });
        inputManager.OnBPressed.AddListener(() => {
            Debug.Log("B Pressed Listener");
        });

        inputManager.OnAHeld.AddListener(() => {
            Debug.Log("A Held Listener");
            MovePlayer(-1);
        });
        inputManager.OnBHeld.AddListener(() => {
            Debug.Log("B Held Listener");
            MovePlayer(1);
        });

        inputManager.OnAHoldReleased.AddListener(() => {
            Debug.Log("A Released Listener");
        });
        inputManager.OnBHoldReleased.AddListener(() => {
            Debug.Log("B Released Listener");
        });

        inputManager.OnBothPressed.AddListener(() =>
        {
            Debug.Log("Both Pressed Listener"); 
        });

        inputManager.OnBothHeld.AddListener(() =>
        {
            Debug.Log("Both Held Listener");
        });

        inputManager.OnBothHoldReleased.AddListener(() =>
        {
            Debug.Log("Both Released Listener");
        });
    }

    private void MovePlayer(int direction)
    {
        if (!gameActive) return;
        Vector3 pos = player.transform.position;
        pos.x += direction * moveSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, -7f, 7f);
        player.transform.position = pos;
    }

    private IEnumerator SpawnItems()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(spawnInterval);

            // random spawn point
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            bool isGood = Random.value > 0.4f; // 60% chance good
            if (isGood) totalGoodSpawned++;

            GameObject prefab = isGood ? goodItemPrefab : badItemPrefab;
            GameObject item = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            item.tag = "Item";

            // attach a component for item behaviour
            var catcher = item.AddComponent<CatchItem>();
            catcher.isGood = isGood;
            catcher.manager = this;

            // make items fall faster with difficulty
            catcher.fallSpeed = itemBaseFallSpeed + (difficulty * 0.75f);
        }
    }
    public void AddScore(int delta)
    {
        score += delta;
        Debug.Log($"Score: {score}");
    }

    private void EndGame()
    {
        gameActive = false;
        StopAllCoroutines();

        // Ensure at least half of the good ones were collected (net 1 score)
        requiredScore = totalGoodSpawned/2;

        bool win = score > requiredScore;
        Debug.Log("Wow!" + ((win)? "You Win!" : "You Lose!"));
    }
}