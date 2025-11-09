using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject squishAlien;
    [SerializeField] private GameObject spikeAlien;
    public void SpawnAlien()
    {
        float rand = Random.Range(0f, 1f);
        if(rand < 0.5f)
        {
            Instantiate(squishAlien, gameObject.transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(spikeAlien, gameObject.transform.position, Quaternion.identity);
        }
    }
}
