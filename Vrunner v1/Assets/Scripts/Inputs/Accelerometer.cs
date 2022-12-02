using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerometer : MonoBehaviour {

    Vector3 tilt;

    // Update is called once per frame
    void Update()
    {
        Acc();
    }
    public void Acc() // rotation por accelerometer do celular/ only work in mobile
    {
        tilt = Input.acceleration;
        tilt = Quaternion.Euler(90, 0, 0) * tilt;
        tilt.x = tilt.z;
        transform.Rotate(0, 0, Input.acceleration.x * 400 * Time.deltaTime);

        Debug.DrawRay(transform.position + Vector3.up, tilt, Color.red);
    }
}
