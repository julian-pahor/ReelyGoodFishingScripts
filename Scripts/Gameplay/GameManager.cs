using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniMammoth;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Vector3 startTouch, endTouch;

    public List<Locations> locations = new List<Locations>();

    private int locationIndex = 0;

    private Fish currentFish = null;

    public FishCatchBar catchPreFab;

    public Transform catchUI;

    public event UnityAction onStartUp;

    public fishTextDisplay ftd;

    public int highScore;

    private bool firstFish = true;

    public AudioSource sfxSource;

    public AudioClip fishFail;

    public AudioClip fishFailBad;

    public AudioClip[] fishCatch;

    public AudioClip casting;

    public AudioClip newHS;

    public AudioClip smallSplash;

    public AudioClip medSplash;

    public AudioClip largeSplash;

    public ParticleSystem catchParticle;

    public SplashParticle splashParticle;

    public FishPopFeed fpdPreFab;

    public GameObject popUp;

    public enum State
    {
        start,
        gaming,
        finished
    }

    private State currentState;
    public State getState()
    {
        return currentState;
    }

    public void startUp()
    {
        //Animate in swipe UI, Title UI, Collection Button
        currentState = State.start;
        Debug.Log("Animate in Swipe UI");
        Debug.Log("Animte in Title UI");
        Debug.Log("Spawn in Collection Button");

        catchUI.localScale = Vector3.zero;
        firstFish = true;

        if(onStartUp != null)
        {
            onStartUp();
        }
    }

    public event UnityAction onGameLoop;
    public void gameLoop()
    {
        LeanTween.scale(catchUI.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeInOutBack);

        sfxSource.PlayOneShot(casting);

        //Instantiate reel crank + hook bar prefabs

        if (onGameLoop != null)
        {
            onGameLoop();
        }

        currentState = State.gaming;
    }

    public event UnityAction onLoopEnd;
    public void loopEnd()
    {

        LeanTween.scale(catchUI.gameObject, Vector3.zero, 0.2f).setEase(LeanTweenType.easeOutBack);

        if (onLoopEnd != null)
        {
            onLoopEnd();
        }

        currentState = State.finished;
    }

    //set up variables and references here
    void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    //subscribe to delegates here
    void OnEnable()
    {
        onGameLoop += Spawn;
        
    }

    //Unsubscribe to delegates here
    void OnDisable()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        GameData data = SaveLoad.LoadData();

        if(data != null)
        {
            highScore = data.HighScore;
        }

        catchUI.localScale = Vector3.zero;
        locations[locationIndex].Initialise();
        Invoke("startUp", 0.5f);
    }

    void Update()
    {
        if (currentState != State.start)
        {
            endTouch = Vector3.positiveInfinity;
            startTouch = Vector3.zero;
            return;
        }

        var primaryTouch = Touchscreen.current;

        if(primaryTouch == null)
        {
            return;
        }

        if(primaryTouch.press.wasPressedThisFrame)
        {
            endTouch = primaryTouch.primaryTouch.position.ReadValue();
        }
        
        if (primaryTouch.primaryTouch.isInProgress)
        {
            startTouch = primaryTouch.position.ReadValue();
        }

        if(primaryTouch.press.wasReleasedThisFrame)
        {
            if (Vector3.Distance(startTouch, endTouch) > Screen.height * 0.12f
      && Mathf.Abs(startTouch.y) > Mathf.Abs(endTouch.y))
            {
                //Was a swipe! Start the game!
                gameLoop();
            }
        }
    }

    public void Spawn()
    {
        currentFish = locations[locationIndex].GetFish();
        Debug.Log($"Current Fish = {currentFish.name}");
        FishCatchBar clone = Instantiate(catchPreFab, catchUI);
        clone.StartFish(currentFish);

        if(firstFish)
        {
            clone.FirstFish();
            firstFish = false;
        }
    }

    public void SetHighScore(int i)
    {
        if (i > highScore)
        {
            highScore = i;
            sfxSource.PlayOneShot(newHS);
            SaveLoad.SaveData(this);
        }
    }

    private void sourceRand()
    {
        sfxSource.pitch = Random.Range(0.9f, 1.1f);
        sfxSource.volume = Random.Range(0.85f, 1.05f);
    }

    public void playCatchSFX(Fish f)
    {
        sourceRand();
        sfxSource.PlayOneShot(fishCatch[Random.Range(0, fishCatch.Length)]);

        switch (f.catchWidth)
        {
            case 30:
                sfxSource.PlayOneShot(smallSplash);
                break;
            case 60:
                sfxSource.PlayOneShot(medSplash);
                break;
            case 100:
                sfxSource.PlayOneShot(largeSplash);
                break;
            default:
                break;
        }
    }

    public void playFailSFX()
    {
        sourceRand();

        if(ftd.GetFishCount() > 0)
        {
            sfxSource.PlayOneShot(fishFail);
        }
        else
        {
            sfxSource.PlayOneShot(fishFailBad);
        }
        
    }

    public void PlayParticle()
    {
        catchParticle.Play();
    }

    public void PopFeed(Fish f)
    {
        var clone = Instantiate(fpdPreFab, popUp.transform);
        clone.Setup(f);
    }
}
