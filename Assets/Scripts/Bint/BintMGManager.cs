using UnityEngine;

public class BintMGManager : MicroGameManager
{
    // Author: Sam Easton
    // an example micro game manager for a simple microgame that moves something left on A held and right on B held.
    // it also roates clockwise on A and counterclockwise on B
    // primarily made to provide an example and test input methods
    // you would put the rules for the game here, easily readable by everyone!

    //Needed Vars

    [SerializeField] private GameObject bogo;

    [SerializeField] private Vector2 bogoLeftPos;
    [SerializeField] private Vector2 bogoRightPos;
    [SerializeField] private Vector3 bogoRotation;

    [SerializeField] private Sprite binted;

    private bool isLeft = false;
    private bool isRight = false;

    private int shakes = 0;
    [SerializeField] private int shakesGoal;

    private float timer = 0;
    [SerializeField] private float gameOverTimer = 20;

    [SerializeField] private float difficultyCoeff;
    public override void Initialize(InputManager im)
    {
        //these two steps should always be done
        inputManager = im;
        BindInput(); //this has to be made later

        //initialize any variables you would normally do in Start here
        shakesGoal = (int)(25 * (0.75 * GameManager.Instance.Difficulty));

        //load the scene
        LoadScene();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > gameOverTimer)
        {
            GameManager.Instance.EndMicrogame(false);
        }

        if(shakes > shakesGoal)
        {
            BintingSuccessful();
            //GameManager.Instance.EndMicrogame(true);
        }
        Debug.Log(shakes);

    }

    public override void LoadScene()
    {
        //set up the scene to play
        bogo.transform.position = Vector3.zero;
        bogo.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public override void UnloadScene()
    {
        //this currently does not require any unloading, as it is for testing
    }

    protected override void BindInput()
    {
        inputManager.OnAPressed.AddListener(ShakeLeft);
        inputManager.OnBPressed.AddListener(ShakeRight);
        inputManager.OnBothPressed.AddListener(ShakeBoth);
    }
    public void ShakeLeft()
    {
        bogo.transform.position = bogoLeftPos;
        bogo.transform.rotation = Quaternion.Euler(bogoRotation);
        if (!isLeft)
        {
            isRight = false;
            isLeft = true;
            shakes++;
        }
    }
    public void ShakeRight()
    {
        bogo.transform.position = bogoRightPos;
        bogo.transform.rotation = Quaternion.Euler(-bogoRotation);
        if (!isRight)
        {
            isLeft = false;
            isRight = true;
            shakes++;
        }
    }
    public void ShakeBoth()
    {
        if (isLeft) ShakeLeft();
        else if (isRight) ShakeRight();
    }
    public void BintingSuccessful()
    {
        bogo.GetComponent<SpriteRenderer>().sprite = binted;
    }
}
