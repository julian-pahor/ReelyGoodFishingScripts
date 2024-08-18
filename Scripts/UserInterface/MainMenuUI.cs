using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public RectTransform title;
    public RectTransform arrow;
    public RectTransform hand;
    public TextMeshProUGUI highScore;

    private int score;

    private LTDescr handID;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        GameManager.Instance.onStartUp += Enable;
        GameManager.Instance.onGameLoop += Disable;
    }

    public void Enable()
    {
        score = GameManager.Instance.highScore;
        highScore.text = $"High Score : {score.ToString("N0")}";
        LeanTween.moveY(title, 200, 1.0f);
        LeanTween.color(arrow, new Color(1, 1, 1, 0.5f), 0.5f);
        LeanTween.color(hand, new Color(1, 1, 1, 0.5f), 0.5f).setOnComplete(HandMovement);
        LeanTween.moveY(highScore.rectTransform, -225, 1.0f).setDelay(0.3f);
    }

    public void Disable()
    {
        StopMovement();
        LeanTween.moveY(title, 450, 0.5f);
        LeanTween.color(arrow, new Color(1, 1, 1, 0), 0.3f).setDelay(0.3f);
        LeanTween.color(hand, new Color(1, 1, 1, 0), 0.3f).setDelay(0.3f);
        hand.localPosition = new Vector2(11.1f, -100);
        LeanTween.moveY(highScore.rectTransform, -275, 0.5f);
    }

    public void HandMovement()
    {
        handID =  LeanTween.moveY(hand, 0, 1.5f).setLoopPingPong().setEase(LeanTweenType.easeInOutQuart);
    }

    public void StopMovement()
    {
        if(handID != null)
        {
            LeanTween.cancel(handID.id);
        }
       
    }

}
