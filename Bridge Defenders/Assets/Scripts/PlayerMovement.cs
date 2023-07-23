using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlaceToMove{InPlace, Lobby, Shop, Fight}

public class PlayerMovement : MonoBehaviour
{
    public PlaceToMove placeToMove;
    [SerializeField]private Transform[] playerMovePoints;
    [SerializeField]private float velocity;
    public bool battle;

    public bool onPlace;
    void Start()
    {
        
    }

    void Update()
    {
        MoveToFight();
        MoveToLobby();
        MoveToShop();
    }

    public void MoveToFight()
    {
        if(placeToMove == PlaceToMove.Fight && transform.position != playerMovePoints[0].position)
        {
            battle = true;
            transform.position = Vector2.MoveTowards(transform.position, playerMovePoints[0].position, velocity * Time.deltaTime);
        }else if(placeToMove == PlaceToMove.Fight)
        {
            onPlace = true;
        }
    }


    public void MoveToLobby()
    {
        if(placeToMove == PlaceToMove.Lobby && transform.position != playerMovePoints[1].position)
        {
            lobby();
        }
    }


    public void MoveToShop()
    {
        if(placeToMove == PlaceToMove.Shop && transform.position != playerMovePoints[2].position)
        {
            shop();
        }
    }
    public void lobby()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerMovePoints[1].position, velocity * Time.deltaTime);
    }

    public void shop()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerMovePoints[2].position, velocity * Time.deltaTime);
    }
}