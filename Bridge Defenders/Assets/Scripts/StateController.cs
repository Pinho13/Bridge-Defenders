using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum States { Lobby, Backing, Start, Setup, PlayerTurn, EnemyTurn, Won, Lost}

public class StateController : MonoBehaviour
{
    public States state;



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
    public GameObject Camera;
    public Transform Lobby;
    public Transform Battle;
    public float cameraVelocity;



    [Header("UI")]
    public GameObject lobbyUI;
    public GameObject battleUI;
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
    public List<EnemiePoints> places;
    public static List<GameObject> SpawnedEnemies = new List<GameObject>();
    private int numberOfEnemiesToSpawn;
    public float enemySpeed;
    private int randomNumber;





    void Start()
    {
        state = States.Lobby;
        pu = Player.GetComponent<PlayerUnit>();
        PlayerPos = Player.GetComponent<Transform>();
    }


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
        NumberOfEnemies();
        SpawningEnemies();
    }

    public void startGame()
    {
        state = States.Start;
    }

    void starting()
    {
        if(state == States.Start)
        {
            lobbyUI.SetActive(false);
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, Battle.position, cameraVelocity * Time.deltaTime);
            PlayerPos.position = Vector3.Lerp(PlayerPos.position, Fight.position, cameraVelocity * Time.deltaTime);
        }
        if(state == States.Start && Camera.transform.position.x >= Battle.position.x-1.5)
        {
            state = States.Setup;
        }
    }

    void won()
    {
        if(state == States.Won)
        {
            AddWave();
            endBattleUI.SetActive(true);
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
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, Lobby.position, cameraVelocity * Time.deltaTime);
            PlayerToLobby();
            if(Camera.transform.position.x <= 0)
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
            endBattleUI.SetActive(true);
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
        if(state == States.Setup)
        {
            if(wave != 16 || wave != 32 || wave != 48)
            {
                if(wave == 0 && numberOfEnemiesToSpawn > 0)
                {
                GameObject enemy = Instantiate(EasyEnemies[0], Spawner);
                randomNumber = Random.Range(0, places.Count);
                while(places[randomNumber].Occupied)
                {
                    randomNumber = Random.Range(0, places.Count);
                }
                enemy.GetComponent<Transform>().position = Vector3.Lerp(enemy.GetComponent<Transform>().position, places[randomNumber].point.position, enemySpeed * Time.deltaTime);
                places[randomNumber].Occupied = true;
                if(Vector2.Distance(enemy.GetComponent<Transform>().position, places[randomNumber].point.position) > 1)
                {
                    enemy.GetComponent<Transform>().position = Vector3.Lerp(enemy.GetComponent<Transform>().position, places[randomNumber].point.position, enemySpeed * Time.deltaTime);
                }
                SpawnedEnemies.Add(enemy);
                numberOfEnemiesToSpawn -= 1;
            }else if(wave <= 3 && numberOfEnemiesToSpawn > 0)
            {
                GameObject enemy = Instantiate(EasyEnemies[Random.Range(0, EasyEnemies.Length)], Spawner);
                randomNumber = Random.Range(0, places.Count);
                while(places[randomNumber].Occupied)
                {
                    randomNumber = Random.Range(0, places.Count);
                }
                enemy.GetComponent<Transform>().position = Vector3.Lerp(enemy.GetComponent<Transform>().position, places[randomNumber].point.position, enemySpeed * Time.deltaTime);
                places[randomNumber].Occupied = true;
                if(Vector2.Distance(enemy.GetComponent<Transform>().position, places[randomNumber].point.position) > 1)
                {
                    enemy.GetComponent<Transform>().position = Vector3.Lerp(enemy.GetComponent<Transform>().position, places[randomNumber].point.position, enemySpeed * Time.deltaTime);
                }
                SpawnedEnemies.Add(enemy);
                numberOfEnemiesToSpawn -= 1;
            }else if(wave <= 9 && numberOfEnemiesToSpawn > 0)
            {
                GameObject enemy = Instantiate(MediumEnemies[Random.Range(0, MediumEnemies.Length)], Spawner);
                randomNumber = Random.Range(0, places.Count);
                while(places[randomNumber].Occupied)
                {
                    randomNumber = Random.Range(0, places.Count);
                }
                enemy.GetComponent<Transform>().position = Vector3.Lerp(enemy.GetComponent<Transform>().position, places[randomNumber].point.position, enemySpeed * Time.deltaTime);
                places[randomNumber].Occupied = true;
                if(Vector2.Distance(enemy.GetComponent<Transform>().position, places[randomNumber].point.position) > 1)
                {
                    enemy.GetComponent<Transform>().position = Vector3.Lerp(enemy.GetComponent<Transform>().position, places[randomNumber].point.position, enemySpeed * Time.deltaTime);
                }
                SpawnedEnemies.Add(enemy);
                numberOfEnemiesToSpawn -= 1;
            }else if(wave >= 9 && numberOfEnemiesToSpawn > 0)
            {
                GameObject enemy = Instantiate(HardEnemies[Random.Range(0, HardEnemies.Length)], Spawner);
                randomNumber = Random.Range(0, places.Count);
                while(places[randomNumber].Occupied)
                {
                    randomNumber = Random.Range(0, places.Count);
                }
                enemy.GetComponent<Transform>().position = Vector3.Lerp(enemy.GetComponent<Transform>().position, places[randomNumber].point.position, enemySpeed * Time.deltaTime);
                places[randomNumber].Occupied = true;
                if(Vector2.Distance(enemy.GetComponent<Transform>().position, places[randomNumber].point.position) > 1)
                {
                    enemy.GetComponent<Transform>().position = Vector3.Lerp(enemy.GetComponent<Transform>().position, places[randomNumber].point.position, enemySpeed * Time.deltaTime);
                }
                SpawnedEnemies.Add(enemy);
                numberOfEnemiesToSpawn -= 1;
            }
            }else if(numberOfEnemiesToSpawn > 0)
            {
                GameObject enemy = Instantiate(BossEnemies[Random.Range(0, BossEnemies.Length)], Spawner);
                enemy.GetComponent<Transform>().position = Vector3.Lerp(enemy.GetComponent<Transform>().position, BossPoint.position, enemySpeed * Time.deltaTime);
                if(Vector2.Distance(enemy.GetComponent<Transform>().position, places[randomNumber].point.position) > 1)
                {
                    enemy.GetComponent<Transform>().position = Vector3.Lerp(enemy.GetComponent<Transform>().position, places[randomNumber].point.position, enemySpeed * Time.deltaTime);
                }
                SpawnedEnemies.Add(enemy);
                numberOfEnemiesToSpawn -= 1;
            }
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

}

[System.Serializable]
public class EnemiePoints
{
    public Transform point;
    public bool Occupied;
}
