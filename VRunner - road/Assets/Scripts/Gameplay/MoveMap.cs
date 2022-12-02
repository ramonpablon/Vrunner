using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMap : MonoBehaviour {

    public static MoveMap Instance { get; set; }

    public float speed = 50f;

    private void Awake()
    {
        Instance = this;
    }

	// Update is called once per frame
	void Update () {

        if (GameManager.Instance.isGameStarted && !PlayerBehaviour.Instance.isDeath)
            transform.position += new Vector3(0, 0, speed * Time.deltaTime);
        else
            transform.position += new Vector3(0, 0, 0 * Time.deltaTime);
    }
}
