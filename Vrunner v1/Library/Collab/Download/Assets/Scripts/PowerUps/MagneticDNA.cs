using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticDNA : MonoBehaviour {

    public static MagneticDNA Instance { get; set; }
    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag.Equals("Atract") || other.gameObject.tag.Equals("DNAStashes"))
        {
            other.gameObject.transform.position = Vector3.Slerp(other.gameObject.transform.position, transform.position, Time.deltaTime*10);
        }
    }

    public void Magnetic(bool active)
    {
        GetComponent<SphereCollider>().radius = active ? 0.7f : 0.1f;
    }
}
