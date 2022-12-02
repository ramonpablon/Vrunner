using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUps : MonoBehaviour
{
    public static PowerUps Instance { get; set; }

    public float deltaTime;
    float _lastframetime;
    float time;

    public int powerups;
    int randompowerups;
    int sound;

    public Image spriteA, spriteB;
    public Sprite doubleDNA, superVirus, ima, feverReduction, freeFall, freeLinf;

    public bool feverReduct;
    public bool inDeadzone;

    public static bool pum;

    private void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start()
    {
        inDeadzone = false;
        spriteA = spriteA.gameObject.GetComponent<Image>();
        spriteB = spriteB.gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        #region CONST_TIME_OF_POWERUP
        deltaTime = Time.realtimeSinceStartup - _lastframetime;
        _lastframetime = Time.realtimeSinceStartup;

        if (time <= 10000)
        {
            time -= time * deltaTime;
        }
        if (time <= 1f || GameModeBH.modo != 1)
        {
            time = 0;
            AudioPowerUpEffects.Instance.PowerUpSoundStop(sound);

            GameManager.Instance.active = false;
            MapMotor.Instance.active = false;
            MagneticDNA.Instance.active = false;
        }
        #endregion

        #region Fever Reduct
        if (GameManager.Instance.febre <= 0.01)
            feverReduct = false;
        if (feverReduct)
            GameManager.Instance.febre = Mathf.Lerp(GameManager.Instance.febre, 0, 2 * Time.deltaTime);
        #endregion

        RandomPowerUps();


        if (GameManager.Instance.mutacao >= 99 && CameraMotor.deadzone == false) // não esta morrendo
        {
            randompowerups = Random.Range(1, 11);
            powerups = randompowerups;
        }
        else if (GameManager.Instance.mutacao >= 99 && CameraMotor.deadzone == true) // esta morrendo
        {
            randompowerups = Random.Range(11, 16);
            powerups = randompowerups;
        }
        else
            powerups = 0;
    }
    void RandomPowerUps()
    {
        switch (powerups)
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
            AudioPowerUpEffects.Instance.PowerUpSoundOneShot(0);
            time = 2000f;
        }
        if (temp.sprite == superVirus)
        {
            sound = 1;
            MapMotor.Instance.active = true;
            AudioPowerUpEffects.Instance.PowerUpSoundLoop(sound);
            time = 1500f;
        }
        if (temp.sprite == ima)
        {
            sound = 2;
            MagneticDNA.Instance.active = true;
            AudioPowerUpEffects.Instance.PowerUpSoundLoop(sound);
            //time = 15;
            time = 5000f;
        }
        if (temp.sprite == feverReduction)
        {
            AudioPowerUpEffects.Instance.PowerUpSoundOneShot(3);
            feverReduct = true;
        }
        if (temp.sprite == freeFall)
        {
            GameModeBH.freeFallATivo = true;
            AudioPowerUpEffects.Instance.PowerUpSoundOneShot(4);
            //time = 15;
        }
        if (temp.sprite == freeLinf)
        {
            pum = true;
            print("freelinf");
            MapMotor.Instance.active = true;
            AudioPowerUpEffects.Instance.PowerUpSoundOneShot(5);
            time = 2;
        }
        GameManager.Instance.powerUpActive = true;
        GameManager.Instance.powerupMenu.SetActive(false);
    }

}
