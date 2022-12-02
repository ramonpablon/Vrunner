﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour {

	public int SegId { get; set; }
    public bool transition;

    public int lenght;

    private PieceSpawner[] pieces;

    private void Awake()
    {
        pieces = gameObject.GetComponentsInChildren<PieceSpawner>();

        for (int i = 0; i < pieces.Length; i++)
            foreach (MeshRenderer mr in pieces[i].GetComponentsInChildren<MeshRenderer>())
                mr.enabled = LevelManager.Instance.show_Collider;
    }

    public void Spawn()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < pieces.Length; i++)
            pieces[i].Spawn();
    }

    public void DeSpawn()
    {
        gameObject.SetActive(false);

        for (int i = 0; i < pieces.Length; i++)
            pieces[i].DeSpawn();
    }
}
