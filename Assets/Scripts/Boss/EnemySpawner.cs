using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{

    public static EnemySpawner instance;

    public List<GameObject> enemiesList = new();
    [SerializeField] GameObject smallShipPrefab, bigShipPrefab, supMachinePrefab;
    [SerializeField] Transform supMachinePosition;
    float speed, movementDelay;
    public bool isReached;
    GameObject bossEnemyPositions, enemies;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        movementDelay = 4.5f;
        speed = 3.5f;
        enemies = GameObject.FindGameObjectWithTag("Enemies");
        bossEnemyPositions = GameObject.FindGameObjectWithTag("BossEnemyPositions");
    }

    private void Update()
    {
        if (BossController.instance.isProtectingFinished) enemiesList.Clear();
        if (!isReached && enemiesList.Count > 0) ShipMovement();

    }

    public IEnumerator CreateShips(int sShipCount, int bShipCount, bool isSupCreated)
    {
        movementDelay = 5f;
        for (int i = 0; i < sShipCount; i++)
        {
            GameObject smallShip = Instantiate(smallShipPrefab, BossController.instance.firstPosOfBoss, Quaternion.identity, enemies.transform);
            enemiesList.Add(smallShip);
            smallShip.SetActive(false);
        }

        for (int i = 0; i < bShipCount; i++)
        {
            GameObject bigShip = Instantiate(bigShipPrefab, BossController.instance.firstPosOfBoss, Quaternion.identity, enemies.transform);
            enemiesList.Add(bigShip);
            bigShip.SetActive(false);
        }
        yield return new WaitForSeconds(1.5f);
        if (isSupCreated)
        {
            Instantiate(supMachinePrefab, BossController.instance.firstPosOfBoss, Quaternion.identity, enemies.transform);            
        }

        foreach (GameObject ship in enemiesList)
        {
            ship.SetActive(true);
        }
        
        yield return new WaitForSeconds(7f);
        isReached = true;
    }

    public void ShipMovement()
    {
        movementDelay -= Time.deltaTime;
        if (movementDelay <= 0)
        {
            for (int i = 0; i < enemiesList.Count; i++)
            {
                enemiesList[i].transform.position = Vector2.MoveTowards(enemiesList[i].transform.position, bossEnemyPositions.transform.GetChild(i).position, speed * Time.deltaTime);
            }
        }
    }

    
}
