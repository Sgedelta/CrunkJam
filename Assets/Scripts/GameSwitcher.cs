using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameSwitcher : MonoBehaviour
{
    [SerializeField] float initializationDelay = 3;

    public void LoadMicrogame(string sceneName)
    {
        Debug.Log("Loading the microgame : " + sceneName);

        //subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;

        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene " + scene.name + " loaded successfully!");
        //initialization would go here (including the code i need)
        MicroGameManager microGameManager = FindFirstObjectByType<MicroGameManager>();
        if (microGameManager != null)
        {
            Debug.Log("Found a microgameManager!!! " + microGameManager.gameObject.name);
            StartCoroutine(WaitToInit(microGameManager));
        }
        else
        {
            Debug.LogError("MicroGameManager not found.");
        }

        SceneManager.sceneLoaded -= OnSceneLoaded; //unsubscribe so it only runs once and doesnt call again.
    }

    private IEnumerator WaitToInit(MicroGameManager mm)
    {
        yield return new WaitForSeconds(initializationDelay);

        GameManager.Instance.IntializeManager(mm);
    }
}
