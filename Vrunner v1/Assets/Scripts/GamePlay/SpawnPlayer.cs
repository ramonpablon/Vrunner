using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour {

    public GameObject[] playerSkins = new GameObject[0];
    public Transform spawnPoin = null;

	void Start ()
    {
        for(int i = 0; i < playerSkins.Length; i++)
        {
            if (PlayerPrefs.GetInt("Skin" + i) == 1)
            {
                Instantiate(playerSkins[i], spawnPoin);
            }
        }
	}
}
