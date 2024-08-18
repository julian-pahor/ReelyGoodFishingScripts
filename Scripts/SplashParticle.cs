using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashParticle : MonoBehaviour
{

    private ParticleSystem ps;
    private RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        ps = GetComponentInChildren<ParticleSystem>();
    }



    public void SetTargetPos(float x)
    {
        rt.position = new Vector3(x, rt.position.y);

        ps.Play();
    }
}
