using System;
using UnityEngine;
using UnityEngine.XR;

public class Aliens : MonoBehaviour
{
    [SerializeField] public bool squish;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] public float speed = 2f;

    [SerializeField] Boolean squished;

    [SerializeField] SpriteRenderer sprite;

    [SerializeField] Sprite change;
    // Update is called once per frame
    void Update()
    {
        if(!squished)
            rb.linearVelocity = new Vector2(-speed , 0);
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }


    public void Squished()
    {
        squished = true;
        sprite.sprite = change;
        sprite.sortingOrder = -1;
        rb.excludeLayers += LayerMask.GetMask("Alien");

    }
}
