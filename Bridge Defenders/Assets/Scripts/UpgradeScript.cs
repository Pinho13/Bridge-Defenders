using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpgradeScript : MonoBehaviour
{
    public GameObject Panel;
    private bool activePanel;
    [SerializeField]private PlayerMovement pm;
    Animator anim;



    void Start()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        OnShop();
    }

    void OnShop()
    {
        if(activePanel)
        {
            anim.Play("Highlighted");
            pm.placeToMove = PlaceToMove.Shop;
        }
    }

    public void DeactivatePanel()
    {
        anim.Play("shop idle");
        Panel.SetActive(false);
        pm.placeToMove = PlaceToMove.Lobby;
        activePanel = false;
    }

    void OnMouseOver()
    {
        anim.Play("Highlighted");
        pm.placeToMove = PlaceToMove.Shop;
        if(Input.GetMouseButtonDown(0))
        {
            Panel.SetActive(true);
            activePanel = true;
        }
    }


    void OnMouseExit()
    {
        if(!activePanel)
        {
            anim.Play("shop idle");
            pm.placeToMove = PlaceToMove.Lobby;
        }
    }
}
