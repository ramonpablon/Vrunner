using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameModeBH: MonoBehaviour 
{

	bool 
	pos_fix, //ARRUMA A POSIÇÃO DO PLAYER (PRA NÃO ENTRAR DENTRO DO TUBO OU PULAR ROTACIONADO QUANDO SAI DA FEBRE/FREEFALL E ENTRA NO MODO NORMAL)
	temhinge; //VERIFICA SE TEM HINGE...

	public static bool taNoLugar;

	int modoStore;

	public int modo; //1 NORMAL; 2 FREEFALL; 3 FEBRE

	public static int randInt;

	float
	fastForwarCounter,
	monocitoCounter,
	timeScaleStore,
	virusspeedStore;


	public GameObject 
	barreiraMonocitos,
	cameraMan,
	guia,
	player;

	public GameObject[] monocitos;

	Quaternion
	guiaRotZero,
	cameraManRotZero;

	Vector3
	playerPosZero,
	cameraManPosZero;

	void Start() 
	{
		playerPosZero = player.transform.localPosition;
		cameraManPosZero = cameraMan.transform.localPosition;
		guiaRotZero = guia.transform.rotation;
		cameraManRotZero = cameraMan.transform.localRotation;
		fastForwarCounter = 0f;
		modo = 1;
		modoStore = 1;
		virusspeedStore = player.GetComponent<FreeFallBH> ().virusspeed;
		timeScaleStore = Time.timeScale;
	}

	void Update ()
	{
		//TROCA OS MODOS (SÓ PRA TESTE)
		if (MobileInputs.Instance.SwipeLeft)
		{
			modo = 1;
		}
		else if (MobileInputs.Instance.SwipeRight)
		{
			modo = 3;
		}


		if (modo == 1)
			Mode_Normal ();
		else if (modo == 2)
			Mode_FreeFall ();
		else
			Mode_Febre ();


		monocitoCounter+= Time.deltaTime;
		fastForwarCounter -= Time.deltaTime;

		Debug.Log ("time" + Time.timeScale);
	}

	void Mode_Normal()
	{



		//AJEITA A POSIÇÃO DO MODO NORMAL
		if(!pos_fix)
		{
			player.transform.localPosition = playerPosZero;
			pos_fix = true;
		}

		//ATIVA/DESATIVA SCRIPTS
		guia.GetComponent<Accelerometer> ().enabled = true;
		player.GetComponent<PlayerBehaviour> ().enabled = true;
		player.GetComponent<FreeFallBH> ().enabled = false;	
		cameraMan.GetComponent<CameraMotor> ().enabled = true;

		//MOVIMENTAÇÃO NORMAL DO PLAYER (DIREITA = DREITA, ESQUERDA = ESQUERDA)
		FreeFallBH.flip = 1f;

		//ARRUMA O FIELD OF VIEW DA CAMERA
		cameraMan.GetComponent<Camera>().fieldOfView = 60;
		cameraMan.GetComponent<Animator> ().SetBool ("zoom", false);

		//DESTROI A HINGE E SETA OS CONNSTRAINTS
		if (temhinge)
		{
			Destroy (player.GetComponent<HingeJoint> ());
			player.GetComponent<Rigidbody> ().mass = 1f;
			//player.GetComponent<Rigidbody> ().WakeUp ();
			player.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
			temhinge = false;
		}


	


		LevelManager.Instace.febre = false;



		if (modoStore != modo)
		{
			//fastForwwarCounter = 0;
			fastForwarCounter =  GameObject.Find("1 Transition_24m(Clone)").GetComponent<Segment>().lenght * (GameObject.Find ("LevelManager").GetComponentsInChildren<Segment> ().GetLength (0) -4f)/ MapMotor.Instance.speed;
			modoStore = modo;
		}

		/*
		if (fastForwarCounter >0)// GameObject.Find("1 Transition_24m(Clone)").GetComponent<Segment>().lenght * (GameObject.Find ("LevelManager").GetComponentsInChildren<Segment> ().GetLength (0) -3.5f)/ MapMotor.Instance.speed)
		{
			Time.timeScale += 1.5f*Time.deltaTime;
			player.GetComponent<SphereCollider> ().enabled = false;
		}
		else if (fastForwarCounter >-1 && fastForwarCounter <0)
		{
			Time.timeScale = timeScaleStore;
			player.GetComponent<SphereCollider> ().enabled = true;
		}
		else if (timeScaleStore <= 1.8f)
				timeScaleStore += 0.0001f;
		*/
		taNoLugar = false;

	}

	void Mode_FreeFall()
	{
		pos_fix = false;

		guia.GetComponent<Accelerometer> ().enabled = false;
		player.GetComponent<PlayerBehaviour> ().enabled = false;
		player.GetComponent<FreeFallBH> ().enabled = true;
		cameraMan.GetComponent<CameraMotor> ().enabled = false;



		//AJEITA CAMERA E POSIÇÃO
		guia.transform.rotation = Quaternion.Lerp (guia.transform.rotation, guiaRotZero, 0.05f);
		cameraMan.transform.localPosition = Vector3.Lerp (cameraMan.transform.localPosition ,new Vector3 (cameraManPosZero.x, cameraManPosZero.y, 4*-1.7f*cameraManPosZero.z), 0.05f);
		cameraMan.transform.rotation = Quaternion.Lerp (cameraMan.transform.rotation, Quaternion.LookRotation (guia.transform.position - cameraMan.transform.position), 0.1f);

		//TOCA ANIMAÇÃO
		cameraMan.GetComponent<Animator> ().SetBool ("zoom", true);


		//MOVIMENTAÇÃO INVERTIDA DO PLAYER (DIREITA = ESQUERDA, ESQUERDA = DIREITA, PQ A CAMERA TA DO CONTRARIO)
		FreeFallBH.flip = -1f;

		//CRIA HINGE E CONFIGURA O COMPONENTE
		if (!temhinge)
		{
			player.GetComponent<Rigidbody> ().mass = 0.1f;

			player.AddComponent<HingeJoint> ();
			player.GetComponent<HingeJoint> ().connectedBody = guia.GetComponent<Rigidbody> ();
			player.GetComponent<HingeJoint> ().anchor = Vector3.zero;
			player.GetComponent<HingeJoint> ().autoConfigureConnectedAnchor = false;
			player.GetComponent<HingeJoint> ().connectedAnchor = Vector3.zero;
			player.GetComponent<HingeJoint> ().connectedMassScale = 100;

			player.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

			temhinge = true;
		}


		LevelManager.Instace.febre = true;

		if (modoStore != modo)
		{
			fastForwarCounter = GameObject.Find("1 Transition_24m(Clone)").GetComponent<Segment>().lenght * (GameObject.Find ("LevelManager").GetComponentsInChildren<Segment> ().GetLength (0) -1.5f)/ MapMotor.Instance.speed;
			modoStore = modo;

		//	timeScaleStore = Time.timeScale;

		}
		/*
		if (fastForwarCounter > 0)
		{
			Time.timeScale += 1.5f * Time.deltaTime;
			player.GetComponent<SphereCollider> ().enabled = false;
			player.GetComponent<FreeFallBH> ().virusspeed = 0f;
		}
		else
		{
			Time.timeScale = timeScaleStore*2f;
			player.GetComponent<SphereCollider> ().enabled = true;
			player.GetComponent<FreeFallBH> ().virusspeed = virusspeedStore;
		}
*/



	}

	void Mode_Febre()
	{
		pos_fix = false;

		guia.GetComponent<Accelerometer> ().enabled = false;
		player.GetComponent<PlayerBehaviour> ().enabled = false;
		player.GetComponent<FreeFallBH> ().enabled = true;
		cameraMan.GetComponent<CameraMotor> ().enabled = false;

		guia.transform.rotation = Quaternion.Lerp (guia.transform.rotation, guiaRotZero, 0.05f);
		cameraMan.transform.localPosition = Vector3.Lerp (cameraMan.transform.localPosition ,new Vector3 (cameraManPosZero.x, cameraManPosZero.y, 4f*cameraManPosZero.z), 0.05f);
		//cameraMan.transform.localPosition = Vector3.Lerp (cameraMan.transform.localPosition, cameraManPosZero, 0.05f);
		cameraMan.transform.localRotation = Quaternion.Lerp (cameraMan.transform.localRotation, cameraManRotZero, 0.05f);

		FreeFallBH.flip = 1f;

		cameraMan.GetComponent<Camera>().fieldOfView = 60;

		cameraMan.GetComponent<Animator> ().SetBool ("zoom", false);

		//CRIA HINGE E CONFIGURA O COMPONENTE
		if (!temhinge)
		{
			player.GetComponent<Rigidbody> ().mass = 0.1f;

			player.AddComponent<HingeJoint> ();
			player.GetComponent<HingeJoint> ().connectedBody = guia.GetComponent<Rigidbody> ();
			player.GetComponent<HingeJoint> ().anchor = Vector3.zero;
			player.GetComponent<HingeJoint> ().autoConfigureConnectedAnchor = false;
			player.GetComponent<HingeJoint> ().connectedAnchor = Vector3.zero;
			player.GetComponent<HingeJoint> ().connectedMassScale = 100;

			player.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

			temhinge = true;
		}

	

		LevelManager.Instace.febre = true;

		if (modoStore != modo)
		{
			fastForwarCounter = GameObject.Find("1 Transition_24m(Clone)").GetComponent<Segment>().lenght * (GameObject.Find ("LevelManager").GetComponentsInChildren<Segment> ().GetLength (0) -1f)/ MapMotor.Instance.speed;
			modoStore = modo;


			PlayerBehaviour.bulletTime = false;
			//Time.timeScale = timeScaleStore;
		//	timeScaleStore = Time.timeScale;
		}


		/*
		if (fastForwarCounter > 0)
		{
			
			Time.timeScale += 1.5f * Time.deltaTime;
			player.GetComponent<SphereCollider> ().enabled = false;
			player.GetComponent<FreeFallBH> ().virusspeed = 0f;


		}
		else 
		{
			player.GetComponent<SphereCollider> ().enabled = true;
			player.GetComponent<FreeFallBH> ().virusspeed = virusspeedStore;

			if (!PlayerBehaviour.bulletTime)
				Time.timeScale = timeScaleStore * 3f;
			else
				Time.timeScale = timeScaleStore * 0.5f;
		}

			
*/
		if (!taNoLugar)
		{
			monocitos [randInt].SetActive (true);
			if(fastForwarCounter >0)
				barreiraMonocitos.transform.position = new Vector3 (guia.transform.position.x, guia.transform.position.y, guia.transform.position.z + 180f);
			else
				barreiraMonocitos.transform.position = new Vector3 (guia.transform.position.x, guia.transform.position.y, guia.transform.position.z + 30f);

			randInt = Random.Range (0, monocitos.Length);
			monocitos [randInt].SetActive (false);

			taNoLugar = true;
		}

		if (fastForwarCounter < -10f * 3)
		{
			barreiraMonocitos.transform.position = new Vector3 (guia.transform.position.x, guia.transform.position.y, guia.transform.position.z - 180f);
			modo = 1;
		}
	}
}