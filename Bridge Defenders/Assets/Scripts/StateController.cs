using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum States { Lobby, Start, Setup, PlayerTurn, EnemyTurn, Won, Lost}

public class StateController : MonoBehaviour
{
    public States state;


    public static int wave = 0;
    public static bool waveCounted;


    [Header("Camera")]
    public GameObject Camera;
    public Transform Lobby;
    public Transform Battle;
    public float cameraVelocity;



    [Header("UI")]
    public GameObject lobbyUI;
    public GameObject battleUI;
    public GameObject endBattleUI;
    public TMP_Text waveText;




    void Start()
    {
        state = States.Lobby;
    }


    void Update()
    {
        starting();
        won();
        lobby();
        waveTextChange();
    }

    public void startGame()
    {
        state = States.Start;
    }

    void starting()
    {
        if(state == States.Start)
        {
            lobbyUI.SetActive(false);
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, Battle.position, cameraVelocity * Time.deltaTime);
        }
        if(state == States.Start && Camera.transform.position.x >= Battle.position.x-1.5)
        {
            state = States.Won;
        }
    }

    void won()
    {
        if(state == States.Won)
        {
            AddWave();
            endBattleUI.SetActive(true);
        }
    }

    public void backButton()
    {
        state = States.Lobby;
        waveCounted = false;
    }

    void lobby()
    {
        if(state == States.Lobby)
        {
            endBattleUI.SetActive(false);
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, Lobby.position, cameraVelocity * Time.deltaTime);
            lobbyUI.SetActive(true);
        }
    }

    void waveTextChange()
    {
        waveText.text = "Wave: " + wave;
    }

    public static void AddWave()
    {
        if(waveCounted == false)
        {
            wave += 1;
            waveCounted = true;
        }
    }

}
