using Unity.VisualScripting;
using UnityEngine;

public class Stomp : MonoBehaviour
{
    private float stompCooldown;

    private bool stomping;

    [SerializeField] private float stompSpeed;

    [SerializeField] private float stompMinHeight;

    [SerializeField] private float stompMaxHeight;

    [SerializeField] private float stompRechargeTime;
    private void Update()
    {
        if (stomping)
        {
            transform.position -= new Vector3(0, stompSpeed * Time.deltaTime, 0);
            if(transform.position.y <= stompMinHeight)
            {
                stomping = false;
            }
        }
        else if(transform.position.y <= stompMaxHeight)
        {
            transform.position += new Vector3(0, Mathf.Lerp(stompMinHeight, stompMaxHeight, Mathf.Clamp01(stompCooldown/ stompRechargeTime)), 0);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.gameObject.GetComponent<Aliens>().squish)
        {
            GameManager.Instance.EndMicrogame(false);
        }
    }

    public void StompAlien()
    {
        if(stompCooldown > stompRechargeTime)
        {
            stomping = true;
            stompCooldown = 0;
        }
            
    }
}
