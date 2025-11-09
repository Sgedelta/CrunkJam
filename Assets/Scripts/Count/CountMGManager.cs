using System;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class CountMGManager: MicroGameManager
{
    //Needed Vars
    float timer = 0;
    int currentCount = 0;
    [SerializeField] Text countText = null;
    [SerializeField] float difficultyCoeff = 1;
    [SerializeField] private int gameOverTime;

    int guyNumToSpawn;
    GameObject[] lilGuysArray = null;
    [SerializeField]GameObject lilGuyPrefab = null;
    [SerializeField] Vector2 maxBounds;
    [SerializeField] Vector2 minBounds;
    [SerializeField] float lilGuyMoveSpeed = 1f;
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
    }

    public override void UnloadScene()
    {
        //this currently does not require any unloading, as it is for testing
    }

    protected override void BindInput()
    {
        inputManager.OnAPressed.AddListener(DecrementCount);
        inputManager.OnBPressed.AddListener(IncrementCount);
    }

    private void Start()
    {
        SpawnLilGuys();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        //foreach (GameObject go in lilGuysArray)
        //{
        //    if (go.GetComponent<Rigidbody2D>().linearVelocityX > 0)go.transform.Rotate(new Vector3(0, 0, 1));
            
        //    if (go.transform.position.x > 7 && go.GetComponent<Rigidbody2D>().linearVelocityX > 0)
        //    {
        //        go.transform.position = new Vector3(go.transform.position.x - .1f, go.transform.position.y, go.transform.position.z);
        //        ReverseDirection(go);
        //    }
           
        //    if (go.GetComponent<Rigidbody2D>().linearVelocityX < 0) go.transform.Rotate(new Vector3(0, 0, -1));
            
        //    if (go.transform.position.x < 7 && go.GetComponent<Rigidbody2D>().linearVelocityX < 0)
        //    {
        //        go.transform.position = new Vector3(go.transform.position.x + .1f, go.transform.position.y, go.transform.position.z);
        //        ReverseDirection(go);
        //    }
        //}
        for (int i = 0; i < lilGuysArray.Length; i++)
        {
            if (lilGuysArray[i].GetComponent<Rigidbody2D>().linearVelocityX > 0) lilGuysArray[i].transform.Rotate(new Vector3(0, 0, -.5f));

            if (lilGuysArray[i].transform.position.x > 7 && lilGuysArray[i].GetComponent<Rigidbody2D>().linearVelocityX > 0)
            {
                lilGuysArray[i].transform.position = new Vector3(lilGuysArray[i].transform.position.x - .1f, lilGuysArray[i].transform.position.y, lilGuysArray[i].transform.position.z);
                ReverseDirection(lilGuysArray[i]);
            }
            else if (lilGuysArray[i].transform.position.x < -7 && lilGuysArray[i].GetComponent<Rigidbody2D>().linearVelocityX < 0)
            {
                lilGuysArray[i].transform.position = new Vector3(lilGuysArray[i].transform.position.x + .1f, lilGuysArray[i].transform.position.y, lilGuysArray[i].transform.position.z);
                ReverseDirection(lilGuysArray[i]);
            }
            if (lilGuysArray[i].GetComponent<Rigidbody2D>().linearVelocityX < 0) lilGuysArray[i].transform.Rotate(new Vector3(0, 0, .5f));

        }
        if(timer >= gameOverTime)
        {
            if (currentCount == lilGuysArray.Length)
            {
                //YOU WIN :D
                Debug.Log("wahoo.gif");
                GameManager.Instance.EndMicrogame(true);
            }
            else
            {
                //YOU LOSE >:(
                Debug.Log("dumb ahh");
                GameManager.Instance.EndMicrogame(false);
            }
        }
        countText.text = currentCount.ToString();

    }

    public void IncrementCount()
    {
        currentCount++;
    }
    public void DecrementCount()
    {
        currentCount--;
    }
    public void ReverseDirection(GameObject go)
    {
        go.GetComponent<Rigidbody2D>().linearVelocityX *= -1;
    }
    private void SpawnLilGuys()
    {
        guyNumToSpawn = (int)UnityEngine.Random.Range(0, (15 * GameManager.Instance.Difficulty));
        lilGuysArray = new GameObject[guyNumToSpawn];

        for (int i = 0; i < guyNumToSpawn; i++)
        {
            Vector2 spawnPoint = new Vector2(UnityEngine.Random.Range(minBounds.x, maxBounds.x), UnityEngine.Random.Range(minBounds.y, maxBounds.y));
            lilGuysArray[i] = GameObject.Instantiate(lilGuyPrefab, spawnPoint, Quaternion.identity);
            lilGuysArray[i].GetComponent<Rigidbody2D>().linearVelocityX = UnityEngine.Random.Range(0f, 1f) > .5f ? lilGuyMoveSpeed : -lilGuyMoveSpeed;
        }

    }
}
