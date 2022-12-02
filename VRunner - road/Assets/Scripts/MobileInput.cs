using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour {

    public static MobileInput Instance { set; get; }

    public GameObject mainAccRotation;

    public float tiltSpeed;

    [HideInInspector]
    public int tiltDirection;

    private const float DEADZONE = 10f;

    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private Vector2 swipeDelta, startToutch;

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

    private void Update()
    {
        ResetValues();

        #region Standalone Inputs
        if(Input.GetMouseButtonDown(0))
        {
            tap = true;
            startToutch = Input.mousePosition;
        }
        else if ( Input.GetMouseButtonUp(0))
            startToutch = swipeDelta = Vector2.zero;

        #endregion

        #region Mobile Inputs
        if (Input.touches.Length != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                startToutch = Input.mousePosition;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
                startToutch = swipeDelta = Vector2.zero;
        }
        #endregion


        //calcula a distancia do touch
        swipeDelta = Vector2.zero;
        if (startToutch != Vector2.zero)
        {
            /// mobile
            if (Input.touches.Length != 0)
                swipeDelta = Input.touches[0].position - startToutch;

            /// standalone
            else if (Input.GetMouseButtonDown(0))
                swipeDelta = (Vector2)Input.mousePosition - startToutch;
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
            startToutch = swipeDelta = Vector2.zero;
        }

        if(GameManager.Instance.isGameStarted && !PlayerBehaviour.Instance.isDeath)
            mainAccRotation.transform.Rotate(0, 0, tiltDirection * Input.acceleration.x * tiltSpeed * Time.deltaTime);
    }

    private void ResetValues() // reseta todos os booleans de touch
    {
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;
    }
}
