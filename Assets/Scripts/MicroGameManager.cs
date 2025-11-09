using UnityEngine;
using System.Collections;

public abstract class MicroGameManager : MonoBehaviour
{
    protected InputManager inputManager;
    public abstract void Initialize(InputManager im, int difficulty);//InputManager im dot bindinput connection happens here as well as any intialize stuff
    protected abstract void BindInput(); //called by initialize ALONE, not by anyone else
    public abstract void LoadScene(); //---- loads the scene somehow
    public abstract void UnloadScene(); //---- unloads the scene somehow (usually delete game objects / send score / all other cleanup needed)

    // public abstract void LeftInput();
    // public abstract void RightInput();
}

