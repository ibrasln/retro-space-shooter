using System.Collections;
using UnityEngine;
using DG.Tweening;
using static BossHealthController;

public class BossController : MonoBehaviour
{

    public enum BossStage
    {
        ENTERING,
        STAGE_1,
        STAGE_2,
        STAGE_3,
        STAGE_4,
        PROTECTING,
        DEAD
    }

    public BossStage curBossStage;

    public static BossController instance;

    [Header("Movement")]
    public float speed;
    public float enteringSpeed;
    [SerializeField] Transform leftTarget, rightTarget;
    private bool isRight;

    [Header("Attack")]
    [SerializeField] Transform missilePositions;
    [SerializeField] GameObject laser1, laser2;
    [SerializeField] Transform bulletPos, laserPos1, laserPos2, bombPos;
    GameObject enemyLasers;
    GameObject player, enemies;
    private float bulletFireCounter, laserFireCounter;
    private bool isMissileFired, isBombFired, isFired;
    float creationTime; // For creating laser sup

    [Header("Protecting")]
    [SerializeField] GameObject shield;
    [SerializeField] GameObject bigShipPrefab, smallShipPrefab, laserSupMachinePrefab, soloSupMachinePrefab;
    GameObject bossBullets;
    private bool isCoroutineStarted, isCameraZoomIn;
    public bool isProtectingStarted, isProtectingFinished;
    [HideInInspector] public Vector2 firstPosOfBoss;

    [Header("Death")]
    [SerializeField] GameObject rocketFire;
    bool isDead;

