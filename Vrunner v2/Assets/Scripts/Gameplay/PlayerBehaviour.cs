using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour {

    public static PlayerBehaviour Instance { get; set; }

    public bool isDeath = false;


    public Text bateu;
    int times = 0;

    // Use this for initialization
    private void Awake () {
        Instance = this;
	}

    // Update is called once per frame
    void Update () {
        transform.localPosition = new Vector3(0, -6.5f, 5);
    }

    private void OnTriggerEnter(Collider trigg)
    {
        if(trigg.gameObject.CompareTag("Death"))
        {
            CameraShaker.Instance.Shake(CameraShakePresets.Bump);
            //isDeath = true;
            times++;
            bateu.text = "Bateu : " + times.ToString();
        }
    }
}
