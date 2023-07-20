using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum States { Lobby, Backing, Start, Setup, Waiting, PlayerTurn, EnemyTurn, Won, Lost}

public class StateController : MonoBehaviour
{
    public States state;

    [Header("Enemy Settings")]
    [SerializeField] int enemyIncrease;
    [SerializeField] int maxEnemies;
    [SerializeField] Enemies[] enemies;
    Dictionary<EnemyDifficulty,Enemies> enemyDic;


    [Header("Wave")]
    public bool normalDifficulty;
    public static int wave = 0;
    public static bool waveCounted;
    public int waveCheckpoint;





    [Header("Player")]
    public GameObject Player;
    private PlayerUnit pu;
    private Transform PlayerPos;
    public Transform Fight;
    public Transform PLobby;
    public Transform Shop;




    [Header("Camera")]
    public GameObject _camera;
    public Transform Lobby;
    public Transform Battle;
    public float cameraVelocity;



    [Header("UI")]
    public GameObject lobbyUI;
    public GameObject battleUI;
    public GameObject FightUI;
    public GameObject endBattleUI;
    public GameObject DifficultyChanger;
    public TMP_Text waveText;



    [Header("Enemy Spawns")]
    public Transform Spawner;
    public Transform BossPoint;
    public GameObject[] EasyEnemies;
    public GameObject[] MediumEnemies;
    public GameObject[] HardEnemies;
    public GameObject[] BossEnemies;
    public List<GameObject> SpawnedEnemies = new List<GameObject>();
    public int numberOfEnemiesToSpawn;
    private int randomNumber;
    public int numberOfDeadEnemies;




    [Header("Player Turn")]
    [SerializeField]private TMP_Text turnText;
    [SerializeField]private float Distance;
    [SerializeField]private Vector2 mousePos;
    public GameObject enemySelected;




void Start()=>Init();

    void Init()
    {
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
    //void Start()
    //{
        //state = States.Lobby;
        //pu = Player.GetComponent<PlayerUnit>();
        //PlayerPos = Player.GetComponent<Transform>();
    //}


    void Update()
    {
        starting();
        won();
        lobby();
        waveTextChange();
        lostWhen();
        checkpoints();
        lost();
        changeDifficulty();
        playerTurn();
        distance();
        enemyTurn();
        combat();
    }

    public void startGame()
    {
        state = States.Start;
    }

    void starting()
    {
        if(state == States.Start)
        {
            waveCounted = false;
            lobbyUI.SetActive(false);
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, Battle.position, cameraVelocity * Time.deltaTime);
            PlayerPos.position = Vector3.Lerp(PlayerPos.position, Fight.position, cameraVelocity * Time.deltaTime);
            NumberOfEnemies();
        }
        if(state == States.Start && _camera.transform.position.x >= Battle.position.x-1.5)
        {
            state = States.Setup;
        }
    }

    void won()
    {
        if(state == States.Won)
        {
            battleUI.SetActive(false);
            if(wave % 3 == 0 && wave != 0)
            {
                AddWave();
                endBattleUI.SetActive(true);
            }else
            {
                AddWave();
                state = States.Start;
            }
        }
    }

    public void backButton()
    {
        state = States.Backing;
        waveCounted = false;
    }

