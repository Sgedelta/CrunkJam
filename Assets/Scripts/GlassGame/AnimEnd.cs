using UnityEngine;

public class AnimEnd : MonoBehaviour
{
    GlassManager manager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = GameObject.Find("GlassGameManager").GetComponent<GlassManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunEnd()
    {
        manager.AnimEnd();
    }
}
