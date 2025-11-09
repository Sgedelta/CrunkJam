using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class Alien : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3;

    private float timer;
    [Header("Timer Threshold")]
    [SerializeField] private float randomThreshold = 2;
    [SerializeField] private float resetThreshold = 3;

    [Header("Crane Game Manager")]
    [SerializeField] private CraneGameManager craneGameManager;

    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite anim1;
    [SerializeField] private Sprite anim2;
    private bool spriteSwap;
    private float animCooldown;

    private int dir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (craneGameManager.allowInput)
        {
            timer += Time.deltaTime;

            if (dir == 0 && timer >= randomThreshold)
            {
                dir = GetRandomAOrB(-1, 1);
            }

            if (timer < resetThreshold)
            {
                Move();
            }
            else
            {
                timer = 0;
                dir = 0;
            }
        }

        if(animCooldown <= 0)
        {
            if (spriteSwap)
                sr.sprite = anim1;
            else sr.sprite = anim2;

            spriteSwap = !spriteSwap;

            animCooldown += 2;
        }

        animCooldown -= Time.deltaTime;
    }

    public void Move()
    {
        Vector2 moveDir = Vector2.zero;

        if (dir == -1)
        {
            // go left
            moveDir.x -= moveSpeed * Time.deltaTime;
        }
        else if (dir == 1)
        {
            // go right
            moveDir.x += moveSpeed * Time.deltaTime;
        }

        this.transform.position = new Vector3(this.transform.position.x + moveDir.x, transform.position.y, 0);
        
    }

    /// <summary>
    /// Helper method to get a random number, either A or B
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public int GetRandomAOrB(int a, int b)
    {
        int randNum = Random.Range(0, 2);

        if (randNum == 0)
        {
            return a;
        }
        else
        {
            return b;
        }
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if(collision.gameObject.tag == "player")
        {
            Debug.Log("You win!");
            craneGameManager.isGameWon = true;
            GameManager.Instance.EndMicrogame(true);
        }
    }
 }
