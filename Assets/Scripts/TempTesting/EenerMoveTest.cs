using UnityEngine;

public class EenerMoveTest : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    private float time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time%10 < 5)
        {
            rb.linearVelocity = new Vector2(1,0);
        }
        else
            rb.linearVelocity = new Vector2(0, 0);

    }
}
