using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    cardiovascular = 0,
    respiratory = 1,
    digestive = 2,
    nervous = 3,
    fever = 4
};

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; set; }

    public CanvasGroup menuGroup;
    public CanvasGroup gameMenuGroup;

    [HideInInspector]
    public bool isGameStarted = false;

    [Space]
    public GameMode gameMode = GameMode.cardiovascular;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Update()
    {
        if (MobileInput.Instance.Tap && !isGameStarted)
            StartGame();
    }

    private void StartGame()
    {
        isGameStarted = true;
        TurnOffMenu();
        TurnOnGameMenu();
    }

    private void TurnOnMenu()
    {
        menuGroup.alpha = 1;
        menuGroup.interactable = true;
        menuGroup.blocksRaycasts = true;
    }
    private void TurnOffMenu()
    {
        menuGroup.alpha = 0;
        menuGroup.interactable = false;
        menuGroup.blocksRaycasts = false;
    }

    private void TurnOnGameMenu()
    {
        gameMenuGroup.alpha = 1;
        gameMenuGroup.interactable = true;
        gameMenuGroup.blocksRaycasts = true;
    }
    private void TurnOffGameMenu()
    {
        gameMenuGroup.alpha = 0;
        gameMenuGroup.interactable = false;
        gameMenuGroup.blocksRaycasts = false;
    }
}
