using UnityEngine;

public class DefenseMGMangager : MicroGameManager
{
    [SerializeField] public GameObject defender;
    [SerializeField] public Vector2 defaultPos = new Vector2(2.5f, 0);
    [SerializeField] public float verticalChange = 3;

    [SerializeField] GameObject evilGuy;

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
        defender.transform.position = new Vector2(defaultPos.x, defaultPos.y +verticalChange);
        Debug.Log("up");
    }

    /// <summary>
    /// Moves the defender to the lower of the three positions
    /// </summary>
    public void MoveDown()
    {
        defender.transform.position = new Vector2(defaultPos.x, defaultPos.y - verticalChange);
        Debug.Log("down");
    }

    /// <summary>
    /// Resets the position of the defender to its default state in the middle
    /// </summary>
    public void ResetPosition()
    {
        defender.transform.position = new Vector2(2.5f, 0);
    }

    public void MoveEvilGuy()
    {
        int toMove = Mathf.FloorToInt(Random.Range(0, 3));

        //switch (toMove)
        //{
        //    case 0:
        //        defender.transform.position = downPosition;
        //    case 1:
        //        defender.transform.position = middlePosition;
        //    default:
        //        break;
        //}
    }
}
