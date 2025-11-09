using UnityEngine;

public class CatchItem : MonoBehaviour
{
    public bool isGood;
    public float fallSpeed;
    public CatchMGManager manager;

    private void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // destroy if it falls below the screen
        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            manager.AddScore(isGood ? 1 : -1);
            Destroy(gameObject);
        }
    }
}

