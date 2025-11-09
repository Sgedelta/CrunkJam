using UnityEngine;
using UnityEngine.InputSystem;

public class StompManager : MicroGameManager
{

    [SerializeField] private StompHandling stomp;

    [SerializeField] private float endGameCountdown = 10;

    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float cooldown = 0;

    [SerializeField] private Spawner spawner;
    public override void Initialize(InputManager im)
    {
        //these two steps should always be done
        inputManager = im;
        BindInput(); //this has to be made later

    }

    private void Update()
    {
        endGameCountdown -= Time.deltaTime;
        if (endGameCountdown <= 0)
        {
            GameManager.Instance.EndMicrogame(true);
        }

        if (cooldown <= 0 && endGameCountdown > 5)
        {
            spawner.SpawnAlien();
            cooldown = spawnInterval;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            stomp.StompAlien();
        }

        cooldown -= Time.deltaTime;

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

            Debug.Log("A Pressed Listener");
        });
        inputManager.OnBPressed.AddListener(() => {

            Debug.Log("B Pressed Listener");
        });

        inputManager.OnAHeld.AddListener(() => {

            Debug.Log("A Held Listener");
        });
        inputManager.OnBHeld.AddListener(() => {

            Debug.Log("B Held Listener");
        });

        inputManager.OnAHoldReleased.AddListener(() => {

            Debug.Log("A Released Listener");
        });
        inputManager.OnBHoldReleased.AddListener(() => {

            Debug.Log("B Released Listener");
        });

        inputManager.OnBothPressed.AddListener(() =>
        {
            Debug.Log("Both Pressed Listener");
            Stomp();
        });

        inputManager.OnBothHeld.AddListener(() =>
        {
            Debug.Log("Both Held Listener");

        });

        inputManager.OnBothHoldReleased.AddListener(() =>
        {
            Debug.Log("Both Released Listener");
        });
    }

    public void Lose()
    {
        Debug.Log("You had one fucking job)");
        GameManager.Instance.EndMicrogame(false);
    }

    public void Stomp()
    {
        stomp.StompAlien();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Aliens>().squish)
        {
            GameManager.Instance.EndMicrogame(false);
        }
    }


}
