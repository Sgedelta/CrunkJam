using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6;

    private bool movingRight = false;
    private bool movingUp = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveRight()
    {
        this.transform.position += (Vector3)Vector2.right * moveSpeed * Time.deltaTime;
    }

    public void MoveUp()
    {
        this.transform.position += (Vector3)Vector2.up * moveSpeed * Time.deltaTime;
    }
}
