using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public enum GameState
    {
        GAMESTARTING, // Ship is flying
        WAITING, // Cooldown between WAVEs
        SPAWNING, 
        WAVESTARTED,
        BOSSFIGHT,
        GAME_OVER
    }

    [System.Serializable]
    public class Wave
    {
        public string name;
        public int bigShip1Count, bigShip2Count, smallShipCount, teleportableShipCount;
        public bool isBossCreated;        
    }

    public Transform bigShip1, bigShip2, smallShip1, smallShip2, teleportableShip, boss;

    public Wave[] waves;
    private int nextWave;
    public float timeBetweenWaves = 6;
    public float waveCountdown;

    public float searchCountdown = 1f;

    public bool isPlayerBlocked;
    public bool isGameWon;

    public GameState curState;

    [SerializeField] Transform enemies;
    [SerializeField] TextMeshProUGUI waveText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        isPlayerBlocked = true;
        waveCountdown = timeBetweenWaves;
    }

    private void Update()
    {
        if (curState == GameState.WAVESTARTED)
        {
            if (!IsEnemyAlive()) WaveCompleted();
            else return;
        }
        if (curState == GameState.BOSSFIGHT && !IsEnemyAlive())
        {
            GameWon();
            return;
        }

        if (waveCountdown <= 0)
        {
            if (curState == GameState.WAITING || curState == GameState.GAMESTARTING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    void GameWon()
    {
        curState = GameState.GAME_OVER;
        UIController.instance.bossMusic.Stop();
        isPlayerBlocked = true;
        isGameWon = true;
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed!");
        curState = GameState.WAITING;
        PowerUpSpawner.instance.CreateHealthPowerUp();
        waveCountdown = timeBetweenWaves;

        if (nextWave > waves.Length)
        {
            nextWave = 0;
            Debug.Log("All waves completed!");
        }
        else nextWave++;
    }

    bool IsEnemyAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0)
        {
            searchCountdown = 1f;
            if (enemies.transform.childCount == 0) return false;
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave:" + _wave.name);
        curState = GameState.SPAWNING;
        isPlayerBlocked = true;

        if (_wave.isBossCreated)
        {
            UIController.instance.gameMusic.Stop();
            waveText.text = "BOSS FIGHT";
        }
        else waveText.text = "WAVE " + (nextWave + 1).ToString();
        
        waveText.transform.DOScale(1f, 1.25f);
        yield return new WaitForSeconds(2f);
        waveText.transform.DOScale(0f, 1.25f);

        yield return new WaitForSeconds(1.25f);

        for (int i = 0; i < _wave.smallShipCount; i++)
        {
            SpawnEnemy(smallShip1);
        }
        for (int i = 0; i < _wave.smallShipCount; i++)
        {
            SpawnEnemy(smallShip2);
        }

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < _wave.bigShip1Count; i++)
        {
            SpawnEnemy(bigShip1);
        }
        for (int i = 0; i < _wave.bigShip2Count; i++)
        {
            SpawnEnemy(bigShip2);
        }

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < _wave.teleportableShipCount; i++)
        {
            SpawnEnemy(teleportableShip);
        }

        if (_wave.isBossCreated) SpawnEnemy(boss);

        yield return new WaitForSeconds(5f);

        if (_wave.isBossCreated)
        {
            curState = GameState.BOSSFIGHT;
            Debug.Log("Boss has been created");
            _wave.isBossCreated = false;
        }
        else curState = GameState.WAVESTARTED;

        isPlayerBlocked = false;
        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("Enemy has been created!" + _enemy.name);
        Instantiate(_enemy, transform.position, Quaternion.identity, enemies.transform);
    }

}
