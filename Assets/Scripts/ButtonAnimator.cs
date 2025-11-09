using System.Collections;
using UnityEngine;

public class ButtonAnimator : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void AnimStart()
    {
        anim.enabled = true;
        StartCoroutine(DestroyInAMin());
    }

    IEnumerator DestroyInAMin()
    {
        yield return new WaitForSeconds(.15f);
        Destroy(anim.gameObject);
    }
}
