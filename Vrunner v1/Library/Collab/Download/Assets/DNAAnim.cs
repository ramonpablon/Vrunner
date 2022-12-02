using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNAAnim : MonoBehaviour {

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Player"))
        {
            anim.SetTrigger("Collected");
        }
    }
}
