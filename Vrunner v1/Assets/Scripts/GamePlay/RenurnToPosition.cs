using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenurnToPosition : MonoBehaviour {

    public static RenurnToPosition Instance { get; set; }

    // Use this for initialization
    void Awake () {
        Instance = this;
	}

    public void ReturnPos()
    {
        transform.localPosition = new Vector3(0, -1.7f, 0);
        gameObject.GetComponent<SphereCollider>().enabled = false;
        StartCoroutine(WaitForActive());
    }
    IEnumerator WaitForActive()
    {
        yield return new WaitForSeconds(5);
        gameObject.GetComponent<SphereCollider>().enabled = true;
    }
}
