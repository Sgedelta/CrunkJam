using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlassManager : MicroGameManager
{
    AudioSource aSource;    
    int numberCleans = 5;
    int numberTaps = 5;
    int startNum;
    [SerializeField]Text timerText;
    GameObject alien;

    SpriteRenderer dirt;
    float timer;
    float seconds;
    float milliseconds;

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            seconds = (int)timer;
            milliseconds = (int)((timer - (int)timer) * 100);
            timerText.text = seconds + ":" + milliseconds;
        }
        else if (timer != 0)
        {
            timer = 0;
            timerText.text = "FAIL";
            GameManager.Instance.EndMicrogame(false);
            Debug.Log("GAME FAILED");
        }
    }

    public override void Initialize(InputManager im)
    {

        //these two steps should always be done
        inputManager = im;
        BindInput(); //this has to be made later

        //initialize any variables you would normally do in Start here

        LoadScene();
    }

    public override void LoadScene()
    {
        alien = GameObject.Find("Alien");
        alien.SetActive(false);
        dirt = GameObject.Find("dirt").GetComponent<SpriteRenderer>();
        aSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();

        timer = 6 - GameManager.Instance.Difficulty / 2f;
        numberCleans += GameManager.Instance.Difficulty;
        numberTaps += GameManager.Instance.Difficulty;
        startNum = numberTaps;
    }

    public override void UnloadScene()
    {

    }

    protected override void BindInput()
    {
        inputManager.OnAPressed.AddListener(CleanWindow);
        inputManager.OnBPressed.AddListener(TapWindow); ;
    }

    public void CleanWindow()
    {
        numberCleans--;
        Debug.Log(numberCleans);
        // Decrease opacity of dirt
        float opacityNum = dirt.color.a - (1f/startNum);
        Debug.Log(opacityNum);
        dirt.color = new Color(1, 1, 1, opacityNum);

        // Check completion
        if (numberTaps <= 0 && numberCleans <= 0) { AlienApproach(); numberCleans = 0; }
    }

    public void TapWindow()
    {
        numberTaps--;

        Debug.Log(numberTaps);
        // Play Tap Sound
        aSource.Play();

        // Check completion
        if (numberTaps <= 0 && numberCleans <= 0) { AlienApproach(); }
    }

    public void AlienApproach()
    {
        if (!alien.activeSelf)
            alien.SetActive(true);
        timer = 0;
        timerText.text = "";
    }

    public void AnimEnd()
    {
        GameManager.Instance.EndMicrogame(true);
        Debug.Log("GAME END");
    }
}
