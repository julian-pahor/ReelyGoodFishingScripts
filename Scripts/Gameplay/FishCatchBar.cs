using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishCatchBar : MonoBehaviour
{
    private RectTransform rt;

    public RectTransform catchMeter;
    private Image cmImage;

    //Grab x extents from parent
    private float xMin, xMax;

    private bool catching = false;
    public void setCatching(bool b)
    {
        catching = b;
    }

    private float fillSpeed;

    //10 = can jump whole bar, 1 = small leap
    private float jitterDistance;
    private float jitterSpeed;
    private float jitterTimer;

    private float jitterCooldown;

    //Target to lerp to for jitters
    private Vector3 targetPos;

    private Fish thisFish;

    private bool firstFish;
    private float gracePeriod = 2.0f;
    private float graceTimer;

    private bool leaning = false;

    private void Awake()
    {
        rt = transform.GetComponent<RectTransform>();

        RectTransform pRect = this.GetComponentInParent<RectTransform>();

        if (pRect)
        {
            xMin = - pRect.rect.width / 2;
            xMax = pRect.rect.width / 2;
        }

        cmImage = catchMeter.GetComponent<Image>();

        cmImage.fillAmount = 0.5f;
    }

    public void StartFish(Fish fish)
    {
        thisFish = fish;

        rt.sizeDelta = new Vector2(fish.catchWidth, rt.sizeDelta.y);

        GetComponent<BoxCollider2D>().size = new Vector2(fish.catchWidth, rt.rect.height);
        
        Vector3 randPos = new Vector3(Random.Range(xMin, xMax), 0, 0);

        rt.localPosition = randPos;

        targetPos = rt.localPosition;

        fillSpeed = fish.fillSpeed;
        jitterDistance = fish.cbarDistance;
        jitterSpeed = fish.cbarSpeed;

        jitterCooldown = fish.cbarTime;

        jitterTimer = 0;
    }


    // Start is called before the first frame update
    void Start()
    {
        //Quick tween anim for fish bar spawn
    }

    // Update is called once per frame
    void Update()
    {
        jitterTimer -= Time.deltaTime;

        graceTimer -= Time.deltaTime;

        if(graceTimer < 0)
        {
            firstFish = false;
        }

        if(jitterTimer <= 0.0f)
        {
            jitterTimer = jitterCooldown;
            float dist = 0;

            while(dist < 20 && dist > -20)
            {
                dist = Random.Range(xMin, xMax);
            }

            dist *= jitterDistance / 5.0f; //Difficulty Scaling

            targetPos = new Vector2(Random.Range(-dist, dist), targetPos.y);

            if (targetPos.x > xMax) targetPos.x = xMax;
            if (targetPos.x < xMin) targetPos.x = xMin;
        }

        rt.localPosition = Vector3.Lerp(rt.localPosition, targetPos, Time.deltaTime * jitterSpeed / 2.0f); //MoveSpeed difficulty scaling

        if(catching)
        {

            cmImage.fillAmount += fillSpeed * Time.deltaTime / 4.0f;
        }
        else if (!firstFish)
        {
            cmImage.fillAmount -= Time.deltaTime / 5.0f;
        }

        checkCatch();

    }

    private void LateUpdate()
    {
        catching = false;
    }

    private void checkCatch()
    {
        if(cmImage.fillAmount < 1.0f && cmImage.fillAmount > 0.0f)
        {
            return;
        }
        else if(cmImage.fillAmount >= 1.0f)
        {
            if(!leaning)
            {
                leaning = true;
                GameManager.Instance.splashParticle.SetTargetPos(rt.position.x);
                LeanTween.scale(gameObject, new Vector3(0, 1, 1), 0.4f).setEase(LeanTweenType.easeInBack).setOnComplete(catchMake);
            }
        }
        else
        {
            catchFail();
        }
    }

    private void catchFail()
    {

        GameManager.Instance.playFailSFX();
        GameManager.Instance.loopEnd();
        Destroy(this.gameObject);
    }

    private void catchMake()
    {
        GameManager.Instance.ftd.AddCatch(thisFish);
        //Add a call to add catch to groupindDisplay

        GameManager.Instance.playCatchSFX(thisFish);

        GameManager.Instance.PopFeed(thisFish);

        GameManager.Instance.PlayParticle();

        GameManager.Instance.Spawn();
        Destroy(this.gameObject);
    }

    public void FirstFish()
    {
        firstFish = true;
        graceTimer = gracePeriod;
    }
    
}
