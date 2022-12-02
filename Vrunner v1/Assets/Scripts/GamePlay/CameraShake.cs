using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class CameraShake : MonoBehaviour {

	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
            CameraShaker.Instance.Shake(CameraShakePresets.Bump);
        if (Input.GetKeyDown(KeyCode.A))
        {
            CameraShaker.Instance.Shake(CameraShakePresets.Earthquake);
        }
    }
}
