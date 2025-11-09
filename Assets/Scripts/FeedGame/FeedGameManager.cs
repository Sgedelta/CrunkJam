using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

struct Food
{
    public GameObject gameObject;
    public bool isSent;
    public bool isEaten;
    public int dir;
    public int color;
}

public class FeedGameManager : MicroGameManager
{
    public Sprite cake;
    public Sprite donut;
    public bool feedingLeft = false;
    public bool feedingRight = false;
    public int iterator = 0;

    private bool isGameOver = false;
    private bool isGameWon = false;
    [Header("Initial Timer Value")]
    [SerializeField] private float timer;

    [Header("Red Guy")]
    [SerializeField] private GameObject redGuy;
    [Header("Blue Guy")]
    [SerializeField] private GameObject blueGuy;

    [Header("TimerUI")]
    [SerializeField] private Text timerTextUI;

    private GameManager gameManagerScript;

    Vector2 screenMin;
    Vector2 screenMax;

    // Array containers to hold our food items
    private GameObject[] foodsContainer;
    private Food[] foods;

    // How fast the food moves across the screen
    private int moveSpeed = 10;

    // Keeps track of how many times the player made the correct choice in feeding aliens
    public int count;

    public override void Initialize(InputManager im)
    {
        inputManager = im;
        BindInput();

        // load the scene
        LoadScene();

        // grab reference to Game Manager script

        gameManagerScript = GameManager.Instance;

        // get min and max of the screen from camera viewport
        screenMin = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        screenMax = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // initialize timer text ui
        timerTextUI.text = string.Format("{0:F2}", timer);

    }

    public override void LoadScene()
    {
        // load scene
    }

    public override void UnloadScene()
    {
        // unload scene
    }

    protected override void BindInput()
    {
        //inputManager.OnAPressed.AddListener(FeedLeft);
        //inputManager.OnBPressed.AddListener(FeedRight);
        inputManager.OnAHeld.AddListener(FeedLeft);
        inputManager.OnBHeld.AddListener(FeedRight);
        
        inputManager.OnAHoldReleased.AddListener(FeedLeftReset);
        inputManager.OnBHoldReleased.AddListener(FeedRightReset);
        inputManager.OnBothHoldReleased.AddListener(FeedBothReset);
    }

    private void Start()
    {
        foodsContainer = GameObject.FindGameObjectsWithTag("food");
        int x = GameManager.Instance.Difficulty + 5;
        foods = new Food[x];

        for (int i = 0; i < foodsContainer.Length; i++)
        {
            foods[i].gameObject = foodsContainer[i];
            foods[i].gameObject.transform.position = new Vector3((screenMin.x + screenMax.x) / 2, (screenMin.y + screenMax.y)/2, 2);
            foods[i].color = GetRandomAOrB(-1, 1);
            if (foods[i].color == -1)
                foods[i].gameObject.GetComponent<SpriteRenderer>().sprite = donut;
            else if (foods[i].color == 1)
                foods[i].gameObject.GetComponent<SpriteRenderer>().sprite = cake;
            foods[i].gameObject.GetComponent<FoodItem>().Color = foods[i].color;
            Debug.Log(foods[i].color.ToString() + " " + foods[i].gameObject.GetComponent<SpriteRenderer>().sprite.name);
        }
    }

    private void Update()
    {
        if (isGameOver || isGameWon) return;

        UpdateTimer();

        if(foods.Length > 0 && iterator < foods.Length)
        {
            UpdateFoods();
        }
        else
        {
            GameOver();
        }
    }

    public void FeedLeft()
    {
        feedingLeft = true;
    }

    public void FeedRight()
    {
        feedingRight = true;
    }

    public void FeedLeftReset()
    {
        feedingLeft = false;
    }

    public void FeedRightReset()
    {
        feedingRight = false;
    }

    public void FeedBothReset()
    {
        feedingLeft = feedingRight = false;
    }

    /// <summary>
    ///  Chooses the direction the food moves in 
    /// </summary>
    public void UpdateFoods()
    {
        if (feedingLeft && !foods[iterator].isSent)
        {
            foods[iterator].dir = -1;
            foods[iterator].isSent = true;
        }

        if (feedingRight && !foods[iterator].isSent)
        {
            foods[iterator].dir = 1;
            foods[iterator].isSent = true;
        }

        MoveFood();
    }

    /// <summary>
    /// Moves the Food left or right across the screen
    /// </summary>
    public void MoveFood()
    {
        Food food = foods[iterator];

        Vector2 moveDir = Vector2.zero;
        if (food.dir == 1)
        {
            moveDir.x += moveSpeed * Time.deltaTime;
        }
        else if (food.dir == -1)
        {
            moveDir.x -= moveSpeed * Time.deltaTime;
        }

        food.gameObject.transform.position = new Vector3(food.gameObject.transform.position.x + moveDir.x, food.gameObject.transform.position.y, 0);
    }

    /// <summary>
    /// Logic for updating timer variable
    /// </summary>
    public void UpdateTimer()
    {
        if (!isGameWon)
        {
            timer -= Time.deltaTime;
        }

        timerTextUI.text = string.Format("{0:F2}", timer);

        if (timer <= 0)
        {
            GameOver();
        }
    }

    /// <summary>
    /// On Game Won
    /// </summary>
    public void GameWon()
    {
        isGameWon = true;
        gameManagerScript.EndMicrogame(true);
        Debug.Log("Game Won");
    }

    /// <summary>
    /// On Game Over
    /// </summary>
    public void GameOver()
    {
        isGameOver = true;
        gameManagerScript.EndMicrogame(false);
        Debug.Log("Game Lost");
    }

    /// <summary>
    /// Sets the necessary variables for the next food item 
    /// </summary>
    public void ReadyNextFood()
    {
        iterator++;
        if (iterator < 5)
            foods[iterator].gameObject.transform.position = new Vector3((screenMin.x + screenMax.x) / 2, (screenMin.y + screenMax.y) / 2, -1);
        else
        {
            GameWon();
        }
        feedingLeft = false;
        feedingRight = false;
    }

    /// <summary>
    /// Helper method to get a random number, either A or B
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public int GetRandomAOrB(int a, int b)
    {
        int randNum = Random.Range(0, 2);

        if (randNum == 0)
        {
            return a;
        }
        else
        {
            return b;
        }
    }
}
