using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class GroupDisplay : MonoBehaviour
{
    private int MAX_STACK = 4;

    public RectTransform darkBackground;

    public RectTransform fishContainer;

    public GameObject horizontalGroup;

    public FishPopFeed fishPopFeed;

    private List<GroupedDisplay> groupedDisplay = new List<GroupedDisplay>();

    public float displaySpeed;

    private List<Fish> fishList = new List<Fish>();

    private float newScore;

    public TextMeshProUGUI score;

    private struct GroupedDisplay
    { 
        public GroupedDisplay(Fish f, int i, TextMeshProUGUI tmp)
        {
            fishInfo = f;
            fishAmount = i;
            textAmount = tmp;
        }

        public Fish fishInfo;
        public int fishAmount;
        public TextMeshProUGUI textAmount;
    }

    private void Start()
    {

        //GameManager.Instance.onLoopEnd += ;
    }

    private void Update()
    {
        if (GameManager.Instance.getState() != GameManager.State.finished)
        {
            return;
        }

        //detect input

        //Ending Calls;
        //GameManager.Instance.SetHighScore((int)orgScore);

        //GameManager.Instance.startUp();
    }

    public void AddCatch(Fish f)
    {
        fishList.Add(f);
    }

    private void ResetValues()
    {
        groupedDisplay.Clear();

        foreach(Transform c in fishContainer.transform)
        {
            Destroy(c.gameObject);
        }

        darkBackground.gameObject.GetComponent<Image>().color = Color.clear;
        fishContainer.localScale = Vector3.zero;

        
    }

    private void InitiateUI()
    {
        StartCoroutine(theSpawner());
    }

    private IEnumerator theSpawner()
    {
        yield return null;
    }

    //LeanTween.scale(rt, Vector3.one, 0.3f).setEaseOutElastic();
    //LeanTween.scale(rt, Vector3.zero, 0.2f).setEaseInOutBack().setOnComplete(() => Destroy(this.gameObject));



}
