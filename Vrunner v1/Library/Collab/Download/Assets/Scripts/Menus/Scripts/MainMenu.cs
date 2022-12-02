using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{

    const float cameraRotationVelocity = 5f;

    Transform cameraTransform = null;
    Transform cameraDesireTransform = null;
    
    public string gameScene = null;
    public Text bestScore = null;
    

    //    private bool tutorial = true;
    //    private bool musica = true;
    //    private bool efeitoSonoro = true;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        cameraDesireTransform = cameraTransform;
        if (PlayerPrefs.GetFloat("bestScore") < PlayerPrefs.GetFloat("pontuaçaoFinal"))
        {
            PlayerPrefs.SetFloat("bestScore", PlayerPrefs.GetFloat("pontuaçaoFinal"));
        }
        PlayerPrefs.SetFloat("pontuaçaoFinal", 0f);
        SetText();
    }

    void Update()
    {
        cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraDesireTransform.rotation, cameraRotationVelocity * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.SetFloat("bestScore", 0f);
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void LookTo(Transform lookTo)
    {
        cameraDesireTransform = lookTo;
    }

    public void Teste(int numero)
    {
        PlayerPrefs.SetInt("Player", numero);
        Debug.Log(PlayerPrefs.GetInt("Player"));
    }

    void SetText()
    {
        bestScore.text = "Melhor Pontuaçao: " + PlayerPrefs.GetFloat("bestScore");
    }


        /*
        public void MusciaBool()
        {
            if (musica)
            {
                musica = false;
            }
            else
            {
                musica = true;
            }
        }

        public void EfeitoSonoroBool()
        {
            if (efeitoSonoro)
            {
                efeitoSonoro = false;
            }
            else
            {
                efeitoSonoro = true;
            }
        }

        public void TutorialBool()
        {
            if (tutorial)
            {
                tutorial = false;
            }
            else
            {
                tutorial = true;
            }
        }
        */
}
