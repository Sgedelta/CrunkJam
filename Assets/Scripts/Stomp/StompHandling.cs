using UnityEngine;

public class StompHandling : MonoBehaviour
{
    

    private bool stomping;

    [SerializeField] private float stompSpeed;

    [SerializeField] private float stompMinHeight;

    [SerializeField] private float stompMaxHeight;

    [SerializeField] private float stompRechargeTime;

    private float stompCooldown;

    private void Start()
    {
        stompCooldown = stompRechargeTime;
    }
    private void Update()
    {
        if (stomping)
        {
            transform.position -= new Vector3(0, stompSpeed * Time.deltaTime, 0);
            if (transform.position.y <= stompMinHeight)
            {
                stomping = false;
            }
        }
        else if (transform.position.y <= stompMaxHeight)
        {
            transform.position = new Vector3(0, Mathf.Clamp(stompMinHeight + (stompMaxHeight-stompMinHeight) * (stompCooldown/stompRechargeTime), stompMinHeight, stompMaxHeight), 0);
            stompCooldown += Time.deltaTime;
        }

        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<Aliens>().squish)
        {
            GameManager.Instance.EndMicrogame(false);
        }
        else
        {
            collision.gameObject.GetComponent<Aliens>().Squished();
        }
    }

    public void StompAlien()
    {
        if (stompCooldown > stompRechargeTime)
        {
            stomping = true;
            stompCooldown = 0;
        }

    }
}
