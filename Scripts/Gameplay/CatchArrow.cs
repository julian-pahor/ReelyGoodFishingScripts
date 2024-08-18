using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CatchArrow : MonoBehaviour
{
    private RectTransform rt;
    private RaycastHit2D catchRay;

    private float xMin, xMax;

    public float reelPower;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();

        //hard coded magic numbers because parent was being weird
        xMin = -120;
        xMax = 120;
    }

    // Update is called once per frame
    void Update()
    {
        catchRay = Physics2D.Raycast(transform.position, Vector2.up);

        if(catchRay.collider != null)
        {
            FishCatchBar fcb = catchRay.collider.gameObject.GetComponent<FishCatchBar>();

            if(fcb != null)
            {
                fcb.setCatching(true);
            }
        }

        reelPower = Mathf.Clamp(reelPower, 0, 7);

        float reelScaleX = ((reelPower / 7) * (xMax * 2)) - xMax;
        
        rt.localPosition = Vector2.Lerp(rt.localPosition, new Vector2(reelScaleX, rt.localPosition.y), Time.deltaTime * 2);

        if (transform.localPosition.x > xMax)
        {
            transform.localPosition = new Vector2(xMax, transform.localPosition.y);
        }
        if(transform.localPosition.x < xMin)
        {
            transform.localPosition = new Vector2(xMin, transform.localPosition.y);
        }

        reelPower -= 0.06f;
    }

    public void AddPower()
    {
        reelPower += .275f;
    }

    public void KillPower()
    {
        reelPower -= 0.07f;
    }
}
