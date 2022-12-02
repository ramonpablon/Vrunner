using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoonInZoonOut : MonoBehaviour {

    Camera mainCamera;
    float prevPosDiff, currentPosDiff, zoomModifier;

    Vector2 firstTouchPos, secondTouchPos;

    public float zoomModifierSpeed = .1f;

    public Text text;    

    // Use this for initialization
    void Start () {
        mainCamera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.touchCount == 2)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            firstTouchPos = firstTouch.position - firstTouch.deltaPosition;
            secondTouchPos = secondTouch.position - secondTouch.deltaPosition;

            prevPosDiff = (firstTouchPos - secondTouchPos).magnitude;
            currentPosDiff = (firstTouch.position - secondTouch.position).magnitude;

            zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed;

            if (prevPosDiff > currentPosDiff)
                mainCamera.orthographicSize += zoomModifier;
            if (prevPosDiff < currentPosDiff)
                mainCamera.orthographicSize -= zoomModifier;
        }

        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 2f, 10f);
        text.text = "camera size " + mainCamera.orthographicSize;
    }
}