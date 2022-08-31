using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{

    [Header("Health")]
    public int maxHealth;
    public int currentHealth;
    public bool isDead;

    float laserDamageTimer;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        laserDamageTimer = 1f;
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Enemy ship takes damage
    /// </summary>
    /// <param name="value"></param>
    public void TakeDamage(int value)
    {
        currentHealth -= value;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Destroys enemy ship
    /// </summary>
    public void Die()
    {
        SoundEffectController.instance.SoundEffect(3);
        isDead = true;
        CameraController.instance.ShakeCamera(.25f, .2f);
        if (gameObject.name.Contains("BigEnemy"))
        {
            EffectController.instance.CreateEffect(4, gameObject.transform.position, 1f);
        } 
        else if (gameObject.name.Contains("SmallEnemy"))
        {
            EffectController.instance.CreateEffect(5, gameObject.transform.position, 1f);
        }
        else if (gameObject.name == "TeleportableEnemy(Clone)" || gameObject.name == "EnemySupMachine" || gameObject.name == "SoloSupMachine(Clone)")
        {
            EffectController.instance.CreateEffect(5, gameObject.transform.position, 1f);
        }
        Destroy(gameObject);
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
