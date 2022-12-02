using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNAStashes : MonoBehaviour {

    private GameObject[] segments;
    public int active;
    int r;
    // Use this for initialization
    void Awake()
    {
        segments = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            segments[i] = transform.GetChild(i).gameObject;

        OnDisable();
    }

    private void OnEnable()
    {
        active = Random.Range(0, 7);
        if(active < 2)
        {
            r = Random.Range(0, segments.Length);
            segments[r].SetActive(true);
        }
        if(active > 2)
        {
            foreach (GameObject go in segments)
                go.SetActive(true);
        }
    }
    private void OnDisable()
    {
        foreach (GameObject go in segments)
            go.SetActive(false);
    }
}