using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BendPiece : MonoBehaviour
{
    Renderer rend;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        rend.material.SetFloat("_CurvatureX", BendMoviment.Instance.curvatureX_Action);
        rend.material.SetFloat("_CurvatureY", BendMoviment.Instance.curvatureY_Action);
    }
}
