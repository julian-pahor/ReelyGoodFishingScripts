using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FishingLine : MonoBehaviour
{
    private LineRenderer line;
    public Transform rodEnd;
    public Transform waterSpot;
    private Vector3 lastWaterSpot;
    public int numPoints;
    [Range(0.0f, 5.0f)]
    public float slackAmount;
    private float lastSlackAmount;
    private float val2;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        lastWaterSpot = waterSpot.position;
        lastSlackAmount = slackAmount;
        SetPositions();
    }

    // Update is called once per frame
    void Update()
    {
        if ((lastWaterSpot != waterSpot.position) || (lastSlackAmount != slackAmount))
        {
            SetPositions();
            lastWaterSpot = waterSpot.position;
            lastSlackAmount = slackAmount;
        }

    }
    private void SetPositions()
    {
        line.positionCount = numPoints;
        line.SetPosition(0, rodEnd.position);
        line.SetPosition(numPoints - 1, waterSpot.position);
        for (int i = 1; i < numPoints - 1; i ++)
        {
            float val = ((float)i + 1) / (numPoints + 1);
            if (val > 0.5f)
            {
                val2 = (0.5f - (val - 0.5f)) * 2;
            }
            else
            {
                val2 = val * 2;
            }
            //line.SetPosition(i, Vector3.Lerp(rodEnd.position, waterSpot.position, val));
                line.SetPosition(i, new Vector3(Vector3.Lerp(rodEnd.position, waterSpot.position, val).x, Vector3.Lerp(rodEnd.position, waterSpot.position, val).y - (val2 * slackAmount), Vector3.Lerp(rodEnd.position, waterSpot.position, val).z));
        }
    }
    private void SetSlack()
    {
        slackAmount = 10 - (Vector3.Distance(rodEnd.position, waterSpot.position) * .10f);
        if (slackAmount < 0)
        {
            slackAmount = 0;
        }
    }

}