    Rigidbody2D rb;
    Collider2D col;
    Animator anim;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        transform.position = new(0f, 16f);
        col.enabled = false;
        curBossStage = BossStage.ENTERING;
        bulletFireCounter = 5f;
        laserFireCounter = 10f;
        player = GameObject.FindGameObjectWithTag("Player");
        enemies = GameObject.FindGameObjectWithTag("Enemies");
        bossBullets = GameObject.FindGameObjectWithTag("BossBullets");
        enemyLasers = GameObject.FindGameObjectWithTag("EnemyLasers");
        StartCoroutine(CreateLaserSup());
        InvokeRepeating(nameof(CreateSupMachine), 20f, 20f);
    }

    private void Update()
    {
        CheckStatus();

        if (curBossStage == BossStage.PROTECTING)
        {
            transform.position = Vector2.MoveTowards(transform.position, firstPosOfBoss, speed * Time.deltaTime);
            if (isCameraZoomIn) CameraController.instance.ZoomInCamera(55, transform.position, 2f);
            else CameraController.instance.ZoomOutCamera(2f);
        }
        else
        {
            if (curBossStage != BossStage.ENTERING || curBossStage != BossStage.DEAD) Movement();

            #region Weapon Fire
            if (isMissileFired)
            {
                StartCoroutine(MissileFire());
                isMissileFired = false;
            }
            else if (isBombFired)
            {
                StartCoroutine(BombFire());
                isBombFired = false;
            }
            #endregion
        }
    }

    public void Movement()
    {
        if (isRight) transform.position = Vector2.MoveTowards(transform.position, rightTarget.position, speed * Time.deltaTime);
        else transform.position = Vector2.MoveTowards(transform.position, leftTarget.position, speed * Time.deltaTime);

        if (transform.position.x <= leftTarget.position.x) isRight = true;
        else if (transform.position.x >= rightTarget.position.x) isRight = false;
    }

    public void CheckStatus()
    {
        switch (curBossStage)
        {
            case BossStage.ENTERING:
                if (!isCoroutineStarted)
                {
                    StartCoroutine(EnteringScene());
                    isCoroutineStarted = true;
                }
                transform.position = Vector2.MoveTowards(transform.position, new(0, 3.2f), enteringSpeed * Time.deltaTime);
                break;

            case BossStage.STAGE_1:
                Fire(BulletPools.instance.bossBulletPool_1);
                if (BossHealthController.instance.currentHealth <= PercentageOfHealth(85) && !isFired)
                {
                    isFired = true;
                    isBombFired = true;
                }
                if (BossHealthController.instance.currentHealth <= PercentageOfHealth(75)) StartNextStage();
                break;

            case BossStage.STAGE_2:
                Fire(BulletPools.instance.bossBulletPool_2);
                if (BossHealthController.instance.currentHealth <= PercentageOfHealth(65) && !isFired)
                {
                    isFired = true;
                    isMissileFired = true;
                }
                if (BossHealthController.instance.currentHealth <= PercentageOfHealth(50)) StartNextStage();
                break;

            case BossStage.STAGE_3:
                Fire(BulletPools.instance.bossBulletPool_3);
                if (BossHealthController.instance.currentHealth <= PercentageOfHealth(35) && !isFired)
                {
                    isFired = true;
                    isBombFired = true;
                }
                if (BossHealthController.instance.currentHealth <= PercentageOfHealth(25)) StartNextStage();
                break;

            case BossStage.STAGE_4:
                Fire(BulletPools.instance.bossBulletPool_4);
                if (BossHealthController.instance.currentHealth <= PercentageOfHealth(10) && !isFired)
                {
                    isFired = true;
                    isMissileFired = true;
                }
                if (BossHealthController.instance.isBossDead) StartNextStage();
                break;

            case BossStage.PROTECTING:
                if (!isCoroutineStarted)
                {
                    StartCoroutine(StartProtecting());
                    isCoroutineStarted = true;
                }
                else if (enemies.transform.childCount == 1)
                {
                    StartCoroutine(FinishProtecting());
                    StartNextStage();
                }
                break;

            case BossStage.DEAD:
                if (!isDead)
                {
                    Destroy(enemyLasers);
                    Destroy(laserPos1);
                    Destroy(laserPos2);
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    StopAllCoroutines();
                    StartCoroutine(Die());
                    isDead = true;
                }
                break;
        }
    }

    IEnumerator EnteringScene()
    {
        speed = 0;
        SoundEffectController.instance.SoundEffect(5);
        UIController.instance.bossMusic.PlayDelayed(1.7f);
        yield return new WaitForSeconds(.1f);
        StartCoroutine(CameraController.instance.ShakeCamera(1f, .75f));
        yield return new WaitForSeconds(5.5f);
        UIController.instance.bossHealthBar.GetComponent<CanvasGroup>().DOFade(1f, 1.5f);
        yield return new WaitForSeconds(5f);
        firstPosOfBoss = transform.position;
        StartNextStage();

    }

    public float PercentageOfHealth(int percentage) => BossHealthController.instance.maxHealth * percentage / 100;
    
    IEnumerator StartProtecting()
    {
        PlayerController.instance.col.enabled = false;
        col.enabled = false;
        yield return new WaitForSeconds(1.5f);
        isProtectingStarted = true;
        Debug.Log("StartProtecting");
        StartCoroutine(CameraController.instance.ShakeCamera(1f, .2f));
        yield return new WaitForSeconds(1.5f);
        isCameraZoomIn = true;
        yield return new WaitForSeconds(2f);
        shield.SetActive(true);
        SoundEffectController.instance.SoundEffect(8);
        isCameraZoomIn = false;
        yield return new WaitForSeconds(1f);
        bulletFireCounter = 3f;
        laserFireCounter = 5f;
        yield return new WaitForSeconds(1.5f);
        GameManager.instance.isPlayerBlocked = false;
        PlayerController.instance.col.enabled = true;
    }
    IEnumerator FinishProtecting()
    {
        Debug.Log("FinishProtecting");
        speed = 1.4f;
        PlayerController.instance.col.enabled = false;
        isProtectingFinished = true;
        GameManager.instance.isPlayerBlocked = true;
        StartCoroutine(CameraController.instance.ShakeCamera(1f, .2f));
        SoundEffectController.instance.soundEffects[8].Stop();
        shield.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        EnemySpawner.instance.isReached = false;
        GameManager.instance.isPlayerBlocked = false;
        isProtectingFinished = false;
        PlayerController.instance.col.enabled = true;
        col.enabled = true;
    }

    IEnumerator Die()
    {
        PlayerController.instance.col.enabled = false;
        GameManager.instance.isPlayerBlocked = true;
        StartCoroutine(CameraController.instance.ShakeCamera(1f, .7f));
        yield return new WaitForSeconds(1.5f);
        rocketFire.SetActive(false);
        anim.SetTrigger("death");
        col.enabled = false;
        yield return new WaitForSeconds(1.75f);
        SoundEffectController.instance.SoundEffect(7);
        EffectController.instance.CreateEffect(7, gameObject.transform.position, 1f);
        Destroy(gameObject);
    }

    public void StartNextStage()
    {
        switch (curBossStage)
        {
            case BossStage.ENTERING:
                leftTarget.parent = null;
                rightTarget.parent = null;
                col.enabled = true;
                creationTime = 12.5f;
                speed = 1.4f;
                curBossStage = BossStage.STAGE_1;
                break;
            case BossStage.STAGE_1:
                creationTime = 7.5f;
                speed = 2f;
                ChangeStatusToProtecting(2, 2, false);
                break;
            case BossStage.STAGE_2:
                creationTime = 2.5f;
                speed = 2.5f;
                ChangeStatusToProtecting(2, 0 , true);
                break;
            case BossStage.STAGE_3:
                creationTime = .1f;
                speed = 3f;
                ChangeStatusToProtecting(3, 3, true);
                break;
            case BossStage.STAGE_4:
                curBossStage = BossStage.DEAD;
                break;
            case BossStage.PROTECTING:
                if (BossHealthController.instance.currentHealth <= PercentageOfHealth(25)) curBossStage = BossStage.STAGE_4;
                else if (BossHealthController.instance.currentHealth <= PercentageOfHealth(50)) curBossStage = BossStage.STAGE_3;
                else if (BossHealthController.instance.currentHealth <= PercentageOfHealth(75)) curBossStage = BossStage.STAGE_2;
                break;
        }
    }

    public void ChangeStatusToProtecting(int _smallShipCount, int _bigShipCount, bool _isSupCreated)
    {
        isFired = false;
        isCoroutineStarted = false;
        curBossStage = BossStage.PROTECTING;
        speed = 3f;
        GameManager.instance.isPlayerBlocked = true;
        StartCoroutine(EnemySpawner.instance.CreateShips(_smallShipCount, _bigShipCount, _isSupCreated));
        for (int i = 0; i < bossBullets.transform.childCount; i++)
        {
            bossBullets.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void Fire(ObjectPool pool)
    {
        bulletFireCounter -= Time.deltaTime;
        laserFireCounter -= Time.deltaTime;

        if (bulletFireCounter < 0)
        {
            StartCoroutine(BulletFire(pool));
            if (Mathf.Abs(player.transform.position.x - transform.position.x) < 2.5f) bulletFireCounter = Random.Range(1f, 1.75f);
            else bulletFireCounter = Random.Range(1.75f, 2.25f);
        }
        else if (laserFireCounter < 0)
        {
            StartCoroutine(LaserFire(laserPos1, laser1));
            StartCoroutine(LaserFire(laserPos2, laser2));
            laserFireCounter = Random.Range(7f, 10f);
            bulletFireCounter = Random.Range(4f, 6f);
        }
    }

    public IEnumerator BulletFire(ObjectPool pool)
    {
        GameObject bullet = pool.PullObjectFromPool();
        bullet.transform.position = bulletPos.position;
        yield return new WaitForSeconds(1.5f);
        pool.AddObjectToPool(bullet);
    }

    IEnumerator MissileFire()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < missilePositions.childCount; i++)
        {
            SoundEffectController.instance.SoundEffect(9);
            GameObject missile = BulletPools.instance.bossMissilePool.PullObjectFromPool();
            missile.transform.SetPositionAndRotation(missilePositions.GetChild(i).transform.position, missilePositions.GetChild(i).transform.rotation);
            yield return new WaitForSeconds(.25f);
        }
    }

    IEnumerator BombFire()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject bomb = BulletPools.instance.bossBombPool.PullObjectFromPool();
            bomb.transform.position = bombPos.transform.position;
            yield return new WaitForSeconds(.25f);
        }
    }
    IEnumerator LaserFire(Transform laserPos, GameObject laser)
    {
        yield return new WaitForSeconds(.5f);
        laserPos.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.25f);
        SoundEffectController.instance.SoundEffect(4);
        laser.SetActive(true);
        laserPos.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        laser.SetActive(false);
    }

    IEnumerator CreateLaserSup()
    {
        yield return new WaitForSeconds(12.5f);
        float posY = Random.Range(-2f, -7.5f);
        Vector2 pos = new(0, posY);
        Instantiate(laserSupMachinePrefab, pos, Quaternion.identity);
        Debug.Log(creationTime);
        yield return new WaitForSeconds(creationTime);
        StartCoroutine(CreateLaserSup());
    }

    void CreateSupMachine()
    {
        Instantiate(soloSupMachinePrefab, transform.position, Quaternion.identity);
    }

}
