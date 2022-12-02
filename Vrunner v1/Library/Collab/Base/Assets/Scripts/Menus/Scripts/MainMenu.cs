using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    const float cameraRotationVelocity = 5f;

    Transform cameraTransform = null;
    Transform cameraDesireTransform = null;

    public string gameScene = null;

    private bool tutorial = true;
    private bool musica = true;
    private bool efeitoSonoro = true;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraDesireTransform.rotation, cameraRotationVelocity * Time.deltaTime);
    }

    public void Play()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void Credit(Transform creditTransform)
    {
        cameraDesireTransform = creditTransform;
    }

    public void Rewards(Transform rewardsTransform)
    {
        cameraDesireTransform = rewardsTransform;
    }

    public void BackMainMenu(Transform menuTransform)
    {
        cameraDesireTransform = menuTransform;
    }

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
}
