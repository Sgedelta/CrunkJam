using UnityEngine;
using System.Collections;
using System.Colelctions.Generic;

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
    private int score;
    private bool gameActive = false;
    private int requiredScore;

    private float spawnInterval = 1f; //base frequency
    private int totalGoodSpawned = 0;



    public override void Initialize(InputManager im){
        InputManager = im;
        BindInput();

        difficulty =  GameManager.Instance.Difficulty;
        requiredScore = Mathf.RoundToInt(difficulty * 5) //JUST IN CASE DIFFICULTY  IS A FLOAT (WHICH I DONT THINK IT IS)
        

        //adjust difficulty scaled stuff

        moveSpeed = 4f + difficulty * 1.5f;
        spawnInterval = Mathf.Max(0.4f, 1.2f - (difficulty * 0.3f));


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
    }

    public override void UnloadScene(){
        //should not require any unloading right now
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
        });
        inputManager.OnBHeld.AddListener(() => {
            Debug.Log("B Held Listener");
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



}
