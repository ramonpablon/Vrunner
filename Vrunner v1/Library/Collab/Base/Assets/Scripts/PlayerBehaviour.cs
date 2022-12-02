using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    public GameObject burst, burstStashe;
    bool subindo;//precisa disso pra ele não travar na hora do pulo

	float vertical_Velocity;

	public float 
	gravity,
	gravity_multiplier,//gravidade extra na hora de cair
	jumpForce;

	Vector3 posFix;


	public static bool bulletTime;

	void Start()
	{
		posFix = transform.localPosition;
	}

    void Update()
    {
		Jump ();
    }

	void Jump()
	{
		transform.Translate (Vector3.up * vertical_Velocity*Time.deltaTime);

		if (transform.localPosition.y <= -1.7f && !subindo)
		{
			vertical_Velocity = 0f;
			transform.localPosition = new Vector3 (transform.localPosition.x, posFix.y, transform.localPosition.z);
			if (MobileInputs.Instance.SwipeUp)
			{
				vertical_Velocity = jumpForce;
				subindo = true;
			}
		}
		else
		{
			vertical_Velocity -= Time.deltaTime * gravity;

			if (MobileInputs.Instance.SwipeDown)
				vertical_Velocity = -jumpForce;
			
			if (vertical_Velocity < 0)
			{
				vertical_Velocity -= gravity_multiplier* Time.deltaTime * gravity;
				subindo = false;
			}
		}
	}

    void Crash()
    {
        CameraMotor.Instance.Deadzone(1);
    }
    void HitDNA()
    {
        GameObject go = Instantiate(burst, transform.position, transform.rotation);
        Destroy(go, 1);
        GameManager.Instance.GetDNA();
    }
    void HitStashes()
    {
        GameObject go = Instantiate(burstStashe, transform.position, transform.rotation);
        Destroy(go, 1);
        GameManager.Instance.GetDNAStashes();
        GameManager.Instance.GetFebreBar();
    }
    private void OnTriggerEnter(Collider hit)
    {
		switch (hit.gameObject.tag)
        {
            case "Obstacles":
                hit.gameObject.transform.parent.transform.parent.GetChild(hit.gameObject.transform.parent.transform.parent.childCount-1).GetComponent<Animator>().SetTrigger("CollisionEnter");
                Crash(); break;
            case "DNA":
               /* hit.gameObject.GetComponent<Animator>().SetTrigger("Collected");*/ HitDNA(); break;
            case "DNAStashes":
               /* hit.gameObject.GetComponent<Animator>().SetTrigger("StasheCollected");*/ HitStashes(); break;
			case"CheckPointFebre":
			GameModeBH.taNoLugar = false; break;
			//case "MonocitoFebre":
			//morre; break;

        }
    }

	void OnTriggerStay (Collider other)
	{
		if(other.gameObject.tag.Equals("BulletTimeTrigger"))
			bulletTime = true;
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag.Equals ("BulletTimeTrigger"))
			bulletTime = false;	
	}
}
