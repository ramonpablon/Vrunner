using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFallBH : MonoBehaviour 
{
	float freio; //DIMINUI A VELOCIDADE (PRA NÃO "ESCAPAR" DO TUBO) 

	public float virusspeed; //AUMENTA A VELOCIDADE DO PLAYER NO MODO FREEFALL 

	public static float flip = 1f; //CORRIGE O TRANSLATE E O SENTIDO DO VETOR (USADO NO MODO FREEFALL PQ ELE INVERTE A CAMERA)

	Vector2 pospos; //ARMAZENA POSIÇÃO PROXIMA DA PAREDE

	Rigidbody playerRig;

	void Awake()
	{
		playerRig = gameObject.GetComponent<Rigidbody> ();
	}

	void Update()
	{
		FreeFall();
	}
	public void FreeFall ()
	{
		//MOVIMENTAÇÃO
		playerRig.AddForce (new Vector2 (Input.acceleration.x * flip, Input.acceleration.y) * Time.deltaTime * virusspeed * freio);

		//transform.Translate(new Vector2 (Input.acceleration.x * flip, Input.acceleration.y) * Time.deltaTime * virusspeed * freio);

		//SE O PLAYER ESTIVER A UMA DISTANCIA 'X' DO CENTRO APLICA O FREIO
		if (Mathf.Sqrt (Mathf.Pow (transform.position.y, 2) + Mathf.Pow (transform.position.x, 2)) > 1.9f)
			freio = 0f;
		//else
			freio = 1f;

		//MENOS QUICADAS NA PAREDE
		if (Mathf.Sqrt (Mathf.Pow (transform.position.y, 2) + Mathf.Pow (transform.position.x, 2)) < 1.9f)
			pospos = transform.localPosition;
		if (Mathf.Sqrt (Mathf.Pow (transform.position.y, 2) + Mathf.Pow (transform.position.x, 2)) > 1.9f)
			transform.localPosition = Vector2.MoveTowards (transform.localPosition, pospos, 1f);

	}
}

