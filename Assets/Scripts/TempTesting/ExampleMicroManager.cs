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

    public override void Initialize(InputManager im)
    {
        //these two steps should always be done
        inputManager = im;
        BindInput(); //this has to be made later
    
        //initialize any variables you would normally do in Start here


        //load the scene
        LoadScene();
    }

    public override void LoadScene()
    {
        //set up the scene to play
        player.transform.position = Vector3.zero;
    }

    public override void UnloadScene()
    {
        //this currently does not require any unloading, as it is for testing
    }

    protected override void BindInput()
    {
        inputManager.OnAPressed.AddListener(RotateCounterClockwise);
        inputManager.OnBPressed.AddListener(RotateClockwise);

        inputManager.OnAHeld.AddListener(MoveNeg);
        inputManager.OnBHeld.AddListener(MovePos);
    }

    public void Move(int moveDirection)
    {
        player.transform.position += (Vector3)dir * moveSpeed * moveDirection * Time.deltaTime;
    }

    public void MovePos()
    {
        Move(1);
    }

    public void MoveNeg()
    {
        Move(-1);
    }

    public void Rotate(float degrees)
    {
        //update angle
        dirAngle += degrees;

        //radians
        float dirAngleRad = Mathf.Deg2Rad * dirAngle;

        //update moveDirection with angle
        dir = new Vector2(Mathf.Cos(dirAngleRad), Mathf.Sin(dirAngleRad));
    }

    public void RotateClockwise()
    {
        Rotate(rotationAmnt);
    }

    public void RotateCounterClockwise()
    {
        Rotate(-rotationAmnt);
    }



}
