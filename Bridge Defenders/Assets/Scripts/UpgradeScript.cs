using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeScript : MonoBehaviour
{

    public GameObject Panel;
    private bool activePanel;
    [SerializeField]private PlayerMovement pm;


    [Header("Distances")]
    public float Distance;
    public Vector2 mousePos;
    public float maxDistance;
    private SpriteRenderer sr;



    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        distance();
        ActivatePanel();
    }

    void distance()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Distance = Vector2.Distance(transform.position, mousePos);
    }

    void ActivatePanel()
    {
        if(Distance < maxDistance)
        {
            sr.color = Color.gray;
            pm.shop();
            if(Input.GetMouseButtonDown(0))
            {
                Panel.SetActive(true);
                activePanel = true;
            }
        }else if(!activePanel && !pm.battle)
        {
            sr.color = Color.white;
            pm.lobby();
        }
    }

    public void DeactivatePanel()
    {
        Panel.SetActive(false);
        activePanel = false;
    }
}
