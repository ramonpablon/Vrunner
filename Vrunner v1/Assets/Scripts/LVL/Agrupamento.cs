using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agrupamento : MonoBehaviour {

    Renderer Rend;
    public Material[] mat;
    float pingPong;

    // Use this for initialization
    void Start()
    {
        Rend = GetComponent<Renderer>();
    }

    void Update()
    {
        pingPong = Mathf.PingPong(Time.time, 6) / 6;
        
        Rend.material.Lerp(mat[0], mat[1], pingPong);
    }
}