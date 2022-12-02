using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNASegments : MonoBehaviour {

    public GameObject[] segments;
    int r;
    Vector3 postition;
    // Use this for initialization
    void Awake () {
        postition = transform.localPosition;
        segments = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            segments[i] = transform.GetChild(i).gameObject;

        OnDisable();
	}

    private void OnEnable()
    {
        transform.localPosition = postition;
        r = Random.Range(0, segments.Length);
        segments[r].SetActive(true);
    }
    private void OnDisable()
    {
        foreach (GameObject go in segments)
            go.SetActive(false);
    }
}