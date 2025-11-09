using UnityEngine;

public class Crane : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6;

    private bool movingRight = false;
    private bool movingLeft = false;

    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite anim1;
    [SerializeField] private Sprite anim2;
    private bool spriteSwap;
    private float animCooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (movingLeft)
        {
            this.transform.position -= (Vector3)Vector2.right * moveSpeed * Time.deltaTime;
        }
        else if (movingRight)
        {
            this.transform.position += (Vector3)Vector2.right * moveSpeed * Time.deltaTime;
        }

        if (animCooldown <= 0)
        {
            if (spriteSwap)
                sr.sprite = anim1;
            else sr.sprite = anim2;

            spriteSwap = !spriteSwap;

            animCooldown++;
        }

        animCooldown -= Time.deltaTime;
    }

    public void MoveRight()
    {
        movingRight = true;
    }

    public void MoveLeft()
    {
        movingLeft = true;
    }

    public void MoveRightReset()
    {
        movingRight = false;
    }

    public void MoveLeftReset()
    {
        movingLeft = false;
    }
}
