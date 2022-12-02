using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInputs : MonoBehaviour {

    public static MobileInputs Instance { set; get; }


    const float DEADZONE = 10f;

    bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    Vector2 swipeDelta, starToutch;

    #region Get Touch Inputs Values

    public bool Tap { get { return tap; } }

    public Vector2 SwipeDelta { get { return swipeDelta; } }

    public bool SwipeLeft { get { return swipeLeft; } }

    public bool SwipeRight { get { return swipeRight; } }

    public bool SwipeDown { get { return swipeDown; } }

    public bool SwipeUp { get { return swipeUp; } }

    #endregion 


    private void Awake()
    {
        Instance = this;
    }

	void Update () {
        ResetValues();

        //recebe os inputs
        #region Mobile Inputs

        if (Input.touches.Length != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                starToutch = Input.mousePosition;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
                starToutch = swipeDelta = Vector2.zero;
        }
        #endregion

        //calcula a distancia do touch
        swipeDelta = Vector2.zero;
        if (starToutch != Vector2.zero)
        {
            if (Input.touches.Length != 0)
                swipeDelta = Input.touches[0].position - starToutch;
        }

        //aplica uma mask para que o calculo demovimento somente seja feito nos polos do plano cartesiano
        if (swipeDelta.magnitude > DEADZONE)
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                // left right
                if (x < 0)
                    swipeLeft = true;
                else
                    swipeRight = true;
            }
            else
            {
                // up down
                if (y < 0)
                    swipeDown = true;
                else
                    swipeUp = true;
            }

            // se remove o dedo zera os parametros
            starToutch = swipeDelta = Vector2.zero;
        }
    }

    private void ResetValues() // reseta todos os booleans de touch
    {
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;
    }
}
