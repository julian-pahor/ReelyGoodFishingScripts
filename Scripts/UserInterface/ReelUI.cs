using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReelUI : MonoBehaviour
{
    public RectTransform reelSphere;
    public RectTransform reelArrow;

    private bool rotateTime = false;

    private float timer = 4;
    private float timerCD;

    // Start is called before the first frame update
    void Start()
    {
        timerCD = timer;

        reelArrow.localScale = Vector3.zero;
        reelSphere.localScale = Vector3.zero;

        GameManager.Instance.onGameLoop += Enable;
        GameManager.Instance.onLoopEnd += Disable;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!rotateTime)
        {
            return;
        }

        if(timerCD <= 0)
        {
            rotateTime = false;
            timerCD = timer;
            Disable();
        }

        if(rotateTime)
        {
            timerCD -= Time.deltaTime;
            reelArrow.localEulerAngles += new Vector3(0, 0, -1.5f);
        }
    }

    void Enable()
    {
        LeanTween.scale(reelSphere, new Vector3(1.5f, 1.5f, 1), 0.3f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(reelArrow, new Vector3(0.5f, 0.5f, 1), 0.2f).setDelay(0.15f).setEase(LeanTweenType.easeOutElastic).setOnComplete(() => rotateTime = true);
    }

    public void Rotate()
    {
        
    }

    void Disable()
    {
        LeanTween.scale(reelArrow, Vector3.zero, 0.15f).setEase(LeanTweenType.easeInElastic);
        LeanTween.scale(reelSphere, Vector3.zero, 0.15f).setDelay(0.1f).setEase(LeanTweenType.easeInElastic);

        rotateTime = false;
    }
}
