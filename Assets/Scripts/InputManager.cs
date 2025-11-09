using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using System.Collections;

public enum InputStates
{
    None,
    Tap,
    Held
}

public enum InputType
{
    Started,
    Performed,
    Canceled
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

    [SerializeField] float bothInputDelayTime = .1f;

    IEnumerator ExecuteInputDelay(float delay, int key, bool isHoldInteraction, InputType type)
    {
        //this breaks things by running aState as held when it was a tap
        if(type == InputType.Canceled && !isHoldInteraction)
        {
            yield return null;
        }

        yield return new WaitForSeconds(delay);
        RunTapHoldInvokes(key, isHoldInteraction, type);
        yield return null;

    }

    //keycodes so that inputmanager has a local copy -- will intialize these values in Start()
    private InputAction keyA;
    private InputAction keyB;
    private void Start()
    {
        bool db = GameManager.Instance.DEBUG;
        keyA = GameManager.Instance.inputA.ToInputAction();
        keyB = GameManager.Instance.inputB.ToInputAction();

        if(db)
        keyA.started += (e) => {
            if (db) Debug.Log(e.interaction.ToString() + " A start");
            if (e.interaction is TapInteraction) 
            {
                aState = InputStates.Tap;
            }
            else if (e.interaction is HoldInteraction)
            {
                aState = InputStates.Held;
            }
            StartCoroutine(ExecuteInputDelay(bothInputDelayTime, 0, e.interaction is HoldInteraction, InputType.Started));
        };
        keyA.performed += (e) => {
            if (db) Debug.Log(e.interaction.ToString() + " A performed");
            StartCoroutine(ExecuteInputDelay(bothInputDelayTime, 0, e.interaction is HoldInteraction, InputType.Performed));
        };
        keyA.canceled += (e) => {
            if (db) Debug.Log(e.interaction.ToString() + " A canceled");
            StartCoroutine(ExecuteInputDelay(bothInputDelayTime, 0, e.interaction is HoldInteraction, InputType.Canceled));
        };

        keyB.started += (e) => {
            if (db) Debug.Log(e.interaction.ToString() + " B start");
            if (e.interaction is TapInteraction)
            {
                bState = InputStates.Tap;
            }
            else if (e.interaction is HoldInteraction)
            {
                bState = InputStates.Held;
            }
            StartCoroutine(ExecuteInputDelay(bothInputDelayTime, 1, e.interaction is HoldInteraction, InputType.Started));
        };
        keyB.performed += (e) => {
            if (db) Debug.Log(e.interaction.ToString() + " B performed");
            StartCoroutine(ExecuteInputDelay(bothInputDelayTime, 1, e.interaction is HoldInteraction, InputType.Performed));
        };
        keyB.canceled += (e) => {
            if (db) Debug.Log(e.interaction.ToString() + " B canceled");
            StartCoroutine(ExecuteInputDelay(bothInputDelayTime, 1, e.interaction is HoldInteraction, InputType.Canceled));
        };



    }


    public bool CheckForBoth()
    {
        return bState == aState && aState != InputStates.None;
    }

    public void RunTapHoldInvokes(int key, bool isHoldInteraction, InputType type)
    {
        InputStates keyState = key == 0 ? aState : bState;
        InputStates otherState = key == 0 ? bState : aState;
        bool db = GameManager.Instance.DEBUG;
        if (db)
        {
            Debug.Log($"Delayed and ran invokes with information: {key}, {isHoldInteraction}, {type}");
        }

        if (type == InputType.Started)
        {
            if(isHoldInteraction)
            {
                if(keyState != InputStates.Held)
                {
                    //both has triggered from other
                    if (db) Debug.Log("Cancelling hold due to not being held anymore");
                    return;
                }

                if(CheckForBoth())
                {
                    OnBothHeld?.Invoke();
                    //don't reset here because we need them later!
                }
                else
                {
                    if (key == 0)
                    {
                        OnAHeld?.Invoke();
                        
                    }
                    else
                    {
                        OnBHeld?.Invoke();
                        
                    }
                }
            }
        }
        //if not started, it's performed? 
        else if(type == InputType.Performed) 
        {
            //check tap/hold
            if (!isHoldInteraction)
            {
                if(keyState != InputStates.Tap)
                {
                    //both has triggered from other starting it, do nothing
                    if (db) Debug.Log("Cancelling tap due to not being tapped anymore");
                    return;
                }

                if (CheckForBoth())
                {
                    OnBothPressed?.Invoke();
                    aState = InputStates.None;
                    bState = InputStates.None;
                }
                else
                {
                    if(key == 0)
                    {
                        OnAPressed?.Invoke();
                        aState = InputStates.None;
                    } else
                    {
                        OnBPressed?.Invoke();
                        bState = InputStates.None;
                    }
                }

            }
        }
        //we're canceled
        else
        {
            if (isHoldInteraction)
            {
                if (keyState != InputStates.Held)
                {
                    //both has triggered from other
                    if (db) Debug.Log("Cancelling hold due to not being held anymore");
                    return;
                }

                if(CheckForBoth())
                {
                    OnBothHoldReleased?.Invoke();
                    aState = InputStates.None;
                    bState = InputStates.None;
                }
                else
                {
                    if (key == 0)
                    {
                        OnAHoldReleased?.Invoke();
                        aState = InputStates.None;
                    }
                    else
                    {
                        OnBHoldReleased?.Invoke();
                        bState = InputStates.None;
                    }
                }
            }
            
        }
    }

    public void ClearAllEvents(){
        OnAHeld = null;
        OnBHeld = null;
        OnAPressed = null;
        OnBPressed = null;
        OnBothPressed = null;
    }
}
