using UnityEngine;

public class EenerNeener : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Rigidbody2D rb;

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Velocity", rb.linearVelocity.magnitude);
    }
}
