using UnityEngine;

public class DefenseMGMangager : MicroGameManager
{
    [SerializeField] GameObject defender;
    [SerializeField] private Vector3 upPosition = new Vector3(2.5f, 2.5f, 0);
    [SerializeField] private Vector3 downPosition = new Vector3(2.5f, -2.5f, 0);
    public override void Initialize(InputManager im)
    {
        im.OnAHeld.AddListener(MoveUp);
        im.OnBHeld.AddListener(MoveDown);
        
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

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Moves the defender to the upper of the three positions
    /// </summary>
    public void MoveUp()
    {
        defender.transform.position = upPosition;
        Debug.Log("up");
    }

    /// <summary>
    /// Moves the defender to the lower of the three positions
    /// </summary>
    public void MoveDown()
    {
        defender.transform.position = downPosition;
        Debug.Log("down");
    }

    /// <summary>
    /// Resets the position of the defender to its default state in the middle
    /// </summary>
    public void ResetPosition()
    {

    }
}
