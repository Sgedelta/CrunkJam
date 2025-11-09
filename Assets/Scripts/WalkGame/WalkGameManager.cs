using UnityEngine;
using UnityEngine.UI;

public class WalkGameManager : MicroGameManager
{
    private bool isGameOver = false;
    private bool isGameWon = false;
    [Header("Initial Timer Value")]
    [SerializeField ] private float timer;

    [Header("Player")]
    [SerializeField] private GameObject playerObj;
    private Player playerScript;
    private SpriteRenderer spriteRenderer;
    private float playerHalfHeight;
    private float playerHalfWidth;

    [Header("Cage")]
    [SerializeField] private GameObject cageObj;

    [Header("TimerUI")]
    [SerializeField] private Text timerTextUI;

    private GameManager gameManagerScript;

    Vector2 screenMin;
    Vector2 screenMax;

    public override void Initialize(InputManager im)
    {
        inputManager = im;
        BindInput();

        // load the scene
        LoadScene();

        // grab a reference to the Player script attached and set playerHalfHeight and playerHalfSize
        if (playerObj)
        {
            playerScript = playerObj.GetComponent<Player>();
            spriteRenderer = playerObj.GetComponent<SpriteRenderer>();
            playerHalfHeight = spriteRenderer.bounds.extents.y;
            playerHalfWidth = spriteRenderer.bounds.extents.x;
        }

        // grab reference to Game Manager script

        gameManagerScript = GameManager.Instance;

        // get min and max of the screen from camera viewport
        screenMin = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        screenMax = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // initialize timer text ui
        timerTextUI.text = string.Format("{0:F2}", timer);
    }

    private void Update()
    {
        if (isGameOver) return;

        UpdateTimer();
        CheckBounds();
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
        inputManager.OnBHeld.AddListener(MoveRight);
        inputManager.OnAHeld.AddListener(MoveUp);

        inputManager.OnAHoldReleased.AddListener(MoveRightReset);
        inputManager.OnBHoldReleased.AddListener(MoveUpReset);

        inputManager.OnBothHeld.AddListener(MoveBothDirections);
        inputManager.OnBothHoldReleased.AddListener(MoveBothDirectionsReset);
    }
    
    public void MoveRight()
    {
        playerScript.MoveRight();
    }

    public void MoveUp()
    {
        playerScript.MoveUp();
    }

    public void MoveBothDirections()
    {
        playerScript.MoveRight();
        playerScript.MoveUp();
    }

    public void MoveRightReset()
    {
        playerScript.MoveRightReset();
    }

    public void MoveUpReset()
    {
        playerScript.MoveUpReset();
    }

    public void MoveBothDirectionsReset()
    {
        playerScript.MoveRightReset();
        playerScript.MoveUpReset();
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
    }

    /// <summary>
    /// Update Timer and format display properly
    /// </summary>
    private void UpdateTimer()
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

    /// <summary>
    /// When player enters collision with cage, player wins
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if(collision.gameObject.tag == "player")
        {
            // You Win Logic!
            GameWin();
        }
    }
}
