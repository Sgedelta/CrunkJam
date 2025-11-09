using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DefenseMGMangager : MicroGameManager
{
    [SerializeField] public GameObject defender;
    [SerializeField] public Vector2 defaultPos = new Vector2(2.5f, 0);

    [SerializeField] public float verticalChange = 3;
    //positions are 0, 1, 2. 0 Is the highest point, 2 the lowest
    private int currentPosInt = 1;

    [SerializeField] GameObject evilGuy;
    private Vector2 evilDefaultPos = new Vector2(-6f, 0);
    float timer = 0;
    int shootInterval = 1;

    [SerializeField] private float laserVelocity;
    [SerializeField] private GameObject laser;
    private List<GameObject> lasersList = new List<GameObject>();

    float winTime = 5;
    float totalTime = 0;

    public override void Initialize(InputManager im, int difficulty)
    {

        //these two steps should always be done
        inputManager = im;
        BindInput(); //this has to be made later

        //initialize any variables you would normally do in Start here
        shootInterval = (shootInterval + difficulty) / Mathf.Max(difficulty, 1);

        //load the scene
        LoadScene();
    }

    public override void LoadScene()
    {

    }

    public override void UnloadScene()
    {

    }

    protected override void BindInput()
    {
        inputManager.OnAPressed.AddListener(MoveUp);
        inputManager.OnBPressed.AddListener(MoveDown);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        totalTime += Time.deltaTime;
        if (timer > shootInterval)
        {
            MoveEvilGuy();
            timer = 0;
        }
        if (lasersList != null)
        {
            GameObject[] lasers = lasersList.ToArray();

            foreach (GameObject a in lasers)
            {
                if (a.transform.position.x > 8)
                {
                    lasersList.Remove(a);
                    GameObject.Destroy(a);
                    //If the player fails to protect a laser from reaching the screen)
                    GameManager.Instance.EndMicrogame(false);
                    Debug.Log("Lmao bad");
                }
            }
        }

        if (totalTime > winTime)
        {
            GameManager.Instance.EndMicrogame(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("blocked");
        if (collision.collider.tag == "Laser")
        {
            lasersList.Remove(collision.collider.gameObject);
            GameObject.Destroy(collision.collider.gameObject);
        }
    }

    /// <summary>
    /// Moves the defender to the upper of the three positions: 0
    /// </summary>
    public void MoveUp()
    {
        switch (currentPosInt)
        {
            case 0:
                break;
            case 1:
                defender.transform.position = new Vector2(defaultPos.x, defaultPos.y + verticalChange);
                currentPosInt--;
                break;
            case 2:
                defender.transform.position = new Vector2(defaultPos.x, defaultPos.y);
                currentPosInt--;
                break;
            default:
                break;
        }
        
    }

    /// <summary>
    /// Moves the defender to the lower of the three positions : 2
    /// </summary>
    public void MoveDown()
    {
        switch (currentPosInt)
        {
            case 0:
                defender.transform.position = new Vector2(defaultPos.x, defaultPos.y);
                currentPosInt++;
                break;
            case 1:
                defender.transform.position = new Vector2(defaultPos.x, defaultPos.y - verticalChange);
                currentPosInt++;
                break;
            case 2:                
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Resets the position of the defender to its default state in the middle
    /// </summary>
    public void ResetPosition()
    {
        defender.transform.position = defaultPos;
    }

    public void MoveEvilGuy()
    {
        int toMove = Mathf.FloorToInt(Random.Range(0, 3));

        switch (toMove)
        {
            case 0:
                evilGuy.transform.position = new Vector2(evilDefaultPos.x, evilDefaultPos.y - verticalChange);
                EvilShoot(evilGuy.transform.position);
                break;
            case 1:
                evilGuy.transform.position = evilDefaultPos;
                EvilShoot(evilGuy.transform.position);
                break;
            case 2:
                evilGuy.transform.position = new Vector2(evilDefaultPos.x, evilDefaultPos.y + verticalChange);
                EvilShoot(evilGuy.transform.position);
                break;
            default:
                break;
        }
    }
    public void EvilShoot(Vector2 startPos)
    {
        GameObject newLaser = GameObject.Instantiate(laser, startPos, Quaternion.identity);
        newLaser.GetComponent<Rigidbody2D>().linearVelocityX = laserVelocity;
        lasersList.Add(newLaser);
    }
}