    void lobby()
    {
        if(state == States.Backing)
        {
            endBattleUI.SetActive(false);
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, Lobby.position, cameraVelocity * Time.deltaTime);
            PlayerToLobby();
            if(_camera.transform.position.x <= 0)
            {
                state = States.Lobby;
                lobbyUI.SetActive(true);
            }
        }
    }

    void waveTextChange()
    {
        waveText.text = "Wave: " + wave;
    }

    public static void AddWave()
    {
        if(waveCounted == false)
        {
            wave += 1;
            waveCounted = true;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }


    void lostWhen()
    {
        if(pu.CurrentHealth <= 0 && state == States.EnemyTurn)
        {
            state = States.Lost;
        }
    }

    void checkpoints()
    {
        if(wave % 3 == 0 && normalDifficulty)
        {
            waveCheckpoint = wave;
        }
    }

    void lost()
    {
        if(state == States.Lost)
        {
            endBattleUI.SetActive(false);
            if(normalDifficulty)
            {
                wave = waveCheckpoint;
            }else
            {
                wave = 0;
            }
        }
    }

    public void PLayerToShop()
    {
        if(Vector2.Distance(PlayerPos.position, Shop.position) >= 1)
        {
            PlayerPos.position = Vector3.Lerp(PlayerPos.position, Shop.position, cameraVelocity * Time.deltaTime);
        }
    }

    public void PlayerToLobby()
    {
        if(Vector2.Distance(PlayerPos.position, PLobby.position) >= 1)
        {
            PlayerPos.position = Vector3.Lerp(PlayerPos.position, PLobby.position, cameraVelocity * Time.deltaTime);
        }
    }

    void changeDifficulty()
    {
        if(wave == 0)
        {
            DifficultyChanger.SetActive(true);
        }else
        {
            DifficultyChanger.SetActive(false);
        }
    }

    public void Difficulty(int index)
    {
        switch(index)
        {
            case 0: normalDifficulty = true; break;
            case 1: normalDifficulty = false; break;
        }
    }

    public void Reset()
    {
        wave = 0;
    }

    void SpawningEnemies()
    {
        while (numberOfEnemiesToSpawn > 0)
        {
            if(wave == 0)
            {
                GameObject enemy = Instantiate(EasyEnemies[0], Spawner.position, Quaternion.identity);
                SpawnedEnemies.Add(enemy);
            }
            if(wave > 0 && wave <= 3)
            {
                GameObject enemy = Instantiate(EasyEnemies[Random.Range(0, EasyEnemies.Length)], Spawner.position, Quaternion.identity);
                SpawnedEnemies.Add(enemy);
            }
            if(wave > 3 && wave <= 9)
            {
                GameObject enemy = Instantiate(MediumEnemies[Random.Range(0, MediumEnemies.Length)], Spawner.position, Quaternion.identity);
                SpawnedEnemies.Add(enemy);
            }
            if(wave > 15 && wave % 16 != 0)
            {
                GameObject enemy = Instantiate(HardEnemies[Random.Range(0, HardEnemies.Length)], Spawner.position, Quaternion.identity);
                SpawnedEnemies.Add(enemy);
            }
            if(wave % 16 == 0 && wave != 0)
            {
                GameObject enemy = Instantiate(BossEnemies[Random.Range(0, BossEnemies.Length)], Spawner.position, Quaternion.identity);
                SpawnedEnemies.Add(enemy);
            }
            numberOfEnemiesToSpawn -= 1;
        }
    }

    void NumberOfEnemies()
    {
        switch(wave)
        {
            case 0:
            numberOfEnemiesToSpawn = 1;
            break;
            case 1:
            numberOfEnemiesToSpawn = 2;
            break;
            case 2:
            numberOfEnemiesToSpawn = 3;
            break;
            case 3:
            numberOfEnemiesToSpawn = 4;
            break;
            case 5:
            numberOfEnemiesToSpawn = 5;
            break;
        }
        if(wave > 5 && wave % 16 != 0)
        {
            numberOfEnemiesToSpawn = 5;
        }else if(wave % 16 == 0)
        {
            numberOfEnemiesToSpawn = 1;
        }
    }

    void playerTurn()
    {
        if(state == States.PlayerTurn)
        {
            foreach(GameObject enemy in SpawnedEnemies)
            {
                
            }
            turnText.text = "Your Turn";
            foreach(GameObject enemy in SpawnedEnemies)
            {
                Distance = Vector2.Distance(enemy.transform.position, mousePos);
                if(Distance < 1)
                {
                    enemy.GetComponent<SpriteRenderer>().color = Color.blue;
                    if(Input.GetMouseButtonDown(0))
                    {
                        enemySelected = enemy;
                        battleUI.SetActive(true);
                    }
                }else
                {
                    enemy.GetComponent<SpriteRenderer>().color = Color.gray;
                }
            }
        }
    }
    void distance()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void enemyTurn()
    {
        if(state == States.EnemyTurn)
        {
            battleUI.SetActive(false);
            turnText.text = "Enemy Turn";
            foreach(GameObject enemy in SpawnedEnemies)
            {
                enemy.GetComponent<EnemyUnit>().atack();
            }

        }else if(state != States.PlayerTurn)
        {
            turnText.text = "";
        }
    }

    void combat()
    {
        if(state == States.PlayerTurn || state == States.EnemyTurn)
        {
            if(SpawnedEnemies.Count == 0 && Player.GetComponent<PlayerUnit>().CurrentHealth > 0)
            {
                state = States.Won;
            }
        }
    }

}


