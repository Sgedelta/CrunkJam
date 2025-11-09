using UnityEngine;
using UnityEngine.UI;

public class FeedGameManager : MicroGameManager
{
    private bool feedingLeft = false;
    private bool feedingRight = false;

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

    [Header("GameManager")]
    [SerializeField] private GameObject gameManagerObj;

    public override void Initialize(InputManager im)
    {
        inputManager = im;
        BindInput();

        // load the scene
        LoadScene();
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
        inputManager.OnAPressed.AddListener(FeedLeft);
        inputManager.OnBPressed.AddListener(FeedRight);

        inputManager.OnAHoldReleased.AddListener(FeedLeftReset);
        inputManager.OnBHoldReleased.AddListener(FeedRightReset);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
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

    public void UpdateTimer()
    {
        if (!isGameWon)
        {
            timer -= Time.deltaTime;
        }


    }
}
