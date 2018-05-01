
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;

public enum EGameState
{
    MainMenu,
    ShowOptions,
    StartNewRound,
    Targeting,
    ShotsOnGoal,
    CheckResult,
    EndRound,
    EndGame
}

public class GameController : MonoBehaviour {

    [Header("Game options: ")]

    [SerializeField]
    private TargetingMarkerHelper TargetingMarker;

    [SerializeField]
    private BallHelper Ball;

    [SerializeField]
    private PlayerHelper Player;

    [SerializeField]
    private GoalkeeperHelper Goalkeeper;

    [SerializeField]
    private int Attempts = 5;

    private int Goals = 0;

    [SerializeField]
    private float ResetTime = 2;

    [Space]
    [Header("UI options: ")]

    [SerializeField]
    private ScoreIndicatorHelper ScoreIndicator;

    [SerializeField]
    private PopupWindowHelper OptionsPanel;

    [SerializeField]
    private Text PlayerWinText;

    [SerializeField]
    private Text PlayerLooseText;

    [Space]
    [Header("Audio parameters:")]
    [SerializeField]
    private AudioSource Audio;

    [SerializeField]
    private AudioClip GoalSound;

    [SerializeField]
    private AudioClip MissSound;

    [Space]
    [Header("Server parameters:")]

    [SerializeField]
    private String _URI = "ws://localhost:8000";

    private WebSocket _webSocket;

    private EGameState GameState;

    private static GameController instance;

    

	void Start ()
    {

        instance = this;

        Uri uri = new Uri(_URI);

        try
        { 
            _webSocket = new WebSocket(uri);
            print("Web socket created! _webSocket = " + _webSocket);
        }
        catch (Exception e)
        {
            print(e.ToString());
        }

       
        if (_webSocket != null)
            StartCoroutine(_webSocket.Connect());
        else
            print("_webSocket not initialized! ");

        // send data
       // StartCoroutine(SendData());

        // Receive data
        //StartCoroutine(ReceiveData());

        // Close connection
        //StartCoroutine(CloseConnection());

        if (TargetingMarker)
            TargetingMarker.TargetingEnded += TargetRequied;

        ScoreIndicator.Reset();
        //GameState = EGameState.Targeting;

        PlayerWinText.gameObject.SetActive(false);
        PlayerLooseText.gameObject.SetActive(false);
        OptionsPanel.Hide();

        ChangeGameState(EGameState.StartNewRound);
    }
	
    public static GameController GetInstance()
    {
        if (instance == null)
            return null;
        else
            return instance;
    }

	void Update ()
    {
        //if (Input.GetKeyDown(KeyCode.Tab))
        //    ChangeGameState(EGameState.Targeting);
		
	}

    //IEnumerator ResetScene()
    //{
    //    yield return new WaitForSeconds(ResetTime);

    //    //Player.Reset();
    //    //Ball.Reset();

    //}

    IEnumerator StartTargeting()
    {
        yield return new WaitForSeconds(ResetTime);

        Player.Reset();
        Ball.Reset();
        Goalkeeper.Reset();

        TargetingMarker.StartTargeting();
    }

    void ChangeGameState(EGameState newState)
    {
        switch (newState)
        {
            case EGameState.MainMenu:

                break;

            case EGameState.ShowOptions:
                break;

            case EGameState.StartNewRound:
                GameState = EGameState.StartNewRound;
                if (ScoreIndicator.CanOneMoreApproache())
                    //GameState = EGameState.Targeting;
                    ChangeGameState(EGameState.Targeting);
                else
                    //GameState = EGameState.EndGame;
                    ChangeGameState(EGameState.EndGame);
                break;

            case EGameState.Targeting:
                GameState = newState;
                StartCoroutine(StartTargeting());
                break;

            case EGameState.ShotsOnGoal:
                GameState = newState;
                Player.Ball = Ball;
                Player.PunchBall();
                Goalkeeper.Activate();
                ChangeGameState(EGameState.CheckResult);
                break;

            case EGameState.CheckResult:
                GameState = newState;
                break;

            case EGameState.EndRound:
                if (ScoreIndicator.CanOneMoreApproache())
                    ChangeGameState(EGameState.StartNewRound);
                else
                    ChangeGameState(EGameState.EndGame);
                break;

            case EGameState.EndGame:
                GameState = newState;
                if (ScoreIndicator.IsPlayerWin())
                { 
                    PlayerWinText.gameObject.SetActive(true);
                }
                else
                { 
                    PlayerLooseText.gameObject.SetActive(true);
                }
                OptionsPanel.Show();

                break;
            }

    }

    private IEnumerator TimeOutChecker()
    {
        yield return new WaitForSeconds(ResetTime * 1.5f);

        if (GameState == EGameState.CheckResult)
            Miss();
    }

    public void TargetRequied(Vector3 targetPosition)
    {
        
        Ball.Target = targetPosition;
        ChangeGameState(EGameState.ShotsOnGoal);
    }

    public void OpenWindowOptions()
    {
        OptionsPanel.Show();
    }

    public void Goal()
    {

        if (GameState != EGameState.CheckResult)
            return;

        PlaySound(GoalSound);
        ScoreIndicator.AddScore(true);
        StartCoroutine(SendData());
        StartCoroutine(ReceiveData());
        ChangeGameState(EGameState.EndRound);
    }

    public void Miss()
    {
        if (GameState != EGameState.CheckResult)
            return;

        PlaySound(MissSound);
        ScoreIndicator.AddScore(false);
        ChangeGameState(EGameState.EndRound);
    }

    public void PlaySound(AudioClip sound)
    {
        if(Audio)
        {
            Audio.clip = sound;
            Audio.Play();
        }

    }

    public void Restart()
    {
        Application.LoadLevel("Main");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void OpenStartScene()
    {
        Application.LoadLevel("Start");
    }


    IEnumerator SendData()
    {
        yield return new WaitForSeconds(0.1f);

        if (_webSocket != null)
        {
            print("Sending test data...");
            string testData = "Test data";
            _webSocket.SendString(testData);
        }
        else
            yield return 0;
    }

    IEnumerator ReceiveData()
    {
        yield return new WaitForSeconds(5);

        if (_webSocket != null)
        {
            print("Receive test data...");
            string inputMsg = _webSocket.RecvString();
            print(inputMsg);
        }
        else
            yield return 0;
    }

    IEnumerator CloseConnection()
    {
        yield return new WaitForSeconds(15);

        if (_webSocket != null)
        {
            print("Close connection");
            _webSocket.Close();
        }
        else
            yield return 0;
    }


}
