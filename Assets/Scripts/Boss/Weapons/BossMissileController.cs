using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMissileController : MonoBehaviour
{

    GameObject player;
    public float speed, rotateSpeed;
    public float followCooldown;
    public bool isFollowing;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        followCooldown = 1f;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {

        if (GameManager.instance.isPlayerBlocked)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }

        if (!isFollowing)
        {
            followCooldown -= Time.deltaTime;
            rb.velocity = speed * transform.up;
            if (followCooldown < 0) isFollowing = true;
        }
        else
        {
            if (player) Movement();
            else Hit();
        }
    }

    public void Movement()
    {
        Vector2 direction = (Vector2)player.transform.position - rb.position;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Bullet") || collision.CompareTag("PowerfulBullet") || collision.CompareTag("Laser")) Hit();
    }

    public void Hit()
    {
        EffectController.instance.CreateEffect(3, gameObject.transform.position, 1f);
        BulletPools.instance.bossMissilePool.AddObjectToPool(gameObject);
    }

}
