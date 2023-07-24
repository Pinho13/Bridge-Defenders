using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    GameObject player;
    PlayerUnit playerUnit;
    [HideInInspector] public float CarryingGold;
    [SerializeField] float speed;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerUnit = player.GetComponent<PlayerUnit>();
    }


    void Update()
    {
        GoldDroped();
    }


    void GoldDroped()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        if(transform.position == player.transform.position)
        {
            playerUnit.currency += CarryingGold;
            playerUnit.UpdateCurrencyText();
            Destroy(this.gameObject);
        }
    }
}
