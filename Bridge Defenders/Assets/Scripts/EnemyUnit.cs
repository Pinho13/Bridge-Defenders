using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    public int MaxHealth;
    public int CurrentHealth;

    public StateController SC;
    public Vector3 PosToMove;
    public bool Movable;
    [SerializeField]private float speed;
    private Canvas canvas;
    [SerializeField]private GameObject healthbar;


    

    void Start()
    {

    }


    void Update()
    {
        MoveToPoint();
    }

    void MoveToPoint()
    {
        if(Movable)
        {
            transform.position = Vector2.MoveTowards(transform.position, PosToMove, speed * Time.deltaTime);
            if(transform.position == PosToMove)
            {
                Movable = false;
            }
        }
    }

    void healthBar()
    {
        healthbar.transform.localScale = new Vector3(healthbar.transform.localScale.x, (CurrentHealth * 0.15f) / MaxHealth, healthbar.transform.localScale.y);
    }
}
