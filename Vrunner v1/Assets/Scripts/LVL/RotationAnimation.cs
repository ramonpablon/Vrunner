using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAnimation : MonoBehaviour {
    [Header ("Velocidade negativa muda a direção que o bagui gira")]
    public float speed;

	void Update () {
        var angles = transform.rotation.eulerAngles;
        angles.z += Time.deltaTime * speed;
        transform.rotation = Quaternion.Euler(angles);
	}
}
