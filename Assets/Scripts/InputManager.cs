using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public enum InputStates
{
    None,
    Tap,
    Held
}

public class InputManager : MonoBehaviour
{
    //Defining all the events:

    public UnityEvent OnAPressed;
    public UnityEvent OnBPressed;
    public UnityEvent OnBothPressed;
    public UnityEvent OnAHeld;
    public UnityEvent OnBHeld;
    public UnityEvent OnBothHeld;
    public UnityEvent OnAHoldReleased;
    public UnityEvent OnBHoldReleased;
    public UnityEvent OnBothHoldReleased;


    InputStates aState = InputStates.None;
    InputStates bState = InputStates.None;



    //keycodes so that inputmanager has a local copy -- will intialize these values in Start()
    private InputAction keyA;
    private InputAction keyB;
    private void Start()
    {

        keyA = GameManager.Instance.inputA.ToInputAction();
        keyB = GameManager.Instance.inputB.ToInputAction();


        keyA.started += (e) => {
            //Debug.Log(e.interaction + " Start");
            if (e.interaction is HoldInteraction)
            {
                aState = InputStates.Held;
            }

            if (e.interaction is TapInteraction)
            {
                aState = InputStates.Tap;
                //Debug.Log("A tap");
            }

        };
        keyA.performed += (e) => {
           // Debug.Log(e.interaction + " perf");
            if (e.interaction is HoldInteraction)
            {
                if (bState == InputStates.Held)
                {
                    OnBothHeld?.Invoke();
                }
                else
                {
                    OnAHeld?.Invoke();
                }
            }

            if (e.interaction is TapInteraction)
            {
                if (aState == InputStates.Tap)
                {
                    OnBothPressed?.Invoke();
                    aState = InputStates.None;
                    bState = InputStates.None;
                    //Debug.Log("Both from A");
                }
                else
                {
                    OnAPressed?.Invoke();
                }

            }
        };
        keyA.canceled += (e) => {
            //Debug.Log(e.interaction + " canc");
            if (e.interaction is HoldInteraction)
            {
                if (bState == InputStates.Held)
                {
                    OnBothHoldReleased?.Invoke();
                    aState = InputStates.None;
                    bState = InputStates.None;
                }
                else
                {
                    OnAHoldReleased?.Invoke(); 
                    aState = InputStates.None;
                }
            }

            if(e.interaction is TapInteraction)
            {
                aState = InputStates.None;
                //Debug.Log("A cancel");
            }
        };

        keyB.started += (e) => {
           // Debug.Log(e.interaction + " Start");
            if (e.interaction is HoldInteraction)
            {
                bState = InputStates.Held;
            }

            if (e.interaction is TapInteraction)
            {
                bState = InputStates.Tap;
            }
        };
        keyB.performed += (e) => {
            //Debug.Log(e.interaction + " perf");
            if (e.interaction is HoldInteraction)
            {
                if(aState == InputStates.Held)
                {
                    OnBothHeld?.Invoke();
                }
                else
                {
                    OnBHeld?.Invoke();
                }

            }

            if (e.interaction is TapInteraction)
            {
                if(aState == InputStates.Tap)
                {
                    OnBothPressed?.Invoke();
                    aState = InputStates.None;
                    bState = InputStates.None;
                    //Debug.Log("Both from B");


                }
                else
                {
                    OnBPressed?.Invoke();
                }
                
            }
        };
        keyB.canceled += (e) => {
            //Debug.Log(e.interaction + " canc");
            if (e.interaction is HoldInteraction)
            {
                if (aState == InputStates.Held)
                {
                    OnBothHoldReleased?.Invoke();
                    aState = InputStates.None;
                    bState = InputStates.None;
                }
                else
                {
                    OnBHoldReleased?.Invoke();
                    bState = InputStates.None;
                }
            }

            if (e.interaction is TapInteraction)
            {
                bState = InputStates.None;
            }
        };
    }

    public void ClearAllEvents(){
        OnAHeld = null;
        OnBHeld = null;
        OnAPressed = null;
        OnBPressed = null;
        OnBothPressed = null;
    }
}
