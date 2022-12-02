using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticDNA : MonoBehaviour {

    public static MagneticDNA Instance { get; set; }

    [HideInInspector] public bool active;
    public GameObject atract;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        Magnetic(active);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag.Equals("Atract") || other.gameObject.tag.Equals("DNAStashes"))
        {
            other.gameObject.transform.position = Vector3.Slerp(other.gameObject.transform.position, transform.position, Time.deltaTime*12);
        }
    }

    void Magnetic(bool active)
    {
        atract.SetActive(active);
        GetComponent<SphereCollider>().radius = active ? 1f : 0.1f;
    }
}
