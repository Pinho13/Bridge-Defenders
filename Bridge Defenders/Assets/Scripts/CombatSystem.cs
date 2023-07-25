using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public enum Turns{OffBattle, PlayerTurn, EnemyTurn}

public class CombatSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField]private EnemyWaveManager manager;
    public EnemyUnit currentEnemy;
    [SerializeField] GameObject damageNumbers;

    [Header("Attacks")]
    [SerializeField] List<PlayerAttacks> allAttacks;
    [SerializeField] List<PlayerAttacks> selectedAttacks;
    Dictionary<Button,PlayerAttacks> attackDic;

    [Header("UI Settings")]
    [SerializeField] Button[] attackButtons;
    public GameObject battleUI;


    [Header ("Turns")]
    [HideInInspector] public Turns turn;



    void Awake()=>Init();

    void Init()
    {
        InitAttacks();
    }

    void InitAttacks()
    {
        selectedAttacks=new();
        if(allAttacks==null || allAttacks.Count == 0) return;
        selectedAttacks = allAttacks.Where(x=>x.state == AttackState.Equiped).ToList();
        InitAttackButtons();
    }

    void InitAttackButtons()
    {
        if(attackButtons==null || attackButtons.Length == 0 || selectedAttacks.Count == 0) return;
        attackDic=new();
    
        for (int i = 0; i < selectedAttacks.Count; i++)
        {
            if(i > attackButtons.Length -1) 
            {
                print($"Buttons are less than selected attacks Stopping at index : {i}");
                return;
            }
            var attack = selectedAttacks[i];
            var button = attackButtons[i];
            if (attack == null || button == null) continue;

            attack.AttackButton = button;
            attack.UpdateUI();
            ManageButton(button);

            if(attackDic.ContainsKey(button)) return;
            attackDic.Add(button,attack);
        }
    }

     void ManageButton(Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate{OnButtonClicked(button);});
    }

    void OnButtonClicked(Button button)
    {
        if(!attackDic.ContainsKey(button)) return;

        var attack = attackDic[button];
        if (!attack.AreaDamage && turn == Turns.PlayerTurn)
        {
            var enemyPos = currentEnemy.gameObject.transform.position;
            currentEnemy.CurrentHealth -= attack.AttackDamage;
            GameObject number = Instantiate(damageNumbers, enemyPos + new Vector3(Random.Range(-1,1), Random.Range(-1,1), 0), Quaternion.identity);
            number.transform.GetChild(0).GetComponent<TMP_Text>().text = attack.AttackDamage.ToString();
            Destroy(number, 2);
            battleUI.SetActive(false);
            turn = Turns.EnemyTurn;
        }
        else if(turn == Turns.PlayerTurn)
        {
            foreach (EnemyUnit enemy in manager.SpawnedEnemies)
            {
                enemy.CurrentHealth -= attack.AttackDamage;
                var enemyPos = enemy.gameObject.transform.position;
                GameObject number = Instantiate(damageNumbers, enemyPos + new Vector3(Random.Range(-1,1), Random.Range(-1,1), 0), Quaternion.identity);
                number.transform.GetChild(0).GetComponent<TMP_Text>().text = attack.AttackDamage.ToString();
                Destroy(number, 2);
                battleUI.SetActive(false);
                turn = Turns.EnemyTurn;
            }
        }
    }
}

[System.Serializable]
public class PlayerAttacks
{
    [field: SerializeField] public string atackName { get; private set; }
    [field: SerializeField] public Sprite AttackSprite { get; private set; }
    [field: SerializeField] public float AttackDamage { get; private set; }
    [field: SerializeField] public AttackState state{ get; private set; }
    [field: SerializeField] public bool AreaDamage { get; private set; }

    [field: SerializeField] public Button AttackButton;
    [field: SerializeField] Image buttonImage;

    public void UpdateUI()
    {
        if(AttackButton==null) return;

        if(buttonImage == null) buttonImage = AttackButton.GetComponent<Image>();
        buttonImage.sprite = AttackSprite;
    }
}

public enum AttackState 
{
     Equiped,
     Unequipped 
}
