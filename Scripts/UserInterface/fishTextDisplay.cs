using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class fishTextDisplay : MonoBehaviour
{
    public LeanTweenType inCurve;
    public LeanTweenType outCurve;

    public float openDuration;
    public float openDelay;
    public float outDuration;
    public float outDelay;

    private string orgFishName;
    private string orgFishDescription;

    private float orgScore;
    private float updatedScore;

    private int catches;

    private string currentFishName;
    private string currentFishDesc;

    public RectTransform darkBackground;
    public RectTransform imageBackground;
    public RectTransform descriptionBox;

    public TextMeshProUGUI fishName;
    public Image fishImage;
    public TextMeshProUGUI fishDesctiption;
    public TextMeshProUGUI score;
    public TextMeshProUGUI catchCount;
    public TextMeshProUGUI fishScore;

    public float nameDisplaySpeed;
    public float descriptionDisplaySpeed;
    public float scoreDisplaySpeed;


    private bool tweened = false;
    private LTDescr id1, id2, id3, id4, id5, id6;

    private List<Fish> fish = new List<Fish>();

    public int GetFishCount()
    {
        return fish.Count;
    }

    private List<CountList> fishUIList = new List<CountList>();

    private int fishIndex;

    private struct CountList
    {
        public CountList(Fish f, int i)
        {
            fishInfo = f;
            fishAmount = i;
        }

        public Fish fishInfo;
        public int fishAmount;
    }


    void Start()
    {
        fishIndex = 0;

        Toggle(false);

        GameManager.Instance.onLoopEnd += ResetValue;
        GameManager.Instance.onLoopEnd += InitiateUI;

    }
    void Update()
    {
        if(GameManager.Instance.getState() != GameManager.State.finished)
        {
            return;
        }

        if (Touchscreen.current.press.wasPressedThisFrame)
        {
            if(tweened && fishIndex < fishUIList.Count)
            {
                //move onto the next tween
                SoftResetValue();
                InitiateUI();
                tweened = false;
            }
            else if(!tweened && fishUIList.Count != 0)
            {
                //jump tween to finished and set flag ready to start next one
                SkipTween();
            }
            else// if(fishIndex >= fish.Count)
            {
                fish.Clear();

                GameManager.Instance.SetHighScore((int)orgScore);

                GameManager.Instance.startUp();

                Toggle(false);
            }
        }
    }

    public void ResetValue()
    {
        if (fish.Count <= 0)
        {
            return;
        }

        FillList();

        fishIndex = 0;
        orgScore = 0;
        tweened = false;

        darkBackground.gameObject.GetComponent<Image>().color = Color.clear;
        imageBackground.localScale = Vector3.zero;
        descriptionBox.localScale = Vector3.zero;

        fishScore.text = fishUIList[fishIndex].fishInfo.pointScore.ToString();

        Toggle(true);

        fishImage.sprite = fishUIList[fishIndex].fishInfo.fishPortrait;

        catchCount.text = "";

        orgFishName = fishUIList[fishIndex].fishInfo.name;
        fishName.text = "";

        orgFishDescription = fishUIList[fishIndex].fishInfo.description;
        fishDesctiption.text = "";

        updatedScore = orgScore;
        orgScore += fishUIList[fishIndex].fishInfo.pointScore; 
        score.text = "";

        fishIndex++;
    }
    public void SoftResetValue()
    {
        imageBackground.localScale = Vector3.zero;
        descriptionBox.localScale = Vector3.zero;

        fishScore.text = fishUIList[fishIndex].fishInfo.pointScore.ToString();

        fishImage.sprite = fishUIList[fishIndex].fishInfo.fishPortrait;

        catchCount.text = "";

        orgFishName = fishUIList[fishIndex].fishInfo.name;
        fishName.text = "";

        orgFishDescription = fishUIList[fishIndex].fishInfo.description;
        fishDesctiption.text = "";

        updatedScore = orgScore;
        orgScore += fishUIList[fishIndex].fishInfo.pointScore * fishUIList[fishIndex].fishAmount; 

        fishIndex++;
    }
    public void InitiateUI()
    {
        if (fish.Count <= 0)
        {
            GameManager.Instance.startUp();
            return;
        }

        LeanTween.color(darkBackground, new Color(0, 0, 0, 0.75f), 1.0f).setDelay(0f);
        id1 = LeanTween.scale(imageBackground, Vector3.one, openDuration).setDelay(openDelay).setEase(inCurve);
        id2 = LeanTween.scale(descriptionBox, Vector3.one, openDuration).setDelay(openDelay).setEase(inCurve);

        float valueDelay = openDelay + (openDuration * 1.0f);

        id3 = LeanTween.value(gameObject, 0, (float)orgFishName.Length, nameDisplaySpeed).setEase(LeanTweenType.easeInOutSine).
            setOnUpdate((float val) =>
            {
                fishName.text = orgFishName.Substring(0, Mathf.RoundToInt(val));
            }).setDelay(valueDelay);


        valueDelay += (orgFishName.Length * (nameDisplaySpeed / orgFishName.Length));

        id4 = LeanTween.value(gameObject, 0, (float)orgFishDescription.Length, descriptionDisplaySpeed).setEase(LeanTweenType.easeInOutSine).
            setOnUpdate((float val) =>
            {
                fishDesctiption.text = orgFishDescription.Substring(0, Mathf.RoundToInt(val));
            }).setDelay(valueDelay);

        valueDelay += (orgFishDescription.Length * (descriptionDisplaySpeed / orgFishDescription.Length));


        id5 = LeanTween.value(gameObject, updatedScore, orgScore, scoreDisplaySpeed).setEase(LeanTweenType.easeInOutSine).
            setOnUpdate((float val) =>
            {
                int i = (int)val;
                score.text = i.ToString("N0");

            }).setDelay(valueDelay).setOnComplete(() => tweened = true );

        id6 = LeanTween.value(gameObject, 0, fishUIList[fishIndex - 1].fishAmount, scoreDisplaySpeed / 2).setEase(LeanTweenType.easeInOutSine).
            setOnUpdate((float val) =>
            {
                int i = (int)val;
                catchCount.text = $"x{i}";
            }).setDelay(openDelay);
    }

    public void SkipTween()
    {
        id1.setDelay(0);
        id2.setDelay(0);
        id3.setDelay(0);
        id4.setDelay(0);
        id5.setDelay(0);
        id6.setDelay(0);

        id1.passed = openDuration;
        id2.passed = openDuration;
        id3.passed = nameDisplaySpeed;
        id4.passed = descriptionDisplaySpeed;
        id5.passed = scoreDisplaySpeed;
        id6.passed = scoreDisplaySpeed / 2;

        tweened = true;
    }

    private void Toggle(bool b)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(b);
        }
    }

    public void AddCatch(Fish f)
    {
        fish.Add(f);
    }

    private void FillList()
    {
        fishUIList.Clear();

        bool newFish = true;
        int index = 0;

        for(int i = 0; i < fish.Count; i++)
        {
            if(fishUIList.Count == 0)
            {
                fishUIList.Add(new CountList(fish[i], 1));
            }
            else
            {
                index = 0;

                foreach(CountList cl in fishUIList)
                {
                    newFish = true;

                    if(fish[i].fishName == cl.fishInfo.fishName)
                    {
                        newFish = false;
                        break;
                    }

                    index++;

                }

                if(newFish)
                {
                    fishUIList.Add(new CountList(fish[i], 1));
                }
                else
                {
                    fishUIList.Add(new CountList(fish[i], (fishUIList[index].fishAmount + 1)));
                    fishUIList.RemoveAt(index);
                }
            }


        }
    }
}
