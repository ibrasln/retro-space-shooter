using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissileController : MonoBehaviour
{
    public float speed, rotateSpeed;

    GameObject enemies;
    GameObject enemy;
    

    [SerializeField] GameObject explosionEffectPrefab;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        enemies = GameObject.FindGameObjectWithTag("Enemies");
        enemy = ChooseEnemy();
    }

    private void FixedUpdate()
    {
        if (enemies == null)
        {
            return;
        }
        if(enemy == null) enemy = ChooseEnemy();
        Movement();
    }

    /// <summary>
    /// Moves missile
    /// </summary>
    public void Movement()
    {
        Vector2 direction = (Vector2)enemy.transform.position - rb.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;

        rb.velocity = transform.up * speed;

        //Vector2 dir = enemy.transform.position - transform.position;
        //dir.Normalize();

        //float angle = Vector2.Angle(dir, Vector2.up);

        //transform.position = Vector2.MoveTowards(transform.position, enemy.transform.position, speed * Time.deltaTime);

        //if (enemy.transform.position.x > transform.position.x) transform.eulerAngles = new Vector3(0f, 0f, -angle);
        //else if (enemy.transform.position.x < transform.position.x) transform.eulerAngles = new Vector3(0f, 0f, angle);
    }


    /// <summary>
    /// Choose random enemy
    /// </summary>
    /// <returns></returns>
    public GameObject ChooseEnemy()
    {
        int randNum = Random.Range(0, enemies.transform.childCount);
        GameObject _enemy = enemies.transform.GetChild(randNum).gameObject;
        return _enemy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss") || collision.CompareTag("BossShield"))
        {
            Hit();
        }
    }

    public void Hit()
    {
        SoundEffectController.instance.SoundEffect(6);
        EffectController.instance.CreateEffect(3, gameObject.transform.position, 1f);
        BulletPools.instance.playerMissilePool.AddObjectToPool(gameObject);
    }

}