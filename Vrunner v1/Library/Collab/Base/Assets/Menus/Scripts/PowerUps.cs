using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUps : MonoBehaviour {

    int powerups;

    int randompowerups;
    bool active;

    public Image spriteA, spriteB;

    public Sprite doubleDNA, superVirus, ima, feverReduction, freeFall, freeLinf;

	// Use this for initialization
	void Start () {
        spriteA = spriteA.gameObject.GetComponent<Image>();
        spriteB = spriteB.gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update () {
        RandomPowerUps();

        if (Input.GetKeyDown(KeyCode.X))
            MagneticDNA.Instance.active = true;

        if (Input.GetKeyDown(KeyCode.C))
            MagneticDNA.Instance.active = false;

        if (Input.GetKey(KeyCode.Space))
            GameManager.Instance.febre = 0;


        if (Input.GetKeyDown(KeyCode.B)) // não esta morrendo
        {
            randompowerups = Random.Range(1, 11);
            powerups = randompowerups;
        }

        else if (Input.GetKeyDown(KeyCode.S)) // esta morrendo
        {
            randompowerups = Random.Range(11, 16);
            powerups = randompowerups;
        }
        else
            powerups = 0;
    }
    void RandomPowerUps()
    {
        switch(powerups)
        {
            case 1: spriteA.sprite = doubleDNA; spriteB.sprite = superVirus; break; // 1 2
            case 2: spriteA.sprite = doubleDNA; spriteB.sprite = ima; break; // 1 3
            case 3: spriteA.sprite = doubleDNA; spriteB.sprite = feverReduction; break; // 1 4
            case 4: spriteA.sprite = doubleDNA; spriteB.sprite = freeFall; break; // 1 5
            case 5: spriteA.sprite = superVirus; spriteB.sprite = ima; break; // 2 3
            case 6: spriteA.sprite = superVirus; spriteB.sprite = feverReduction; break; // 2 4
            case 7: spriteA.sprite = superVirus; spriteB.sprite = freeFall; break; // 2 5
            case 8: spriteA.sprite = ima; spriteB.sprite = feverReduction; break; // 3 4
            case 9: spriteA.sprite = ima; spriteB.sprite = freeFall; break; // 3 5
            case 10: spriteA.sprite = feverReduction; spriteB.sprite = freeFall; break; // 4 5


            case 11: spriteA.sprite = doubleDNA; spriteB.sprite = freeLinf; break; // 1 6
            case 12: spriteA.sprite = superVirus; spriteB.sprite = freeLinf; break; // 2 6
            case 13: spriteA.sprite = ima; spriteB.sprite = freeLinf; break; // 3 6
            case 14: spriteA.sprite = feverReduction; spriteB.sprite = freeLinf; break; // 4 6
            case 15: spriteA.sprite = freeFall; spriteB.sprite = freeLinf; break; // 5 6
        }
    }
    public void ChangePowerUp(Image temp)
    {
        if (temp.sprite == doubleDNA)
        {
            GameManager.Instance.active = true;
        }
        if (temp.sprite == superVirus)
        {
            MapMotor.Instance.active = true;
        }
        if (temp.sprite == ima)
        {
            MagneticDNA.Instance.active = true;
        }
        if (temp.sprite == feverReduction)
        {
            GameManager.Instance.febre = 0;
        }
        if (temp.sprite == freeFall)
        {
            print("free");

        }
        if (temp.sprite == freeLinf)
        {
            print("freelinf");

        }
        GameManager.Instance.powerUpActive = true;
        GameManager.Instance.powerupMenu.SetActive(false);
    }

}
