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
        bool db = GameManager.Instance.DEBUG;
        keyA = GameManager.Instance.inputA.ToInputAction();
        keyB = GameManager.Instance.inputB.ToInputAction();


        keyA.started += (e) => {
            if (db) Debug.Log(e.interaction + " Start");
            if (e.interaction is HoldInteraction)
            {
                aState = InputStates.Held;
                if (db) Debug.Log("A Held Start");
            }

            if (e.interaction is TapInteraction)
            {
                aState = InputStates.Tap;
                if(db) Debug.Log("A tap");
            }

        };
        keyA.performed += (e) => {
            if (db) Debug.Log(e.interaction + " perf");
            if (e.interaction is HoldInteraction)
            {
                if (bState == InputStates.Held)
                {
                    OnBothHeld?.Invoke();
                    if (db) Debug.Log("Both Held from A");
                }
                else
                {
                    OnAHeld?.Invoke();
                    if (db) Debug.Log("A Held Trigger");
                }
            }

            if (e.interaction is TapInteraction)
            {
                if (bState == InputStates.Tap)
                {
                    OnBothPressed?.Invoke();
                    aState = InputStates.None;
                    bState = InputStates.None;
                    if (db) Debug.Log("Both Pressed from A");
                }
                else
                {
                    OnAPressed?.Invoke();
                    if (db) Debug.Log("A Pressed");
                }

            }
        };
        keyA.canceled += (e) => {
            if (db) Debug.Log(e.interaction + " canc");
            if (e.interaction is HoldInteraction)
            {
                if (bState == InputStates.Held)
                {
                    OnBothHoldReleased?.Invoke();
                    if (db) Debug.Log("Both HoldReleased from A");
                }
                else
                {
                    OnAHoldReleased?.Invoke(); 
                    if (db) Debug.Log("A Hold Released");
                }
                aState = InputStates.None;
            }

            if(e.interaction is TapInteraction)
            {
                aState = InputStates.None;
                if (db) Debug.Log("A Tap cancel");
            }
        };

        keyB.started += (e) => {
            if (db) Debug.Log(e.interaction + " Start");
            if (e.interaction is HoldInteraction)
            {
                bState = InputStates.Held;
                if (db) Debug.Log("B Held Set");
            }

            if (e.interaction is TapInteraction)
            {
                bState = InputStates.Tap;
                if (db) Debug.Log("B Tap Set");
                if (db) Debug.Log("A state is " + aState);
            }
        };
        keyB.performed += (e) => {
            if (db) Debug.Log(e.interaction + " perf");
            if (e.interaction is HoldInteraction)
            {
                if(aState == InputStates.Held)
                {
                    OnBothHeld?.Invoke();
                    if (db) Debug.Log("Both Held from B");
                }
                else
                {
                    OnBHeld?.Invoke();
                    if (db) Debug.Log("B Held Trigger");
                }

            }

            if (e.interaction is TapInteraction)
            {
                if(aState == InputStates.Tap)
                {
                    OnBothPressed?.Invoke();
                    aState = InputStates.None;
                    bState = InputStates.None;
                    if (db) Debug.Log("Both tap from B");


                }
                else
                {
                    OnBPressed?.Invoke();
                    if (db) Debug.Log("B Pressed");
                }
                
            }
        };
        keyB.canceled += (e) => {
            if (db) Debug.Log(e.interaction + " canc");
            if (e.interaction is HoldInteraction)
            {
                if (aState == InputStates.Held)
                {
                    OnBothHoldReleased?.Invoke();
                    //aState = InputStates.None;
                    if (db) Debug.Log("Both Held Released from B");
                }
                else
                {
                    OnBHoldReleased?.Invoke();
                    if (db) Debug.Log("B Held Released");
                }
                bState = InputStates.None;
            }

            if (e.interaction is TapInteraction)
            {
                bState = InputStates.None;
                if (db) Debug.Log("B Tap Cancel");
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
