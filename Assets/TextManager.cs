using UnityEngine;
using UnityEngine.UI;

public class TextManager : MicroGameManager
{
    [SerializeField] Text text;

    public override void Initialize(InputManager im)
    {
  
    }

    public override void LoadScene()
    {

    }

    public override void UnloadScene()
    {

    }

    protected override void BindInput()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.text = $"Games Completed:" + GameManager.Instance.MinigamesCompleted.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
