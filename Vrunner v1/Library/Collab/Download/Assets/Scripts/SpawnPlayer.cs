using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour {

    public GameObject[] playerSkins = new GameObject[0];
    public Transform spawnPoin = null;

	void Start ()
    {
        Instantiate(playerSkins[PlayerPrefs.GetInt("PlayerSkin")], spawnPoin);
	}
}
