using UnityEngine;
using UnityEngine.UI;

public class CraneGameManager : MicroGameManager
{
    private bool isGameOver = false;
    public bool isGameWon = false;
    public bool allowInput = true;
    [Header("Initial Timer Value")]
    [SerializeField] private float timer;
    private float gameOverThreshold = 0;

    private float craneDownSpeed = 2;

    [Header("Player")]
    [SerializeField] private GameObject playerObj;
    private Crane playerScript;
    private SpriteRenderer playerSpriteRenderer;
    private float playerHalfHeight;
    private float playerHalfWidth;

    [Header("Alien")]
    [SerializeField] private GameObject alienObj;
    private Alien alienScript;
    private SpriteRenderer alienSpriteRenderer;
    private float alienHalfHeight;
    private float alienHalfWidth;

    [Header("TimerUI")]
    [SerializeField] private Text timerTextUI;

    [Header("GameManager")]
    [SerializeField] private GameObject gameManagerObj;

    private GameManager gameManagerScript;

    Vector2 screenMin;
    Vector2 screenMax;

    public override void Initialize(InputManager im)
    {
        inputManager = im;
        BindInput();

        // load the scene
        LoadScene();

        if (playerObj)
        {
            playerScript = playerObj.GetComponent<Crane>();
            playerSpriteRenderer = playerObj.GetComponent<SpriteRenderer>();
            playerHalfHeight = playerSpriteRenderer.bounds.extents.y;
            playerHalfWidth = playerSpriteRenderer.bounds.extents.x;
        }

        if (alienObj)
        {
            alienScript = alienObj.GetComponent<Alien>();
            alienSpriteRenderer = alienObj.GetComponent<SpriteRenderer>();
            alienHalfHeight = alienSpriteRenderer.bounds.extents.y;
            alienHalfWidth = alienSpriteRenderer.bounds.extents.x;
        }

        // grab reference to Game Manager script
        if (gameManagerObj)
        {
            gameManagerScript = gameManagerObj.GetComponent<GameManager>();
        }

        // get min and max of the screen from camera viewport
        screenMin = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        screenMax = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // initialize timer text ui
        timerTextUI.text = string.Format("{0:F2}", timer);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;

        UpdateTimer();
        CheckBounds();

        if (timer <= gameOverThreshold)
        {
            allowInput = false;
        }

        if (!allowInput && !isGameWon)
        {
            CraneDown();
        }

        if (playerObj.transform.position.y - playerHalfHeight <= screenMin.y)
        {
            if (gameManagerObj.GetComponent<GameManager>() != null)
            {
                Debug.Log("You lose");
                gameManagerObj.GetComponent<GameManager>().EndMicrogame(false);
            }
        }
    }

    public void CraneDown()
    {
        playerObj.transform.position = new Vector3(playerObj.transform.position.x, playerObj.transform.position.y - craneDownSpeed * Time.deltaTime, 0);
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
        //inputManager.OnAPressed.AddListener(MoveLeft);
        //inputManager.OnBPressed.AddListener(MoveRight);
        inputManager.OnAHeld.AddListener(MoveLeft);
        inputManager.OnBHeld.AddListener(MoveRight);

        inputManager.OnAHoldReleased.AddListener(MoveLeftReset);
        inputManager.OnBHoldReleased.AddListener(MoveRightReset);

        //inputManager.OnBothHoldReleased.AddListener(MoveBothDirectionsReset);
    }

    public void MoveLeft()
    {
        if(allowInput) playerScript.MoveLeft();
    }

    public void MoveRight()
    {
        if (allowInput) playerScript.MoveRight();
    }

    public void MoveLeftReset()
    {
        playerScript.MoveLeftReset();
    }

    public void MoveRightReset()
    {
        playerScript.MoveRightReset();
    }

    public void MoveBothDirectionsReset()
    {
        playerScript.MoveRightReset();
        playerScript.MoveLeftReset();
    }

    /// <summary>
    /// Check screen bounds so player stays within the screen
    /// </summary>
    private void CheckBounds()
    {
        // keep player within bounds of the screen
        float clampedX = Mathf.Clamp(playerObj.transform.position.x, screenMin.x + playerHalfWidth, screenMax.x - playerHalfWidth);
        float clampedY = Mathf.Clamp(playerObj.transform.position.y, screenMin.y + playerHalfHeight, screenMax.y - playerHalfHeight);
        playerObj.transform.position = new Vector3(clampedX, clampedY, 0);

        float alienClampedX = Mathf.Clamp(alienObj.transform.position.x, screenMin.x + alienHalfWidth, screenMax.x - alienHalfWidth);
        float alienClampedY = Mathf.Clamp(alienObj.transform.position.y, screenMin.y + alienHalfHeight, screenMax.y - alienHalfHeight);
        alienObj.transform.position = new Vector3(alienClampedX, alienClampedY, 0);
    }

    private void UpdateTimer()
    {
        if (allowInput)
        {
            timer -= Time.deltaTime;
        }

        timerTextUI.text = string.Format("{0:F2}", timer);
    }

    /// <summary>
    /// Set Game Win conditions
    /// </summary>
    private void GameWin()
    {
        isGameWon = true;
        inputManager.OnAHeld.RemoveAllListeners();
        inputManager.OnBHeld.RemoveAllListeners();
        inputManager.OnBothHeld.RemoveAllListeners();
        gameManagerScript.EndMicrogame(true);
        Debug.Log("You Win!");
    }

    /// <summary>
    /// Set Game Over conditions
    /// </summary>
    private void GameOver()
    {
        isGameOver = true;
        inputManager.OnAHeld.RemoveAllListeners();
        inputManager.OnBHeld.RemoveAllListeners();
        inputManager.OnBothHeld.RemoveAllListeners();
        gameManagerScript.EndMicrogame(false);
    }
}
