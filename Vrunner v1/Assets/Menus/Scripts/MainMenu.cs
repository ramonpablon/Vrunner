using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public static bool tutorial = true;
    public Toggle tuto;

    const float cameraRotationVelocity = 5f;

    Transform cameraTransform = null;
    Transform cameraDesireTransform = null;


    public string gameSceneToLoad = null;
    public Text bestScore = null;

    public float[] bloqueioContador = new float[0];
    public Transform premio = null;
    public GameObject[] imagensPlayer = new GameObject[0];
    public GameObject[] skins = new GameObject[0];
    public GameObject[] bloqueio = new GameObject[0];
    public GameObject[] equipado = new GameObject[0];
    public GameObject[] notifiacao = new GameObject[0];


    private int imagemAnterior = 0;
    private float testeBestScore = 0;
    private bool notifiacaoBool = false;

    AudioSource ads;
    public AudioClip adc;

    void Start()
    {
        ads = GetComponent<AudioSource>();

        Time.timeScale = 1;


        cameraTransform = Camera.main.transform;

        if (PlayerPrefs.GetInt("RotacaoCamera") == 1)
        {
            cameraDesireTransform = premio;
            PlayerPrefs.SetInt("RotacaoCamera", 0);
        }
        else
        {
            cameraDesireTransform = cameraTransform;
        }

        cameraTransform.rotation = cameraDesireTransform.rotation;


        for (int i = 0; i < bloqueio.Length; i++)
         {
             if (PlayerPrefs.GetFloat("HighScore") >= bloqueioContador[i])
             {
                 bloqueio[i].SetActive(false);
                 skins[i].SetActive(true);
                 if (PlayerPrefs.GetInt("Skin" + i) == 1)
                 {
                     imagensPlayer[i].SetActive(true);
                     equipado[i].SetActive(true);
                 }
                 else if ((PlayerPrefs.GetInt("Skin" + i) == 2 || PlayerPrefs.GetInt("Skin" + i) == 3) && !notifiacaoBool)
                 {
                     notifiacao[i].SetActive(true);
                     notifiacaoBool = true;
                 }
                 PlayerPrefs.SetInt("Skin" + i, 0);
             }
             else
             {
                 PlayerPrefs.SetInt("Skin" + i, 2);
                 imagensPlayer[i].SetActive(false);
             }
         }

        SetText();


        testeBestScore = PlayerPrefs.GetFloat("HighScore");


        if (PlayerPrefs.GetInt("Tutorial") == 1)
        {
            tutorial = true;
            tuto.isOn = true;
        }
        else
        {
            tutorial = false;
            tuto.isOn = false;
        }
    }

    void Update()
    {
        print(PlayerPrefs.GetFloat("HighScore"));


        if (tutorial)
            PlayerPrefs.SetInt("Tutorial", 1);
        else
            PlayerPrefs.SetInt("Tutorial", 0);

        cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraDesireTransform.rotation, cameraRotationVelocity * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.SetFloat("HighScore", 0f);
            PlayerPrefs.SetInt("Tutorial", 0);
            SetText();
        }

        if (Input.GetKeyDown(KeyCode.L)) // comoprar
        {
            for (int i = 0; i < bloqueio.Length; i++)
            {
                if (PlayerPrefs.GetFloat("HighScore") >= bloqueioContador[i])
                {
                    bloqueio[i].SetActive(false);
                    skins[i].SetActive(true);
                    if (PlayerPrefs.GetInt("Skin" + i) == 1)
                    {
                        imagensPlayer[i].SetActive(true);
                        equipado[i].SetActive(true);
                    }
                    else if (PlayerPrefs.GetInt("Skin" + i) == 2 && !notifiacaoBool)
                    {
                        notifiacao[i].SetActive(true);
                        notifiacaoBool = true;
                    }
                    PlayerPrefs.SetInt("Skin" + i, 0);
                }
                else
                {
                    PlayerPrefs.SetInt("Skin" + i, 2);
                    imagensPlayer[i].SetActive(false);
                }
            }
        }
    }


    public void GanharDinheiro()
    {
        PlayerPrefs.SetFloat("HighScore", PlayerPrefs.GetFloat("HighScore") + 1000);
        SetText();
    }





    public void Play()
    {
        SceneManager.LoadScene(gameSceneToLoad);
    }

    public void LookTo(Transform lookTo)
    {
        cameraDesireTransform = lookTo;
    }

    public void Skins(int numero)
    {
        imagemAnterior = numero;
        if (PlayerPrefs.GetInt("Skin" + imagemAnterior) == 0)
        {
            imagensPlayer[imagemAnterior].SetActive(false);
            imagemAnterior = numero;
            imagensPlayer[numero].SetActive(true);
        }
        else if(PlayerPrefs.GetInt("Skin" + imagemAnterior) == 1)
        {
            imagemAnterior = numero;
            imagensPlayer[numero].SetActive(true);
        }
    }

    public void Equipar()
    {
        if (PlayerPrefs.GetFloat("HighScore") >= 1000)
        {
            PlayerPrefs.SetInt("Skin" + imagemAnterior, 1);
            equipado[imagemAnterior].SetActive(true);
            imagensPlayer[imagemAnterior].SetActive(true);
        }

    }

    public void Desequipar()
    {
        PlayerPrefs.SetInt("Skin" + imagemAnterior, 0);
        equipado[imagemAnterior].SetActive(false);
        imagensPlayer[imagemAnterior].SetActive(false);
    }

    public void Desabilitar(GameObject botao)
    {
        botao.SetActive(false);
        notifiacaoBool = false;
    }

    void SetText()
    {
        bestScore.text = "Melhor Pontuaçao: " + PlayerPrefs.GetFloat("HighScore");
    }

    public void Click()
    {
        ads.PlayOneShot(adc);
    }
    
    public void Tutorial()
    {
        tutorial = !tutorial;
    }
}
