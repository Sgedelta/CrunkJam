using UnityEngine;

public class ExampleMicroManager : MicroGameManager
{
    // Author: Sam Easton
    // an example micro game manager for a simple microgame that moves something left on A held and right on B held.
    // it also roates clockwise on A and counterclockwise on B
    // primarily made to provide an example and test input methods
    // you would put the rules for the game here, easily readable by everyone!

    //Needed Vars
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float rotationAmnt = 30;
    [SerializeField] private GameObject player;

    private Vector2 dir = Vector2.right;
    private float dirAngle = 0;

    private int moveDir = 0;

    public override void Initialize(InputManager im)
    {
        //these two steps should always be done
        inputManager = im;
        BindInput(); //this has to be made later

        //initialize any variables you would normally do in Start here
        //such as - difficulty!
        moveSpeed *= Mathf.Max(GameManager.Instance.Difficulty, 1);

        //load the scene
        LoadScene();
    }

    private void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0)
        {
            GameManager.Instance.EndMicrogame(true);
        }
    }
    public override void LoadScene()
    {
        //Don't do nutin
    }

    public override void UnloadScene()
    {
        //Don't do nutin
    }


    protected override void BindInput()
    {
        inputManager.OnAPressed.AddListener(() => {
            Lose();
            Debug.Log("A Pressed Listener");
        });
        inputManager.OnBPressed.AddListener(() => {
            Lose();
            Debug.Log("B Pressed Listener");
        });

        inputManager.OnAHeld.AddListener(() => {
            Lose();
            Debug.Log("A Held Listener");
        });
        inputManager.OnBHeld.AddListener(() => {
            Lose();
            Debug.Log("B Held Listener");
        });

        inputManager.OnAHoldReleased.AddListener(() => {
            Lose();
            Debug.Log("A Released Listener");
        });
        inputManager.OnBHoldReleased.AddListener(() => {
            Lose();
            Debug.Log("B Released Listener");
        });

        inputManager.OnBothPressed.AddListener(() =>
        {
            Debug.Log("Both Pressed Listener");
            Lose();
        });

        inputManager.OnBothHeld.AddListener(() =>
        {
            Debug.Log("Both Held Listener");
            Lose();
        });

        inputManager.OnBothHoldReleased.AddListener(() =>
        {
            Debug.Log("Both Released Listener");
            Lose();
        });
    }

    public void Lose()
    {
        Debug.Log("You had one fucking job)");
        GameManager.Instance.EndMicrogame(false);
    }
}
