using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]private GameObject LobbyUI;
    [SerializeField]private GameObject BattleUI;




    [Header("Camera")]
    [SerializeField]private Transform Lobby;
    [SerializeField]private Transform Battle;
    [SerializeField]private float cameraVelocity;
    public bool[] Movable;

    void Start()
    {
        
    }


    void Update()
    {
        cameraToBattle();
        cameraToLobby();
    }

    public void cameraToBattle()
    {
        if(Vector2.Distance(transform.position, Battle.position) > 1.5 && Movable[0])
        {
            LobbyUI.SetActive(false);
            transform.position = Vector3.Lerp(transform.position, Battle.position, cameraVelocity * Time.deltaTime);
        }else if(Movable[0])
        {
            Movable[0] = false;
        }
    }

    public void cameraToLobby()
    {
        if(Vector2.Distance(transform.position, Lobby.position) > 1.5 && Movable[1])
        {
            LobbyUI.SetActive(true);
            transform.position = Vector3.Lerp(transform.position, Lobby.position, cameraVelocity * Time.deltaTime);
        }else if(Movable[1])
        {
            Movable[1] = false;
        }
    }

    public void BattleActive()
    {
        BattleUI.SetActive(true);
    }

    public void BattleDeactive()
    {
        BattleUI.SetActive(false);
    }
}
