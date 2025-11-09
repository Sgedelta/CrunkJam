using UnityEngine;

public class Dont : MicroGameManager
{
    [SerializeField] private float countdown = 10;
    public override void Initialize(InputManager im, int difficulty)
    {
        //these two steps should always be done
        inputManager = im;
        BindInput(); //this has to be made later

    }

    private void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
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
