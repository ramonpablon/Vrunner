using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNASpawner : MonoBehaviour {

    public int maxDNAs = 5;
    public float chanceToSpawn = 0.5f;
    public bool forceSpawnAll = false;

    private GameObject[] dnas;

    Vector3 postition;
    private void Awake()
    {
        postition = transform.localPosition;

        dnas = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            dnas[i] = transform.GetChild(i).gameObject;

        OnDisable();
    }
    private void OnEnable()
    {
        transform.localPosition = postition;
        if (Random.Range(0.0f, 1.0f) > chanceToSpawn)
            return;

        if(forceSpawnAll)
            for(int i = 0; i < maxDNAs; i++)
                dnas[i].SetActive(true);
        else
        {
            for(int i = 0; i < maxDNAs; i++)
            {
                int r = Random.Range(0, maxDNAs);
                bool active = (r > maxDNAs / 3);
                dnas[i].SetActive(active);
            }
        }
    }
    private void OnDisable()
    {
        foreach (GameObject go in dnas)
            go.SetActive(false);
    }
}