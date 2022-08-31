using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportMachineController : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] Transform bulletPos;
    float fireCooldown;

    [Header("Kamikaze")]
    GameObject enemies;
    public float speed;
    public float rotateSpeed;
    public bool isDying;
    public bool isKamikazeStart;
    GameObject enemy;

    [Space]
    public int maxHealth = 5;
    int currentHealth;

    // Components
    Animator anim;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Start()
    {
        enemies = GameObject.FindGameObjectWithTag("Enemies");
        currentHealth = maxHealth;
        fireCooldown = 1f;
        StartCoroutine(DestroyRoutine());
    }

    private void Update()
    {
        if (PlayerHealthController.instance.isDead || currentHealth <= 0) DestroySupMachine();

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0 && GameManager.instance.curState != GameManager.GameState.SPAWNING)
        {
            StartCoroutine(BulletFire());
            fireCooldown = 1f;
        }
    }

    private void FixedUpdate()
    {

        if (isDying)
        {
            if (enemies.transform.childCount <= 0) DestroySupMachine();
            Kamikaze(enemy);
            StopCoroutine(BulletFire());
            if(enemy == null) enemy = ChooseClosestEnemy();
        }
    }
    /// <summary>
    /// Support Machine Fire
    /// </summary>
    IEnumerator BulletFire()
    {
        SoundEffectController.instance.BulletSoundEffect(1);
        GameObject bullet = BulletPools.instance.supMachineBulletPool.PullObjectFromPool();
        bullet.transform.position = bulletPos.position;
        yield return new WaitForSeconds(1.5f);
        BulletPools.instance.supMachineBulletPool.AddObjectToPool(bullet);
    }

    /// <summary>
    /// Destroys Support Machine after 15 seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(12f);
        enemy = ChooseClosestEnemy();
        isDying = true;
        anim.SetTrigger("death");
        yield return new WaitForSeconds(3f);
        DestroySupMachine();
    }

    public void DestroySupMachine()
    {
        SoundEffectController.instance.SoundEffect(6);
        EffectController.instance.CreateEffect(2, transform.position, 1f);
        PlayerController.instance.isSupActive = false;
        Destroy(gameObject);
    }

    public void Kamikaze(GameObject _enemy)
    {
        isKamikazeStart = true;
        gameObject.transform.parent = null;

        // That's Brackeys' solve. It's better than mine :')
        Vector2 direction = (Vector2)_enemy.transform.position - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.velocity = transform.up * speed;

        // That's my solve.
        //Vector2 dir = _enemy.transform.position - transform.position;
        //dir.Normalize();

        //float angle = Vector2.Angle(dir, transform.up);

        //transform.position = Vector2.MoveTowards(transform.position, _enemy.transform.position, speed * Time.fixedDeltaTime);

        //if (_enemy.transform.position.x > transform.position.x) transform.eulerAngles = new Vector3(0f, 0f, -angle);
        //else if (_enemy.transform.position.x < transform.position.x) transform.eulerAngles = new Vector3(0f, 0f, angle);
    }

    public GameObject ChooseClosestEnemy()
    {
        GameObject closestEnemy = null;
        float currentDir = Mathf.Infinity;
        
        // Find closest enemy
        for (int i = 0; i < enemies.transform.childCount; i++)
        {
            GameObject curEnemy = enemies.transform.GetChild(i).gameObject;
            float dir = Vector2.Distance(curEnemy.transform.position, transform.position);
            if(dir < currentDir)
            {
                closestEnemy = curEnemy;
                currentDir = dir;
            }
        }
        return closestEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isKamikazeStart && collision.CompareTag("Enemy")) DestroySupMachine();
        else if (collision.CompareTag("EnemyBullet")) currentHealth--;
        else if (collision.CompareTag("EnemyLaser")) currentHealth -= 5;
    }
}
