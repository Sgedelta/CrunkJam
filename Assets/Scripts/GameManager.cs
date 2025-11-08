using NUnit.Framework;
using UnityEditor.SearchService;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public KeyCode inputA;
    public KeyCode inputB;

    int minigamesCompleted;  // functions as the score currently (can make a more true score later that is updated based on difficulty and speed)
    public int difficulty;   // difficulty that player is at, dependent on the minigames completed. This can change the games' obstacles, timer, etc.
    public int health;              // each game failed removes 1 from health.

    List<string> minigames;  // List of every potential minigame, the saved strings are the names of their scenes to be loaded
    List<string> grabBag;    // The list of minigames, sorted at random per game, to be played in that order so that the player sees all minigames before true random

    private void Awake()
    {
        //Check if there is no instance of this GameManager that exists.
        if (Instance == null)
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
        // input keys are 1 and 5 respectively
        inputA = KeyCode.Alpha1;
        inputB = KeyCode.Alpha5;

        Restart(); // Set all values to the default and reset grabBag
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Should be called whenever one Microgame Ends, from its respective MicrogameManager. The bool is whether or not the player successfully completed said microgame
    public void LoadNewMicrogame(bool successful) 
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
                return;
            }
        }
        else
        {
            minigamesCompleted++;
        }

        // Regardless of if the last game was won or lost, so long as the player has health left (if they didn't this wouldn't run),
        // start the next minigame taking into account potential grabBag if the player hasn't seen all minigames yet
        if (grabBag.Count > 0)
        {
            SceneManager.LoadScene(grabBag[0]);
            grabBag.RemoveAt(0);
        }
        else
        {
            SceneManager.LoadScene(minigames[Random.Range(0, minigames.Count)]);
        }
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
            grabBag[i] = name;
            minigamesCopy.Remove(name);
        }
    }
}