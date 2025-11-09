using UnityEditor.SearchService;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEditor;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private bool debug = false; //a general debug state for the project, set in editor
    public bool DEBUG { get { return debug; } }


    [SerializeField] private MicroGameManager DEBUGForceMicroManager; //if filled && DEBUG, will start the micro game manager on start, for testing
    [SerializeField] private string DEBUGForceGameLoad; //if filled && DEBUG, will only load the given microgame, for testing

    private InputManager inputManager;
    private GameSwitcher gameSwitcher;

    [SerializeField] public InputActionReference inputA;
    [SerializeField] public InputActionReference inputB;

    [SerializeField] private GameObject DoorAnimationPrefab;
    private GameObject DoorAnimationParent;

    private int minigamesCompleted = 0;  // functions as the score currently (can make a more true score later that is updated based on difficulty and speed)

    public int MinigamesCompleted { get { return minigamesCompleted; } }
    private int difficulty;   // difficulty that player is at, dependent on the minigames completed. This can change the games' obstacles, timer, etc.
    public int Difficulty // properties primarily so that we can call proper animation/display methods when we change these things later - Sam
    {
        get { return difficulty; }
        set { difficulty = value; }
    }

    private int health = 3; // each game failed removes 1 from health. //default 3 for testing
    public int Health // properties primarily so that we can call proper animation/display methods when we change these things later - Sam
    {
        get { return health; }
        set { health = value; }
    }

    [SerializeField] List<string> minigames;  // List of every potential minigame, the saved strings are the names of their scenes to be loaded
    [SerializeField] List<Sprite> gameLInstructions;
    [SerializeField] List<Sprite> gameRInstructions;
    [SerializeField] List<Sprite> numberSprites;

    Dictionary<string, Tuple<Sprite, Sprite>> minigameInstructionDict = new Dictionary<string, Tuple<Sprite, Sprite>>();
    
    List<string> grabBag;    // The list of minigames, sorted at random per game, to be played in that order so that the player sees all minigames before true random

    private void Awake()
    {
        //Check if there is no instance of this GameManager that exists.
        if (Instance == null && Instance != this)
        {
            //Set the gameObject this script is attached to as the single instance of the GameManager.
            Instance = this;

            //This method informs Unity to retain the object this script is attached to when changing scenes.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //If there is already an instance of the GameManager, then delete the object this is attached to.
            //This ensures that only one instance of the GameManager exists across all scenes.
            Destroy(this.gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        grabBag = new List<string>();

        inputManager = gameObject.GetComponent<InputManager>();
        gameSwitcher = gameObject.GetComponent<GameSwitcher>();

        Restart(); // Set all values to the default and reset grabBag

        if (DEBUG && DEBUGForceMicroManager != null)
        {
            DEBUGForceMicroManager.Initialize(inputManager);
        }

        //build dictionary so we can display instructions properly
        for (int i = 0; i < minigames.Count; i++)
        {
            Tuple<Sprite, Sprite> spriteTuple;
            if(i > gameLInstructions.Count || i > gameRInstructions.Count)
            {
                Debug.LogError("more minigames than instructions!! Something's wrong!!!!");
                break;
            }

            spriteTuple = new Tuple<Sprite, Sprite>(gameLInstructions[i], gameRInstructions[i]);

            minigameInstructionDict.Add(minigames[i], spriteTuple);
        }
    }

    /// <summary>
    /// Should be called whenever one Microgame Ends, from its respective MicrogameManager.
    /// The bool is whether or not the player successfully completed said microgame.
    /// </summary>
    /// <param name="successful"></param>
    public void EndMicrogame(bool successful)
    {

        // if the microgame was lost, decrease the health
        if (!successful)
        {
            health--;

            // if all health is gone, manage loss data and then skip the player straight to the End Scene
            if (health <= 0)
            {
                // Potential TODO: if score is greater than the lowest of the 3 recorded high scores, overwrite it to the proper place
                // SceneManager.LoadScene("EndScene");
                Debug.Log("Game Run End\nScore: " + minigamesCompleted);

#if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
#endif

                return;
            }
        }
        else
        {
            minigamesCompleted++;
        }

        StartCoroutine(RunDoorAnimationAndNextGameCoroutine(health <= 0));
    }

    private IEnumerator RunDoorAnimationAndNextGameCoroutine(bool alive)
    {
        //create door animator and parent to GameManager (so it doesn't get destroyed as the game goes on)
        DoorAnimationParent = Instantiate(DoorAnimationPrefab, transform);

        //wait for door animation to go through....
        yield return new WaitForSeconds(2f);

        if(alive)
        {
            //load a game
            string loadedGame = LoadNewMicrogame();

            //set the door sprites
            DoorAnimationParent.GetComponent<DoorDisplayControl>()
                .SetSprites(numberSprites[minigamesCompleted], minigameInstructionDict[loadedGame]);

        } 
        else
        {

        }

        //wait for door animation to complete
        yield return new WaitForSeconds(5f); //bit of buffer time here.


        //destroy door animator after it's done animating (cleanup)
        Destroy(DoorAnimationParent);

        yield return null;


    }


    /// <summary>
    /// Loads a random microgame from the bag if there are any in there, or a random one otherwise
    /// </summary>
    public string LoadNewMicrogame() 
    {
        string chosenGame = "";
        // Regardless of if the last game was won or lost, so long as the player has health left (if they didn't this wouldn't run),
        // start the next minigame taking into account potential grabBag if the player hasn't seen all minigames yet
        if (grabBag.Count > 0)
        {
            chosenGame = grabBag[0];
            grabBag.RemoveAt(0);
        }
        else
        {
            SceneManager.LoadScene(minigames[Random.Range(0, minigames.Count)]);
        }

        if (DEBUG && DEBUGForceGameLoad != null && DEBUGForceGameLoad != "")
        {
            chosenGame = DEBUGForceGameLoad;
        }

        //TODO: implement transition scene here (or in GameSwitcher)
        gameSwitcher.LoadMicrogame(chosenGame);
        return chosenGame;
    }

    /// <summary>
    /// initializes the given MicroGameManager with the correct inputManager
    /// </summary>
    /// <param name="mm"></param>
    public void IntializeManager(MicroGameManager mm)
    {
        mm.Initialize(inputManager);
    }

    // To call before any new run starts
    public void Restart()
    {
        // Reset values
        minigamesCompleted = 0;
        health = 3;
        difficulty = 0;

        // Create a grabBag so that minigames will not repeat until the player sees all of them
        List<string> minigamesCopy = minigames;
        string name = "";
        for (int i = 0; i < minigames.Count; i++)
        {
            // Take a random name from minigames that has not yet been selected,
            // assign it to the current grabBag position, then remove it from the pool
            name = minigamesCopy[Random.Range(0, minigames.Count)];
            grabBag.Add(name);
            minigamesCopy.Remove(name);
        }
    }
}