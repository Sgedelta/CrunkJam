using UnityEngine;

public class RedGuy : MonoBehaviour
{
    [Header("Feed Game Manager")]
    [SerializeField] private GameObject gameManager;
    private FeedGameManager feedGameManagerScript;

    [Header("Color of Alien")]
    [SerializeField] private int color;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameManager)
        {
            feedGameManagerScript = gameManager.GetComponent<FeedGameManager>();
        }
    }

    /// <summary>
    /// When Food item collides with this Alien
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.tag == "food")
        {
            FoodItem foodItem = collision.gameObject.GetComponent<FoodItem>();
         
            // Food is the same color as Alien, they like it!
            if (color == foodItem.color)
            {
                Debug.Log("Alien Likes the Food");
                feedGameManagerScript.count++;

            }
            // Food is not the same color as Alien, they do not like it!
            else if(color != foodItem.color) 
            {
                Debug.Log("Alien Does Not Like the Food");
                // If player feeds wrong food to alien, game over!
                //feedGameManagerScript.GameOver();
            }

            // Hook up Win State here!
            if (feedGameManagerScript.count >= 5)
            {
                Debug.Log("go next level");
                feedGameManagerScript.GameWon();
            }

            // Sets necessary variables for next food item
            collision.gameObject.GetComponent<Renderer>().enabled = false;
            feedGameManagerScript.ReadyNextFood();
        }
    }
}
