using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (MainMenu.tutorial)
        {
            StartCoroutine(WaitTutorialTime("TutoBase"));
        }
        else
            gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if(MainMenu.tutorial)
        {
            if (GameModeBH.modo == 3 || GameModeBH.modo == 2)
            {
                gameObject.GetComponent<Animator>().SetTrigger("TutoFebre");
            }

            if (CameraMotor.deadzone)
            {
                gameObject.GetComponent<Animator>().SetTrigger("TutoFade");
            }
        }
    }

    IEnumerator WaitTutorialTime(string anim)
    {
        gameObject.GetComponent<Animator>().SetTrigger(anim);
        yield return new WaitForSeconds(2);
        gameObject.GetComponent<Animator>().SetTrigger("TutoIdle");
    }
}
