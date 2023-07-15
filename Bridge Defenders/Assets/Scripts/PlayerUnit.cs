using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnit : MonoBehaviour
{
    public float MaxHealth;
    public float CurrentHealth;
    public List<PlayerAttacks> playerAtacks;
    [SerializeField]private StateController SC;
    [SerializeField]private EnemyWaveManager EWM;

    public Image fillImage;
    [SerializeField]private Slider slider;

    void Start()
    {

    }


    void Update()
    {
        HealthCap();
        healthBar();
    }
    void noHealth()
    {
        if(CurrentHealth <= 0)
        {
            SC.state = States.Lost;
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
