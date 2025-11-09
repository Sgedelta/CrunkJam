using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;

public class CatchMGManager : MicroGameManager
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject goodItemPrefab;
    [SerializeField] private GameObject badItemPrefab;
    //[SerializeField] private Transform[] spawnPoints;

    private Vector2[] spawnPoints = new Vector2[7];
    private float spawnXStart = -6f;
    
    private float difficulty;
    private float gameDuration = 10f;
    private float timer;
    private float spawnTimer;
    private int score = 0;
    private bool gameActive = false;
    [SerializeField] private int requiredScore;

    [SerializeField] private float spawnInterval = 1f; //base frequency of spawns
    private int totalGoodSpawned = 0;
    private float itemBaseFallSpeed = 4f; //base fall speed for difficulty 1

    public int currentPosInt = 4;
    private List<GameObject> fruits = new List<GameObject>();
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
        spawnTimer += Time.deltaTime;
        if(spawnTimer > spawnInterval)
        {
            SpawnItems();
            spawnTimer = 0;
        }
        if (!gameActive) return;
        timer += Time.deltaTime;
        if (score > requiredScore){
            GameManager.Instance.EndMicrogame(true);
        }
    }
    private void Start()
    {
        for (int i = 0; i < spawnPoints.Length * 2; i += 2)
        {
            spawnPoints[i/2] = new Vector2(spawnXStart + i, 8.5f);
        }
    }
    public override void LoadScene(){
        //set up the scene to play here, so place the player and the spawners
        player.transform.position = new Vector3(0,-3.5f, 0f);
        timer = 0f;
        score = 0;
        gameActive = true;
        totalGoodSpawned = 0;

        //GenerateSpawnPoints(5);

        //StartCoroutine(SpawnItems());
    }

    //private void GenerateSpawnPoints(int count)
    //{
    //    // Clear existing list
    //    spawnPoints = new Transform[count];

    //    // Use camera bounds to determine visible width
    //    float screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;

    //    // Define top Y coordinate where items spawn
    //    float topY = Camera.main.orthographicSize + 1f;

    //    // Evenly space across visible range
    //    for (int i = 0; i < count; i++)
    //    {
    //        float xPos = Mathf.Lerp(-screenHalfWidth + 1f, screenHalfWidth - 1f, i / (float)(count - 1));
    //        GameObject point = new GameObject($"SpawnPoint_{i}");
    //        point.transform.position = new Vector3(xPos, topY, 0f);
    //        spawnPoints[i] = point.transform;
    //    }
    //}


    public override void UnloadScene(){

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
            MovePlayerLeft();
            //MovePlayer(-1);
        });
        inputManager.OnBHeld.AddListener(() => {
            Debug.Log("B Held Listener");
            MovePlayerRight();
            //MovePlayer(1);
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

    public void MovePlayerRight()
    {
        switch (currentPosInt)
        {
            case 0:
                player.transform.position = new Vector2(spawnPoints[1].x,-3f);
                currentPosInt++;
                break;
            case 1:
                player.transform.position = new Vector2(spawnPoints[2].x, -3f);
                currentPosInt++;
                break;
            case 2:
                player.transform.position = new Vector2(spawnPoints[3].x, -3f);
                currentPosInt++;
                break;
            case 3:
                player.transform.position = new Vector2(spawnPoints[4].x, -3f);
                currentPosInt++;
                break;
            case 4:
                player.transform.position = new Vector2(spawnPoints[5].x, -3f);
                currentPosInt++;
                break;
            case 5:
                player.transform.position = new Vector2(spawnPoints[6].x, -3f);
                currentPosInt++;
                break;
            case 6:

                break;

            default:
                break;
        }
    }
    public void MovePlayerLeft()
    {
        switch (currentPosInt)
        {

            case 1:
                player.transform.position = new Vector2(spawnPoints[0].x, -3f);
                currentPosInt--;
                break;
            case 2:
                player.transform.position = new Vector2(spawnPoints[1].x, -3f);
                currentPosInt--;
                break;
            case 3:
                player.transform.position = new Vector2(spawnPoints[2].x, -3f);
                currentPosInt--;
                break;
            case 4:
                player.transform.position = new Vector2(spawnPoints[3].x, -3f);
                currentPosInt--;
                break;
            case 5:
                player.transform.position = new Vector2(spawnPoints[4].x, -3f);
                currentPosInt--;
                break;
            case 6:
                player.transform.position = new Vector2(spawnPoints[5].x, -3f);
                currentPosInt--;
                break;
            case 7:
                player.transform.position = new Vector2(spawnPoints[6].x, -3f);
                currentPosInt--;
                break;
            default:
                break;
        }
    }

    //private void MovePlayer(int direction)
    //{
    //    if (!gameActive) return;
        
    //    player.transform.position = pos;
    //}

    //private IEnumerator SpawnItems()
    //{
    //    while (gameActive)
    //    {
    //        yield return new WaitForSeconds(spawnInterval);

    //        // random spawn point
    //        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

    //        bool isGood = Random.value > 0.4f; // 60% chance good
    //        if (isGood) totalGoodSpawned++;

    //        GameObject prefab = isGood ? goodItemPrefab : badItemPrefab;
    //        GameObject item = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    //        item.tag = "Item";

    //        // attach a component for item behaviour
    //        var catcher = item.AddComponent<CatchItem>();
    //        catcher.isGood = isGood;
    //        catcher.manager = this;

    //        // make items fall faster with difficulty
    //        catcher.fallSpeed = itemBaseFallSpeed + (difficulty * 0.75f);
    //    }
    //}

    public void SpawnItems()
    {
        int spawnLocation = Random.Range(0, 7);
        if(Random.Range(0, 100) > 60)
        {
            GameObject.Instantiate(badItemPrefab, new Vector2(spawnPoints[spawnLocation].x, 8.5f), Quaternion.identity);
        }
        else
        {
            GameObject goodThing = GameObject.Instantiate(goodItemPrefab, new Vector2(spawnPoints[spawnLocation].x, 8.5f), Quaternion.identity);
            score++;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("AHHHHH");
        if(collision.collider.tag == "Obstacle")
        {
            GameManager.Instance.EndMicrogame(false);
        }
        if(collision.collider.tag == "food")
        {
            collision.collider.gameObject.SetActive(false);
        }
    }
    //public void AddScore(int delta)
    //{
    //    score += delta;
    //    Debug.Log($"Score: {score}");
    //}

    //private void EndGame()
    //{
    //    gameActive = false;
    //    StopAllCoroutines();

    //    // Ensure at least half of the good ones were collected (net 1 score)
    //    requiredScore = totalGoodSpawned/2;

    //    bool win = score > requiredScore;
    //    Debug.Log("Wow!" + ((win)? "You Win!" : "You Lose!"));
    //}
}