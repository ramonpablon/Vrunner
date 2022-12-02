using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurvatureMoviment
{
    up = 0,
    rightUp = 1,
    right = 2,
    rightDown = 3,
    down = 4,
    leftDown = 5,
    left = 6,
    leftUp = 7
}

public class BendMoviment : MonoBehaviour {

    public static BendMoviment Instance { get; set; }


    [HideInInspector]
    public float curvatureX_Action;
    [HideInInspector]
    public float curvatureY_Action;

    private float currentCurvatureX = 0.003f;

    private float currentCurvatureY = 0f;

    private float time;
    private float baseCurve;

    public float speedMultiplier = 1;

    [Space]
    public CurvatureMoviment curvatureMoviment;
    public bool hardnesBend = false;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        curvatureMoviment = CurvatureMoviment.right;

        StartCoroutine(RandomMoviment());
    }

    void Update () {
        time += Time.smoothDeltaTime * speedMultiplier;

        baseCurve = hardnesBend ? 0.006f : 0.003f;

        switch (curvatureMoviment)
        {
            case CurvatureMoviment.up:
                CurvatureX(0f); CurvatureY(baseCurve);
                break;
            case CurvatureMoviment.rightUp:
                CurvatureX(baseCurve); CurvatureY(baseCurve);
                break;
            case CurvatureMoviment.right:
                CurvatureX(baseCurve); CurvatureY(0f);
                break;
            case CurvatureMoviment.rightDown:
                CurvatureX(baseCurve); CurvatureY(-baseCurve);
                break;
            case CurvatureMoviment.down:
                CurvatureX(0f); CurvatureY(-baseCurve);
                break;
            case CurvatureMoviment.leftDown:
                CurvatureX(-baseCurve); CurvatureY(-baseCurve);
                break;
            case CurvatureMoviment.left:
                CurvatureX(-baseCurve); CurvatureY(0f);
                break;
            case CurvatureMoviment.leftUp:
                CurvatureX(-baseCurve); CurvatureY(baseCurve);
                break;
            default:
                break;
        }
    }

    private void CurvatureX(float direction)
    {
        if (direction != currentCurvatureX)
        {
            time = 0;
            currentCurvatureX = direction;
        }

        curvatureX_Action = Mathf.Lerp(curvatureX_Action, direction, time);
    }

    private void CurvatureY(float direction)
    {
        if (direction != currentCurvatureY)
        {
            time = 0;
            currentCurvatureY = direction;
        }

        curvatureY_Action = Mathf.Lerp(curvatureY_Action, direction, time);
    }



    private IEnumerator RandomMoviment()
    {
        while(true)
        {
            int time = Random.Range(5, 10);

            yield return new WaitForSeconds(time);

            curvatureMoviment = (CurvatureMoviment)Random.Range(0, 6);
        }
    }
}
