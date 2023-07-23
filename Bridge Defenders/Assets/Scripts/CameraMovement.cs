using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraPlaces{Lobby, Battle}

public class CameraMovement : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]private GameObject LobbyUI;
    [SerializeField]private GameObject BattleUI;




    [Header("Camera")]
    [SerializeField]private Transform Lobby;
    [SerializeField]private Transform Battle;
    [SerializeField]private float cameraVelocity;
    public CameraPlaces cameraPlaces;
    

    void Start()
    {
        
    }


    void Update()
    {
        cameraToBattle();
        cameraToLobby();
    }

    void cameraToBattle()
    {
        if(cameraPlaces == CameraPlaces.Battle)
        {
            LobbyUI.SetActive(false);
            transform.position = Vector3.MoveTowards(transform.position, Battle.position, cameraVelocity * Time.deltaTime);
        }
    }

    void cameraToLobby()
    {
        if(cameraPlaces == CameraPlaces.Lobby)
        {
            LobbyUI.SetActive(true);
            transform.position = Vector3.MoveTowards(transform.position, Lobby.position, cameraVelocity * Time.deltaTime);
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
