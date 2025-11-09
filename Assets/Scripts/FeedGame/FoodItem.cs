using UnityEngine;

public class FoodItem : MonoBehaviour
{
    public int color;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        color = GetRandomAOrB(-1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
