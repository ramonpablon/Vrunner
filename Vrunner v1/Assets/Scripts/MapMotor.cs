using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMotor : MonoBehaviour
{
    public static MapMotor Instance { get; set; }

    [HideInInspector] public float speed;
    [HideInInspector] public float speedInicialDoMapMotor;
    [HideInInspector] public bool active = false;

    bool colliderVirus = false;
    float pingPong;


    public GameObject pplayer, auraSpeed;
    public ParticleSystem burst;
    public bool livLinf;

    // Use this for initialization
    void Awake()
    {
        Instance = this;

        speed = 11;

        speedInicialDoMapMotor = speed;
    }
    // Update is called once per frame
    void Update()
    {
        SuperVirus(active);
        transform.Translate(new Vector3(0, 0, 1) * speed * Time.deltaTime);

        if (PowerUps.pum)
            burst.Play(true);
    }

    void SuperVirus(bool active)
    {
        pingPong = Mathf.PingPong(Time.time, 1);

        if (!colliderVirus)
        {
            CameraMotor.deadzone = false;
            pplayer.GetComponentInChildren<Renderer>().material.SetFloat("_Glossiness", pingPong);
        }



        if (active)
        {
            auraSpeed.SetActive(true);
            speed += 5 * Time.deltaTime;
            colliderVirus = false;
        }
        if (!active && speed > 11)
        {
            speed -= 10 * Time.deltaTime;
        }
        if (!active && speed > 11 && speed < 12)
        {
            auraSpeed.SetActive(false);
            burst.Play(true); // partviles
            PowerUps.pum = false;
        }
        if (speed <= 11)
        {
            colliderVirus = true;
        }
    }
}
