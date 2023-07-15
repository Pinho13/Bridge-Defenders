using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private Transform[] playerMovePoints;
    [SerializeField]private float velocity;
    public bool[] Movable;
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
        if(Vector2.Distance(transform.position, playerMovePoints[0].position) > 1 && Movable[0])
        {
            battle = true;
            transform.position = Vector3.Lerp(transform.position, playerMovePoints[0].position, velocity * Time.deltaTime);
        }else if(Movable[0])
        {
            Movable[0] = false;
        }else if(Vector2.Distance(transform.position, playerMovePoints[0].position) <= 1)
        {
            onPlace = true;
        }
    }


    public void MoveToLobby()
    {
        if(Vector2.Distance(transform.position, playerMovePoints[1].position) >= 1 && Movable[1])
        {
            lobby();
        }else if(Movable[1])
        {
            Movable[1] = false;
        }
    }


    public void MoveToShop()
    {
        if(Vector2.Distance(transform.position, playerMovePoints[2].position) >= 1 && Movable[2])
        {
            shop();
        }else if(Movable[2])
        {
            Movable[2] = false;
        }
    }
    public void lobby()
    {
        transform.position = Vector3.Lerp(transform.position, playerMovePoints[1].position, velocity * Time.deltaTime);
    }

    public void shop()
    {
        transform.position = Vector3.Lerp(transform.position, playerMovePoints[2].position, velocity * Time.deltaTime);
    }
}