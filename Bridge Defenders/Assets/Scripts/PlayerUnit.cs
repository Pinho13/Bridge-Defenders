using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUnit : MonoBehaviour
{
    [Header("Dropables")]
    public GameObject[] goldIngots;
    public float currency;
    [SerializeField] TMP_Text currencyText;



    [Header("Player Stats")]
    public float MaxHealth;
    public float CurrentHealth;



    [Header("References")]
    CombatSystem combatSystem;
    [SerializeField]private EnemyWaveManager EWM;






    [Header("HealthBar")]
    public Image fillImage;
    [SerializeField]private Slider slider;
    [SerializeField] TMP_Text healthText;




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
            EWM.Lost();
        }
    }

    void healthBar()
    {
        healthText.text = CurrentHealth.ToString("0") + " / " + MaxHealth.ToString("0");
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

    public void UpdateCurrencyText()
    {
        currencyText.text = "Currency: " + currency.ToString("0");
    }
}
