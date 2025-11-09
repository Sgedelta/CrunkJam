using UnityEngine;

public class SFX : MonoBehaviour
{

    [Header("Crane Game")]
    [SerializeField] public AudioClip craneMove;
    [SerializeField] public AudioClip craneSquish;
    [Header("Count Game")]
    [SerializeField] public AudioClip countNumberUp;
    [SerializeField] public AudioClip countNumberDown;
    [SerializeField] public AudioClip countAlienRun;
    [Header("Stomp Game")]
    [SerializeField] public AudioClip stompStomp;
    [SerializeField] public AudioClip stompSquish;
    [SerializeField] public AudioClip stompSpike;
    [Header("BB Game")]//bogosbinted
    [SerializeField] public AudioClip BBwoba;
    [SerializeField] public AudioClip BBprinted;
    [SerializeField] public AudioClip BBshake;
    [Header("Walk Game")]
    [SerializeField] public AudioClip walkAlienWalk;
    [SerializeField] public AudioClip walkAlienEnterCage;
    [Header("Container Game")]
    [SerializeField] public AudioClip containerEscape;
    [Header("Defend Game")]
    [SerializeField] public AudioClip defendLazerShoot;
    [SerializeField] public AudioClip defendLazerBlocked;
    [SerializeField] public AudioClip defendLazerMiss;
    [SerializeField] public AudioClip defendGuyMoving;
    [Header("Glass Game")]
    [SerializeField] public AudioClip glassTap;
    [SerializeField] public AudioClip glassClean;
    [Header("Feed Game")]
    [SerializeField] public AudioClip feedPickFood;
    [SerializeField] public AudioClip feedAlienPos;
    [SerializeField] public AudioClip feedAlienNeg;
    [Header("Run Game")]
    [SerializeField] public AudioClip runJump;
    [SerializeField] public AudioClip runCrouch;
    [Header("Dont Game")]
    [SerializeField] public AudioClip dontExplode;
    [SerializeField] public AudioClip dontSUCCESS;
    [Header("Catch Game")]
    [SerializeField] public AudioClip catchBasket;
    [SerializeField] public AudioClip catchGround;
    
    public float volume;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); 
        if (audioSource == null){
            audioSource = gameObject.AddComponent<AudioSource>();
        }   
        volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
    }

    public void PlaySound(AudioClip source){
        if (source == null){
            Debug.LogWarning("SFX: Called a null sound effect.");
            return;
        }

        volume = PlayerPrefs.GetFloat("MasterVolume", 1f); //update volume, or just default to 1;
        audioSource.PlayOneShot(source, volume);
    }


}

