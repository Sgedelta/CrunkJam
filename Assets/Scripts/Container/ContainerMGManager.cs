using Unity.VisualScripting;
using UnityEngine;

public class ContainerMGManager : MicroGameManager
{
    //Needed Vars
    [SerializeField] private float moveSpeed = 7;
    public float difficultyCoeff = 1;
    private bool velocityPositive = true;

    [SerializeField] private GameObject bucket;
    [SerializeField] private GameObject leftAlien;
    [SerializeField] private GameObject rightAlien;

    private int collectedAliens = 0;
    public override void Initialize(InputManager im, int difficulty)
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
        bucket.transform.position = new Vector2(0, -3.25f);
    }

    public override void UnloadScene()
    {
        //this currently does not require any unloading, as it is for testing
    }

    protected override void BindInput()
    {
        inputManager.OnAPressed.AddListener(ReleaseLeftAlien);
        inputManager.OnBPressed.AddListener(ReleaseRightAlien);
    }

    private void Start()
    {
        bucket.GetComponent<Rigidbody2D>().linearVelocityX = moveSpeed * difficultyCoeff;
    }

    private void Update()
    {
        if (bucket.transform.position.x > 7 && velocityPositive)
        {
            bucket.transform.position = new Vector2(7f, bucket.transform.position.y);
            ReverseDirection();
        }
        else if (bucket.transform.position.x < -7 && !velocityPositive)
        {
            bucket.transform.position = new Vector2(-7f, bucket.transform.position.y);
            ReverseDirection();
        }

        if (leftAlien.transform.position.y < -5 || rightAlien.transform.position.y < -5)
        {
            Debug.Log("womp womp");
            GameManager.Instance.EndMicrogame(false);
        }
        if (collectedAliens == 2)
        {
            Debug.Log("yippee! you win!");
            GameManager.Instance.EndMicrogame(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If the alien collides with the bucket, increment your local "score" and then make it inactive
        if (collision.collider.tag == "Alien")
        {
            collectedAliens++;
            collision.gameObject.SetActive(false);
        }
    }
    public void ReverseDirection()
    {
        bucket.GetComponent<Rigidbody2D>().linearVelocityX *= -1;
        velocityPositive = !velocityPositive;

    }
    /// <summary>
    /// Turns gravity on for the left alien
    /// </summary>
    public void ReleaseLeftAlien()
    {
        leftAlien.GetComponent<Rigidbody2D>().gravityScale = 1;
    }
    /// <summary>
    /// Turns gravity on for the left alien
    /// </summary>
    public void ReleaseRightAlien()
    {
        rightAlien.GetComponent<Rigidbody2D>().gravityScale = 1;
    }



}
