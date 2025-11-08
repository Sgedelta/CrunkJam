using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    //Defining all the events:

    public event Action OnAPressed;
    public event Action OnBPressed;
    public event Action OnBothPressed;
    public event Action OnAHeld;
    public event Action OnBHeld;


    //keycodes so that inputmanager has a local copy -- will intialize these values in Start()
    private KeyCode keyA;
    private KeyCode keyB;
    private void Start()
    {
        //get keyA and keyB values from the gamemanager(this way if the keycode is changed it changes here too)
        //KeyA = gameManager.inputA or something idk
        KeyA = GameManager.Instance.inputA;
        KeyB = GameManager.Instance.inputB;
    }

    private void Update() //VERY VERY BAREBONES -- can and SHOULD be improved upon its 5:45 am cut me some slack
    {
        bool aDown = Input.GetKeyDown(keyA);
        bool bDown = Input.GetKeyDown(keyB);
        bool aHeld = Input.GetKey(keyA);
        bool bHeld = Input.GetKey(keyB);


        if (aDown && bDown){
            OnBothPressed?.Invoke();
        }else
        {
            if (aDown) OnAPressed?.Invoke();
            if (bDown) OnBPressed?.Invoke();
        }

        if (aHeld) OnAHeld?.Invoke();
        if (bHeld) OnBHeld?.Invoke();
    }
    public void ClearAllEvents(){
        OnAHeld = null;
        OnBHeld = null;
        OnAPressed = null;
        OnBPressed = null;
        OnBothPressed = null;
    }
}
