using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSelection : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        #region GameMode Selection
        switch (GameManager.Instance.gameMode)
        {
            case GameMode.cardiovascular:
                Cardiovascular();
                break;
            case GameMode.respiratory:
                Respiratory();
                break;
            case GameMode.digestive:
                Digestive();
                break;
            case GameMode.nervous:
                Nervous();
                break;
            case GameMode.fever:
                Fever();
                break;
        }
        #endregion
    }

    public void Cardiovascular()
    {
        MobileInput.Instance.mainAccRotation = GameObject.Find("LvlManager");
        MobileInput.Instance.tiltDirection = -1;
    }

    public void Respiratory()
    {
        MobileInput.Instance.mainAccRotation = GameObject.Find("CameraRotate");
        MobileInput.Instance.tiltDirection = 1;
    }

    public void Digestive()
    {

    }

    public void Nervous()
    {

    }

    public void Fever()
    {

    }
}
