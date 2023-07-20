using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnit : MonoBehaviour
{

    [Header("Player Stats")]
    public float MaxHealth;
    public float CurrentHealth;



    [Header("References")]
    CombatSystem combatSystem;
    [SerializeField]private EnemyWaveManager EWM;






    [Header("HealthBar")]
    public Image fillImage;
    [SerializeField]private Slider slider;




    void Start()
    {
        combatSystem = GetComponent<CombatSystem>();
    }


    void Update()
    {
        HealthCap();
        healthBar();
        noHealth();
    }
    void noHealth()
    {
        if(CurrentHealth <= 0)
        {
            EWM.LoadLobby();
            EWM.currentWaveCount = 0;
        }
    }

    void healthBar()
    {
        if(slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }
        if(slider.value > slider.minValue && !fillImage.enabled)
        {
            fillImage.enabled = true;
        }
        float fillValue = CurrentHealth / MaxHealth;
        slider.value = fillValue;
    }

    void HealthCap()
    {
        if(MaxHealth <= CurrentHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }
}
