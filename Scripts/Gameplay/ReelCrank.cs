using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ReelCrank : MonoBehaviour
{
    private Vector3 currentPos;

    public GameObject reelBody;

    public GameObject reelSpot;

    public CatchArrow catchArrow;

    private Vector3 lastProjection;

    private Vector3 curProjection;

    private bool distHit;

    private float radius;


    // Start is called before the first frame update
    void Start()
    { 
        radius = GetComponent<SpriteRenderer>().size.x / 2;
        distHit = true;

        GameManager.Instance.onGameLoop += Enable;
        GameManager.Instance.onLoopEnd += Disable;

        Prep();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //lastRot = transform.position - reelSpot.transform.position;

        var touch = Touchscreen.current;

        if(touch != null)
        {
            if(touch.primaryTouch.isInProgress)
            {
                //Getting positional info of finger
                currentPos = touch.primaryTouch.position.ReadValue();
                currentPos.z = 10;
                Vector3 reelPos = Camera.main.WorldToScreenPoint(reelBody.transform.position);
                currentPos.x = currentPos.x - reelPos.x;
                currentPos.y = currentPos.y - reelPos.y;

                Vector3 dir = currentPos - this.transform.position;
                dir = dir.normalized;
                curProjection = dir * radius;

                transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(currentPos.y, currentPos.x) * Mathf.Rad2Deg));

                if(distHit)
                {
                    lastProjection = curProjection;
                    distHit = false;
                }

                if(Vector3.Distance(curProjection, lastProjection) >= 0.1f)
                {
                    Debug.Log("Adding Power");
                    catchArrow.AddPower();
                    distHit = true;
                }

            }
            else
            {
                curProjection = Vector3.zero;
                lastProjection = Vector3.zero;
                catchArrow.KillPower();
            }


        }
    }

    public void Enable()
    {
        //Tween in
        LeanTween.color(reelBody, new Color(0,0,0,0.4f), 0.5f);
        LeanTween.scale(reelBody, new Vector3(1.5f, 1.5f, 2), 0.25f).setEase(LeanTweenType.easeOutBack);

    }


    public void Disable()
    {
        
        //Tween Out
        LeanTween.scale(reelBody, Vector3.zero, 0.2f).setEase(LeanTweenType.easeOutBack).setOnComplete(Prep);


        //Call prep on tween complete
    }

    public void Prep()
    {
        reelBody.transform.localScale = Vector3.zero;

        gameObject.GetComponent<SpriteRenderer>().color = Color.clear;

    }
}
