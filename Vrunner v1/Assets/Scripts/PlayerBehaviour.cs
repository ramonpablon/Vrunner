using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerBehaviour : MonoBehaviour {

    public GameObject burst, burstStashe, crashGorduras, crashPlaquetas;
    bool subindo;//precisa disso pra ele não travar na hora do pulo

	float vertical_Velocity;

	public float 
	gravity,
	gravity_multiplier,//gravidade extra na hora de cair
	jumpForce;

	Vector3 posFix;

	public static bool bulletTime;

	bool vaiPular;

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
		if (MobileInputs.Instance.SwipeUp)
			vaiPular = true;

		transform.Translate (Vector3.up * vertical_Velocity*Time.deltaTime);



		if (transform.localPosition.y <= -1.7f && !subindo)
		{
			vertical_Velocity = 0f;
			transform.localPosition = new Vector3 (transform.localPosition.x, posFix.y, transform.localPosition.z);
			if (vaiPular)
			{
                AudioPlayer.Instance.PowerUpSoundOneShot(0); // reproduz o som

                vertical_Velocity = jumpForce;
				subindo = true;
				vaiPular = false;
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

    #region Colisores de Trigger
    void CrashGorduras(Collider hit)
    {
        CameraShaker.Instance.Shake(CameraShakePresets.Bump); // treme a tela
        AudioPlayer.Instance.PowerUpSoundOneShot(4); // reproduz o som

        hit.gameObject.transform.parent.transform.parent.GetChild(hit.gameObject.transform.parent.transform.parent.childCount - 1).GetComponent<Animator>().SetTrigger("CollisionEnter"); // seta invisivel o obstaculo colidido
        

        GameObject go = Instantiate(crashGorduras, hit.gameObject.transform.position, transform.rotation);
        Destroy(go, 1);
        CameraMotor.Instance.Deadzone(1);
        if(CameraMotor.Instance.time > 0.1f)
            CameraMotor.Instance.totalHitsInDeadzone++;
    }
    void CrashPlaquetas(Collider hit)
    {
        CameraShaker.Instance.Shake(CameraShakePresets.Bump);
        AudioPlayer.Instance.PowerUpSoundOneShot(4); // reproduz o som

        hit.gameObject.transform.parent.transform.parent.GetChild(hit.gameObject.transform.parent.transform.parent.childCount - 1).GetComponent<Animator>().SetTrigger("CollisionEnter");


        GameObject go = Instantiate(crashPlaquetas, hit.gameObject.transform.position, hit.gameObject.transform.rotation);
        Destroy(go, 1);
        CameraMotor.Instance.Deadzone(1);
        if (CameraMotor.Instance.time > 0.1f)
            CameraMotor.Instance.totalHitsInDeadzone++;
    }
    void HitDNA(Collider hit)
    {
        hit.gameObject.transform.parent.GetComponent<RenurnToPosition>().ReturnPos();
        AudioPlayer.Instance.PowerUpSoundOneShot(2); // reproduz o som


        GameObject go = Instantiate(burst, transform.position, transform.rotation);
        Destroy(go, 1);
        GameManager.Instance.GetDNA();
    }
    void HitStashes(Collider hit)
    {
        hit.gameObject.transform.parent.GetComponent<RenurnToPosition>().ReturnPos();
        AudioPlayer.Instance.PowerUpSoundOneShot(3); // reproduz o som


        GameObject go = Instantiate(burstStashe, transform.position, transform.rotation);
        Destroy(go, 1);
        GameManager.Instance.GetDNAStashes();
        GameManager.Instance.GetFebreBar();
    }
	#endregion

    private void OnTriggerEnter(Collider hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Gorduras":
                CrashGorduras(hit); break;

            case "Plaquetas":
                CrashPlaquetas(hit); break;

            case "DNA":
                StartCoroutine(Wait(hit));
                HitDNA(hit); break;

            case "DNAStashes":
                StartCoroutine(Wait(hit));
                HitStashes(hit); break;

		    case"CheckPointFebre":
			GameModeBH.ajusteDaBarreiraDeMonocitos = false;
            GameModeBH.contadorDeObstaculosFebre--; break;

		case "MonocitoFebre":
			GameManager.Instance.OnDeath ();
			GameModeBH.bateuNaFebre = true; break;
		case "CameraShakeShake":
			CameraShaker.Instance.Shake (CameraShakePresets.Earthquake);break;

			
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

    public IEnumerator Wait(Collider hit)
    {
        hit.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        hit.gameObject.SetActive(true);
    }
}
