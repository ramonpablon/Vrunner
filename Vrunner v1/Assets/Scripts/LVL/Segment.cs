using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour {

    public int SegId { get; set; }

    public bool transition; // momento onde não tera nem um tipo de obstáculo

    public int lenght; // quantidade em metros que se estende o segmento
[HideInInspector]
    public int beginY1, beginY2, beginY3;
[HideInInspector]
    public int endY1, endY2, endY3;

    private PieceSpawner[] pieces;

    
    // Use this for initialization
    void Awake () {
        pieces = gameObject.GetComponentsInChildren<PieceSpawner>();

        for (int i = 0; i < pieces.Length; i++)
            foreach (MeshRenderer mr in pieces[i].GetComponentsInChildren<MeshRenderer>())
                mr.enabled = LevelManager.Instace.SHOW_COLLIDER;
    }

    public void Spawn()
    {
        gameObject.SetActive(true);

        for(int i = 0; i < pieces.Length; i++)
        {
            pieces[i].Spawn();
        }
    }

    public void Despawn()
    {
        gameObject.SetActive(false);

        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].Despawn();
        }
    }
}
