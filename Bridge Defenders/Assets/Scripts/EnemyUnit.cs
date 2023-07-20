using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyUnit : MonoBehaviour
{
    //combat system reference
    CombatSystem combatSystem; 


    [Header("Health")]
    public float MaxHealth;
    [HideInInspector] public float CurrentHealth;


    [Header("Move to Battle")]
    [HideInInspector] public Vector3 PosToMove;
    [HideInInspector] public bool Movable;
    [SerializeField]private float speed;



    [Header("Health Bar")]
    [SerializeField]private GameObject healthbar;
    [SerializeField]private GameObject fullHealthBar;
    [SerializeField]private float yBarSize;
    public Action<EnemyUnit> onDeath;




    [Header("Damage Player")]
    private PlayerUnit Player;
    public float Damage;




    [Header("Render")]
    SpriteRenderer spriteRenderer;



    

    void Start()
    {
        CurrentHealth = MaxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUnit>();
        combatSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatSystem>();
    }


    void Update()
    {
        MoveToPoint();
        healthBar();
        atack();
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
        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            fullHealthBar.SetActive(false);
            onDeath.Invoke(this);
            Destroy(this.gameObject);
        }

        if(CurrentHealth > 0)
        {
            fullHealthBar.SetActive(true);
            healthbar.transform.localScale = new Vector3(((CurrentHealth * yBarSize) / MaxHealth), healthbar.transform.localScale.y, healthbar.transform.localScale.y);
        }
    }

    public void atack()
    {
        if(combatSystem.turn == Turns.EnemyTurn)
        {
            Player.CurrentHealth -= Damage;
            combatSystem.turn = Turns.PlayerTurn;
        }
    }

    void OnMouseOver()
    {
        if(combatSystem.turn == Turns.PlayerTurn)
        {
            spriteRenderer.color = Color.black;
            if(Input.GetMouseButtonDown(0))
            {
                combatSystem.currentEnemy = this;
                combatSystem.battleUI.SetActive(true);
            }
        }
    }


    void OnMouseExit()
    {
        if(combatSystem.turn == Turns.PlayerTurn)
        {
            spriteRenderer.color = Color.grey;
        }
    }
}


