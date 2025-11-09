using System;
using System.Collections;
using UnityEngine;

public class DoorDisplayControl : MonoBehaviour
{
    [SerializeField] private SpriteRenderer RInstruct;
    [SerializeField] private SpriteRenderer RNum;
    [SerializeField] private SpriteRenderer LInstruct;
    [SerializeField] private SpriteRenderer LNum;


    public IEnumerator SetSprites(Sprite numbers, Tuple<Sprite, Sprite> instructions)
    {
        Debug.Log("Setting Sprites!!");

        LInstruct.sprite = instructions.Item1;
        RInstruct.sprite = instructions.Item2;

        LNum.sprite = numbers;
        RNum.sprite = numbers;
        yield return null;
    }

}
