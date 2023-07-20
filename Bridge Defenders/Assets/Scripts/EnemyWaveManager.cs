using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EnemyWaveManager : MonoBehaviour
{
    //combat system reference
    [SerializeField] CombatSystem combatSystem;

    [SerializeField]private Transform spawner;
    private int killedEnemies;

    [Header("References")]
    [SerializeField]private PlayerMovement pm;
    [SerializeField]private CameraMovement cm;
    [SerializeField] PlayerUnit playerUnit;


    [Header("Wave Settings")]
    public int currentWaveCount = -1;
    [SerializeField] int maxWaves = 16;
    [SerializeField] TMP_Text waveText;

    [Header("Enemy Settings")]
    [SerializeField] int enemyIncrease = 1;
    [SerializeField] int maxEnemies = 5;
    [SerializeField] Enemies[] enemies;
    int currentEnemyCount;
    EnemyDifficulty currentDifficulty = EnemyDifficulty.Easy;
    Dictionary<EnemyDifficulty,Enemies> enemyDic;
    public List<EnemyUnit> SpawnedEnemies = new List<EnemyUnit>();
    public List<EnemiePoints> places;



    
    IEnumerator Start()
    {
        Init();
        yield return new WaitUntil(()=>pm.onPlace);
        LoadWave();
    }

    void Init()
    {
        waveText.text = "Wave: 0";
        currentWaveCount = -1;
        InitDic();
    }
    void InitDic()
    {
        enemyDic = new();
        if(enemies==null || enemies.Length == 0) return;

        for (int i = 0; i < enemies.Length; i++)
        {
            var enemy = enemies[i];
            if(enemy==null || enemyDic.ContainsKey(enemy.Difficulty)) continue;

            enemyDic.Add(enemy.Difficulty,enemy);
        }
    }
    void OnWaveEnd()
    {
        combatSystem.battleUI.SetActive(false);
        switch(currentWaveCount)
        {
            case 3:
            currentDifficulty = EnemyDifficulty.Medium;
            break;
            case 9:
            currentDifficulty = EnemyDifficulty.Hard;
            break;
        }

        if(currentWaveCount % 3 == 0 && currentWaveCount != 0)
        {
            LoadLobby();
            return;
        } 
        LoadWave();
    }
    public void LoadLobby()
    {
        cm.Movable[1] = true;
        pm.Movable[1] = true;
        pm.battle = false;
        pm.onPlace = false;
        playerUnit.CurrentHealth = playerUnit.MaxHealth;
        combatSystem.battleUI.SetActive(false);
        combatSystem.turn = Turns.OffBattle;
    }
    void LoadBattle()
    {
        cm.Movable[0] = true;
        pm.Movable[0] = true;
        foreach(EnemiePoints place in places)
        {
            place.Occupied = false;
        }
        combatSystem.turn = Turns.PlayerTurn;
    }
    public void LoadWave()
    {
        LoadBattle();
        if(!pm.onPlace) return;
        ++currentWaveCount;
        waveText.text = "Wave: " + currentWaveCount;
        currentEnemyCount = Mathf.Min(currentWaveCount+enemyIncrease,maxEnemies);
        if(!enemyDic.ContainsKey(currentDifficulty)) return;
        killedEnemies = 0;
        for (int i = 0; i < currentEnemyCount; i++)
        {
            var enemy = enemyDic[currentDifficulty].GetEnemy();
            if(enemy==null) continue;

            var spawnedEnemy = Instantiate(enemy, spawner.position, Quaternion.identity);
            EnemyChoosePlace(spawnedEnemy);
            SpawnedEnemies.Add(spawnedEnemy);
            spawnedEnemy.onDeath += OnEnemyDeath;
            //Spawn enemy and do shit with it 
            //Add event to enemy so we can know when it dies and add it as a listener to a method 
        }
    }
    void OnEnemyDeath(EnemyUnit enemy)
    {
        killedEnemies++;
        SpawnedEnemies.Remove(enemy);
        if(killedEnemies == currentEnemyCount)
        {
            OnWaveEnd();
        }
    }

    void EnemyChoosePlace(EnemyUnit eu)
    {
            int randomNumber = Random.Range(0, places.Count);
            while(places[randomNumber].Occupied)
            {
                randomNumber = Random.Range(0, places.Count);
            }
            places[randomNumber].Occupied = true;
            eu.PosToMove = places[randomNumber].point.position;
            eu.Movable = true;
        
    }

    public void ResetWave()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

[System.Serializable]
public class EnemiePoints
{
    public Transform point;
    public bool Occupied;
}
