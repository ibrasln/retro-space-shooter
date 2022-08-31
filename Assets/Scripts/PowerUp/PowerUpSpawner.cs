using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUpSpawner : MonoBehaviour
{

    public static PowerUpSpawner instance;
    public GameObject powerUpText;
    public List<GameObject> powerUps = new();

    float creationCounter;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        creationCounter = 10f;
    }

    private void Update()
    {
        if (GameManager.instance.curState == GameManager.GameState.WAVESTARTED || GameManager.instance.curState == GameManager.GameState.BOSSFIGHT)
        {
            creationCounter -= Time.deltaTime;

            if (creationCounter <= 0)
            {
                CreateRandomPowerUp();
                creationCounter = 15f;
            }
        }
    }

    public void CreateHealthPowerUp()
    {
        Instantiate(powerUps[8], ChooseRandomPos(), Quaternion.identity);
    }

    public void CreateRandomPowerUp()
    {
        int randPowerUpIndex = Random.Range(0, powerUps.Count - 1);
        Instantiate(powerUps[randPowerUpIndex], ChooseRandomPos(), Quaternion.identity);
    }

    Vector2 ChooseRandomPos()
    {
         return new(Random.Range(-8.5f, 8.5f), transform.position.y);
    }

}
