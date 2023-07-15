using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemies
{
    public EnemyDifficulty Difficulty;
    [SerializeField] EnemyUnit[] enemies;

    public EnemyUnit GetEnemy()
    {
        if (enemies == null || enemies.Length == 0) return null;
        return enemies[Random.Range(0, enemies.Length)];
    }
}
public enum EnemyDifficulty
{
    Easy,
    Medium,
    Hard
}
