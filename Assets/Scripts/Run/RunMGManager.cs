using System.Net.Sockets;
using TreeEditor;
using UnityEngine;

public class RunMGManager : MicroGameManager
{
    //Needed Vars
    [SerializeField] private float moveSpeed = 3;

    [SerializeField] private float rotationAmnt = 30;
    [SerializeField] private GameObject runner;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private Vector2 standHeight = new Vector2(1, 1.9f);
    [SerializeField] private Vector2 crouchHeight = new Vector2(1, 0.8f);

    [SerializeField] public Vector2 crouchSpawnPoint = new Vector2(15f, .85f);
    [SerializeField] public Vector2 jumpSpawnPoint = new Vector2(15f, -2.2f);
    [SerializeField] private GameObject crouchObstacle;
    [SerializeField] private GameObject jumpObstacle;
    [SerializeField] private float obstacleInterval = 1;

    private float timer = 0;
    public float difficultyCoeff = 1f;

    public override void Initialize(InputManager im, int difficulty)
    {
        //these two steps should always be done
        inputManager = im;
        BindInput(); //this has to be made later
    
        //initialize any variables you would normally do in Start here
        SpawnObstacle();

        //load the scene
        LoadScene();
    }
    //TODO
    public override void LoadScene()
    {
        //set up the scene to play
        //player.transform.position = Vector3.zero;
    }

    public override void UnloadScene()
    {
        //this currently does not require any unloading, as it is for testing
    }

    protected override void BindInput()
    {
        inputManager.OnAPressed.AddListener(Jump);
        inputManager.OnBPressed.AddListener(Duck);
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > obstacleInterval)
        {
            SpawnObstacle();
            timer = 0;
        }
    }
    public void Jump()
    {
        runner.GetComponent<Rigidbody2D>().AddForceY(jumpForce, ForceMode2D.Impulse);
    }

    public void Duck()
    {
        runner.transform.localScale = crouchHeight;
        runner.GetComponent<Rigidbody2D>().AddForceY(-4f, ForceMode2D.Impulse);
    }

    private void SpawnObstacle()
    {
        if(Random.Range(0f,1f) > 0.5f)
        {
            //spawn crouch
            GameObject newCO = GameObject.Instantiate(crouchObstacle, crouchSpawnPoint, Quaternion.identity);
            newCO.GetComponent<Rigidbody2D>().linearVelocityX = -moveSpeed * difficultyCoeff;
        }
        else
        {
            //spawn jump
            GameObject newJO = GameObject.Instantiate(jumpObstacle, jumpSpawnPoint, Quaternion.identity);
            newJO.GetComponent<Rigidbody2D>().linearVelocityX = -moveSpeed * difficultyCoeff;
        }
    }



}
