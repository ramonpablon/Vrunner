using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class GameModeBH : MonoBehaviour
{
    public static GameModeBH instance { get; set; }

    bool
    ajusteFinalFebre,
    pos_fix, //ARRUMA A POSIÇÃO DO PLAYER (PRA NÃO ENTRAR DENTRO DO TUBO OU PULAR ROTACIONADO QUANDO SAI DA FEBRE/FREEFALL E ENTRA NO MODO NORMAL)
    temhinge; //VERIFICA SE TEM HINGE...


    public static bool
    ajusteDaBarreiraDeMonocitos,
    bateuNaFebre,
    emTransicao;

    int
    modoStore,
    numeroDeVezes,
    randInt;

    public int
    incrementoNumeroDeObstaculosFebre,
    numeroBaseObstaculosFebre;

    public static int contadorDeObstaculosFebre;

    public static int modo; //1 NORMAL; 2 FREEFALL; 3 FEBRE //  

    float
    contadorDeTempoFreeFall,
    posx,
    posy,
    timertimer,
    virusspeedStore;

    public float duracaoBaseDoFreeFall;

    public static float skipCounter;

    GameObject barreirademonocitosGO;

    public GameObject
    barreiraMonocitos,
    cameraMan,
    guia,
    player;

    public GameObject[] monocitos;

    Quaternion
    cameraManRotZero,
    guiaRotZero;


    Vector3
    cameraManPosZero,
    guiaPosZero;

    GameObject luzDoTubo;

    public float speedDaLuz;

    public Behaviour corDaFebre;

    public static bool freeFallATivo;

    public static GameObject playerMesh;

    Quaternion playerMeshRotationZero;

    GameObject freefall_DNA,
    freefall_DNA_2;

    void Awake()
    {

        guiaPosZero = guia.transform.position;
        cameraManPosZero = cameraMan.transform.localPosition;
        guiaRotZero = guia.transform.rotation;
        cameraManRotZero = cameraMan.transform.localRotation;
        skipCounter = 0f;
        modo = 1;
        modoStore = 1;
        virusspeedStore = player.GetComponent<FreeFallBH>().virusspeed;
        emTransicao = false;
        barreirademonocitosGO = GameObject.Find("BarreiraMonocitosFebre");
        luzDoTubo = GameObject.FindGameObjectWithTag("Luz do Tubo");

        contadorDeObstaculosFebre = 0;
        freeFallATivo = false;

        playerMesh = GameObject.FindGameObjectWithTag("PlayerMesh");

        playerMeshRotationZero = playerMesh.transform.localRotation;

        freefall_DNA = GameObject.FindGameObjectWithTag("Freefall_DNA");
        freefall_DNA_2 = GameObject.FindGameObjectWithTag("Freefall_DNA 2");

    }

    void Start()
    {
        pos_fix = true;
        bateuNaFebre = false;
        freefall_DNA.SetActive(false);
        freefall_DNA_2.SetActive(false);
    }
    void FixedUpdate()
    {
        if (skipCounter < -600)
        {
            if (GameManager.Instance.pcamera.GetComponent<UnityStandardAssets.ImageEffects.BloomAndFlares>().bloomIntensity <= 20f)
                GameManager.Instance.pcamera.GetComponent<UnityStandardAssets.ImageEffects.BloomAndFlares>().bloomIntensity += 1 * Time.deltaTime * 10;
        }
        else
        {
            if (GameManager.Instance.pcamera.GetComponent<UnityStandardAssets.ImageEffects.BloomAndFlares>().bloomIntensity >= 0.3f)
                GameManager.Instance.pcamera.GetComponent<UnityStandardAssets.ImageEffects.BloomAndFlares>().bloomIntensity -= 1 * Time.deltaTime * 20;
        }
    }
    void Update()
    {
        if (freeFallATivo)
        {
            emTransicao = true;
            modo = 2;
            freeFallATivo = false;

        }

        timertimer -= Time.deltaTime;

        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        if (emTransicao)
        {
            var speedBolhas = GameObject.Find("SpeedBolhas").GetComponent<ParticleSystem>().emission;
            speedBolhas.enabled = false;
            skipCounter = 0;
            var hemaciasforceoverlifetime = GameObject.Find("Hemacias").GetComponent<ParticleSystem>().forceOverLifetime;
            hemaciasforceoverlifetime.enabled = true;
            hemaciasforceoverlifetime.y = -2;

            var bolhasforceoverlifetime = GameObject.Find("Bolhas").GetComponent<ParticleSystem>().forceOverLifetime;
            bolhasforceoverlifetime.enabled = true;
            bolhasforceoverlifetime.y = -2;

            CameraMotor.deadzone = false;
            MapMotor.Instance.speed = MapMotor.Instance.speedInicialDoMapMotor;
            cameraMan.GetComponent<Animator>().SetBool("zoom", true);

            if (modo == 2)
            {
                var bolhasnoise = GameObject.Find("Bolhas").GetComponent<ParticleSystem>().noise;
                bolhasnoise.enabled = true;

                if (timertimer < 0)
                {
                    int inverte = Random.Range(0, 2) == 0 ? -1 : 1;
                    posx = Random.Range(-1.9f, 1.9f);
                    posy = (Mathf.Sqrt(Mathf.Pow(1.9f, 2) - Mathf.Pow(posx, 2))) * inverte;
                    timertimer = 0.1f;
                    numeroDeVezes++;

                }
                player.transform.localPosition = Vector2.Lerp(player.transform.localPosition, new Vector2(posx, posy), Time.deltaTime * 4f);

                guia.GetComponent<Accelerometer>().enabled = false;
                player.GetComponent<PlayerBehaviour>().enabled = false;
                player.GetComponent<FreeFallBH>().enabled = false;
                cameraMan.GetComponent<CameraMotor>().enabled = false;


                if (numeroDeVezes > 40)
                {
                    emTransicao = false;
                }
            }

        }
        else
        {
            if (modo == 1)
                Mode_Normal();
            else if (modo == 2)
                Mode_FreeFall();
            else if (modo == 3)
            {
                Mode_Febre();
            }
            else if (modo == 4)
            {
                StartCoroutine(WaitShake());
            }
        }

        skipCounter -= Time.deltaTime;


        if (modo == 3 && modoStore == 3)
            barreirademonocitosGO.SetActive(true);
        else
            barreirademonocitosGO.SetActive(false);
    }

    void Mode_Normal()
    {

        playerMesh.transform.Rotate(12f, 0, 0);

        //AJEITA A POSIÇÃO DO MODO NORMAL

        GameObject.Find("TrilhaBolhas_Hemacias").GetComponent<Transform>().transform.localPosition = Vector3.Lerp(GameObject.Find("TrilhaBolhas_Hemacias").GetComponent<Transform>().transform.localPosition, new Vector3(0, 0, 15), 0.01f);
        if (!pos_fix)
        {
            guia.transform.position = new Vector3(guia.transform.position.x, guia.transform.position.y, guia.transform.position.z + 50f);

            player.transform.localPosition = Vector3.zero;
            if (player.transform.localPosition == Vector3.zero)
                pos_fix = true;
        }

        //ATIVA/DESATIVA SCRIPTS
        guia.GetComponent<Accelerometer>().enabled = true;
        player.GetComponent<PlayerBehaviour>().enabled = true;
        player.GetComponent<FreeFallBH>().enabled = false;
        cameraMan.GetComponent<CameraMotor>().enabled = true;


        //MOVIMENTAÇÃO NORMAL DO PLAYER (DIREITA = DREITA, ESQUERDA = ESQUERDA)
        FreeFallBH.flip = 1f;


        //DESTROI A HINGE E SETA OS CONNSTRAINTS
        if (temhinge)
        {
            DestroyHingeJoint();
            temhinge = false;
        }

        LevelManager.Instace.somenteTransicoes = false;



        if (modoStore != modo)
        {
            freefall_DNA.SetActive(false);
            freefall_DNA_2.SetActive(false);
            playerMesh.transform.localRotation = playerMeshRotationZero;
            freeFallATivo = false;
            corDaFebre.enabled = false;
            luzDoTubo.transform.localPosition = Vector3.zero;
            GameObject.Find("Hemacias").GetComponent<Transform>().transform.localPosition = new Vector3(0, 0, 10);


            var hemaciasforceoverlifetime = GameObject.Find("Hemacias").GetComponent<ParticleSystem>().forceOverLifetime;
            hemaciasforceoverlifetime.enabled = false;

            var bolhasforceoverlifetime = GameObject.Find("Bolhas").GetComponent<ParticleSystem>().forceOverLifetime;
            bolhasforceoverlifetime.enabled = false;
            skipCounter = 0f;
            modoStore = modo;
        }

        ajusteDaBarreiraDeMonocitos = false;

        numeroDeVezes = 0;

        if (skipCounter < -10)
            cameraMan.GetComponent<Animator>().SetBool("zoom", false);

    }

    void Mode_FreeFall()
    {
        playerMesh.transform.localRotation = Quaternion.LookRotation(playerMesh.transform.localPosition - cameraMan.transform.position);

        if (skipCounter < -15 && skipCounter > -20)
        {
            cameraMan.GetComponent<Animator>().SetBool("zoom", false);
        }


        luzDoTubo.transform.localPosition = new Vector3(0, 0, 1);


        //EVITA O BUG DO PLAYER PUXAR A GUIA
        guia.transform.position = new Vector3(guiaPosZero.x, guiaPosZero.y, guia.transform.position.z);

        //FIX DO MODO NORMAL
        pos_fix = false;


        guia.GetComponent<Accelerometer>().enabled = false;
        player.GetComponent<PlayerBehaviour>().enabled = false;
        if (numeroDeVezes >= 41)
            player.GetComponent<FreeFallBH>().enabled = true;
        cameraMan.GetComponent<CameraMotor>().enabled = false;


        //AJEITA CAMERA E POSIÇÃO
        guia.transform.rotation = Quaternion.Lerp(guia.transform.rotation, guiaRotZero, 0.05f);
        cameraMan.transform.localPosition = new Vector3(cameraManPosZero.x, cameraManPosZero.y, 4 * -2f * cameraManPosZero.z);//JOGA A CAMERA PRA FRENTE

        cameraMan.transform.rotation = Quaternion.LookRotation(guia.transform.position - cameraMan.transform.position);





        //MOVIMENTAÇÃO INVERTIDA DO PLAYER (DIREITA = ESQUERDA, ESQUERDA = DIREITA, PQ A CAMERA TA DO CONTRARIO)
        FreeFallBH.flip = -1f;

        //CRIA HINGE E CONFIGURA O COMPONENTE
        if (!temhinge && numeroDeVezes >= 41)
        {
            CreateHingeJoint();
            temhinge = true;
        }


        LevelManager.Instace.somenteTransicoes = true;


        if (modoStore != modo)
        {
            freefall_DNA.SetActive(true);
            freefall_DNA_2.SetActive(true);
            freefall_DNA.transform.position = new Vector3(guia.transform.position.x, guia.transform.position.y, guia.transform.position.z + 50f);
            freefall_DNA_2.transform.position = new Vector3(guia.transform.position.x, guia.transform.position.y, guia.transform.position.z + 120f);


            numeroDeVezes = 0;
            CameraMotor.deadzone = false;//ANULA A MORTE
            GameManager.Instance.pcamera.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>().intensity = 0.036f;
            MapMotor.Instance.speed = MapMotor.Instance.speedInicialDoMapMotor;

            bateuNaFebre = false;
            GameObject.Find("TrilhaBolhas_Hemacias").GetComponent<Transform>().transform.localPosition = new Vector3(0, 0, -15);
            GameObject.Find("Hemacias").GetComponent<Transform>().transform.localPosition = new Vector3(0, 0, 0);


            var hemaciasforceoverlifetime = GameObject.Find("Hemacias").GetComponent<ParticleSystem>().forceOverLifetime;
            hemaciasforceoverlifetime.enabled = true;
            hemaciasforceoverlifetime.y = -6;

            var bolhasforceoverlifetime = GameObject.Find("Bolhas").GetComponent<ParticleSystem>().forceOverLifetime;
            bolhasforceoverlifetime.enabled = true;
            bolhasforceoverlifetime.y = -6;



            var bolhasnoise = GameObject.Find("Bolhas").GetComponent<ParticleSystem>().noise;
            bolhasnoise.enabled = false;

            var bolhassstartspeed = GameObject.Find("Bolhas").GetComponent<ParticleSystem>().main;
            bolhassstartspeed.startSpeed = 0.4f;

            guia.transform.position = new Vector3(guia.transform.position.x, guia.transform.position.y, guia.transform.position.z + 250f);
            skipCounter = 0f;
            modoStore = modo;

            contadorDeTempoFreeFall = duracaoBaseDoFreeFall;
        }



        if (skipCounter > -2)
        {
            if (skipCounter > 0)
                player.GetComponent<FreeFallBH>().virusspeed = 0f;
            else
                player.GetComponent<FreeFallBH>().virusspeed = virusspeedStore;
        }
        else
        {
            player.GetComponent<FreeFallBH>().virusspeed = virusspeedStore;
        }



        if (skipCounter < 0)
            contadorDeTempoFreeFall -= Time.deltaTime;
        if (contadorDeTempoFreeFall < duracaoBaseDoFreeFall / 2)
        {
            var bolhasstartspeed = GameObject.Find("Bolhas").GetComponent<ParticleSystem>().main;
            bolhasstartspeed.startSpeed = 1;

            GameObject.Find("TrilhaBolhas_Hemacias").GetComponent<Transform>().transform.localPosition = Vector3.Lerp(GameObject.Find("TrilhaBolhas_Hemacias").GetComponent<Transform>().transform.localPosition, new Vector3(0, 0, 15), 0.005f);
            if (Vector3.Distance(GameObject.Find("TrilhaBolhas_Hemacias").GetComponent<Transform>().transform.localPosition, player.transform.position) < 2)
            {
                var hemaciasforceoverlifetime = GameObject.Find("Hemacias").GetComponent<ParticleSystem>().forceOverLifetime;
                hemaciasforceoverlifetime.enabled = false;

                var bolhasforceoverlifetime = GameObject.Find("Bolhas").GetComponent<ParticleSystem>().forceOverLifetime;
                bolhasforceoverlifetime.enabled = false;
            }


            if (contadorDeTempoFreeFall < 0)
                modo = 1;
        }

        if (numeroDeVezes < 40)
        {
            if (timertimer < 0)
            {
                int inverte = Random.Range(0, 2) == 0 ? -1 : 1;
                posx = Random.Range(-1.9f, 1.9f);
                posy = (Mathf.Sqrt(Mathf.Pow(1.9f, 2) - Mathf.Pow(posx, 2))) * inverte;
                timertimer = 0.1f;
                numeroDeVezes++;

            }
            player.transform.localPosition = Vector2.Lerp(player.transform.localPosition, new Vector2(posx, posy), Time.deltaTime * 4f);
        }

        if (contadorDeTempoFreeFall > 7 && contadorDeTempoFreeFall < 34 && Vector3.Distance(guia.transform.position, freefall_DNA.transform.position) > 105)
        {
            freefall_DNA.transform.position = new Vector3(guia.transform.position.x, guia.transform.position.y, guia.transform.position.z + 10f);

        }
        if (contadorDeTempoFreeFall > 7 && contadorDeTempoFreeFall < 27 && Vector3.Distance(guia.transform.position, freefall_DNA_2.transform.position) > 105)
        {
            freefall_DNA_2.transform.position = new Vector3(guia.transform.position.x, guia.transform.position.y, guia.transform.position.z + 10f);

        }
    }



    void Mode_Febre()
    {
        GameManager.Instance.pcamera.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>().intensity = 0.036f;

        playerMesh.transform.localRotation = Quaternion.LookRotation(playerMesh.transform.localPosition - cameraMan.transform.position);

        playerMesh.transform.localRotation = playerMeshRotationZero;


        guia.transform.position = new Vector3(guiaPosZero.x, guiaPosZero.y, guia.transform.position.z);



        guia.GetComponent<Accelerometer>().enabled = false;
        player.GetComponent<PlayerBehaviour>().enabled = false;
        if (!bateuNaFebre)
            player.GetComponent<FreeFallBH>().enabled = true;
        else
        {
            player.GetComponent<FreeFallBH>().enabled = false;
            if (MapMotor.Instance.speed > 0)
                MapMotor.Instance.speed -= 0.1f;
        }

        guia.transform.rotation = Quaternion.Lerp(guia.transform.rotation, guiaRotZero, 0.05f);
        cameraMan.transform.localPosition = Vector3.Lerp(cameraMan.transform.localPosition, new Vector3(cameraManPosZero.x, cameraManPosZero.y, 4f * cameraManPosZero.z), 0.05f);
        cameraMan.transform.localRotation = Quaternion.Lerp(cameraMan.transform.localRotation, cameraManRotZero, 0.05f);

        FreeFallBH.flip = 1f;

        cameraMan.GetComponent<Animator>().SetBool("zoom", false);

        //CRIA HINGE E CONFIGURA O COMPONENTE

        if (!temhinge)
        {
            CreateHingeJoint();
            temhinge = true;
        }



        LevelManager.Instace.somenteTransicoes = true;

        if (modoStore != modo)
        {
            freeFallATivo = false;

            var speedBolhas = GameObject.Find("SpeedBolhas").GetComponent<ParticleSystem>().emission;
            speedBolhas.enabled = false;


            corDaFebre.enabled = true;

            guia.transform.position = new Vector3(guia.transform.position.x, guia.transform.position.y, guia.transform.position.z + 250f);

            luzDoTubo.transform.localPosition = Vector3.zero;
            numeroDeVezes = 0;
            cameraMan.GetComponent<CameraMotor>().enabled = false;

            CameraMotor.deadzone = false;

            GameObject.Find("TrilhaBolhas_Hemacias").GetComponent<Transform>().transform.localPosition = new Vector3(0, 0, 15);
            GameObject.Find("Hemacias").GetComponent<Transform>().transform.localPosition = new Vector3(0, 0, 10);


            var hemaciasforceoverlifetime = GameObject.Find("Hemacias").GetComponent<ParticleSystem>().forceOverLifetime;
            hemaciasforceoverlifetime.enabled = false;

            var bolhasforceoverlifetime = GameObject.Find("Bolhas").GetComponent<ParticleSystem>().forceOverLifetime;
            bolhasforceoverlifetime.enabled = false;
            skipCounter = 5f;
            modoStore = modo;

            contadorDeObstaculosFebre = numeroBaseObstaculosFebre;

            PlayerBehaviour.bulletTime = false;

            ajusteFinalFebre = false;
        }


        if (!ajusteDaBarreiraDeMonocitos)
        {
            monocitos[randInt].SetActive(true);
            if (skipCounter > 0)
                barreiraMonocitos.transform.position = new Vector3(guia.transform.position.x, guia.transform.position.y, guia.transform.position.z + 100f);
            else
                barreiraMonocitos.transform.position = new Vector3(guia.transform.position.x, guia.transform.position.y, guia.transform.position.z + 30f);

            randInt = Random.Range(0, monocitos.Length);
            monocitos[randInt].SetActive(false);

            ajusteDaBarreiraDeMonocitos = true;
        }

        if (contadorDeObstaculosFebre <= 0)
        {
            if (ajusteFinalFebre == false)
            {
                skipCounter = -600f;
                numeroBaseObstaculosFebre += incrementoNumeroDeObstaculosFebre;
                GameManager.Instance.distance += 500f;
                ajusteFinalFebre = true;
            }

            barreiraMonocitos.transform.position = new Vector3(guia.transform.position.x, guia.transform.position.y, guia.transform.position.z - 180f);

            if (skipCounter < -602f)
                modo = 1;
        }
        if (skipCounter < 1.5f && pos_fix)
        {
            pos_fix = false;
        }



        luzDoTubo.transform.localPosition += new Vector3(0, 0, 1) * Time.deltaTime * speedDaLuz;
        if (luzDoTubo.transform.localPosition.z > 50)
            luzDoTubo.transform.localPosition = new Vector3(0, 0, -20);
        else if (luzDoTubo.transform.localPosition.z < -20)
            luzDoTubo.transform.localPosition = new Vector3(0, 0, 50);

        if (skipCounter < -1)
            cameraMan.GetComponent<Animator>().SetBool("fade", false);
    }


    IEnumerator WaitShake()
    {
        playerMesh.transform.Rotate(12f, 0, 0);
        CameraMotor.deadzone = false;
        cameraMan.GetComponent<Animator>().SetBool("fade", true);


        yield return new WaitForSeconds(2.4f);
        modo = 3;
    }




    void DestroyHingeJoint()
    {
        player.GetComponent<Rigidbody>().mass = 1f;

        Destroy(player.GetComponent<HingeJoint>());

        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
    void CreateHingeJoint()
    {
        player.GetComponent<Rigidbody>().mass = 0.1f;

        player.AddComponent<HingeJoint>();
        player.GetComponent<HingeJoint>().connectedBody = guia.GetComponent<Rigidbody>();
        player.GetComponent<HingeJoint>().anchor = Vector3.zero;
        player.GetComponent<HingeJoint>().autoConfigureConnectedAnchor = false;
        player.GetComponent<HingeJoint>().connectedAnchor = Vector3.zero;
        player.GetComponent<HingeJoint>().connectedMassScale = 100;

        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }
}