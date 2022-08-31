using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthController : MonoBehaviour
{

    public static BossHealthController instance;

    [Header("Health")]
    public int maxHealth;
    public int currentHealth;
    public bool isBossDead;
    HealthBarController healthBar;

    float laserDamageTimer;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        instance = this;
    }

    void Start()
    {
        laserDamageTimer = 1f;
        healthBar = UIController.instance.bossHealthBar.GetComponent<HealthBarController>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0) isBossDead = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet")) TakeDamage(1);
        else if (collision.CompareTag("BombExplosion")) TakeDamage(2);
        else if (collision.CompareTag("Missile")) TakeDamage(3);
        else if (collision.CompareTag("PowerfulBullet")) TakeDamage(4);
        else if (collision.CompareTag("Laser"))
        {
            TakeDamage(5);
            anim.SetBool("hurtFromLaser", true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            laserDamageTimer -= Time.deltaTime;
            if (laserDamageTimer <= 0)
            {
                TakeDamage(5);
                laserDamageTimer = 1f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        anim.SetBool("hurtFromLaser", false);
    }
}
