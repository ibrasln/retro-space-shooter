using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    [Header("Health")]
    public int maxHealth = 25;
    public int currentHealth;
    [SerializeField] GameObject deathEffectPrefab;
    public bool isDead;
    public HealthBarController healthBar;
    [SerializeField] GameObject rocketFire;
    float laserDamageTimer;

    // Components
    Animator anim;
    BoxCollider2D myCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        maxHealth = 40;
        laserDamageTimer = 1f;
        instance = this;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    /// <summary>
    /// Player takes damage
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        StartCoroutine(CameraController.instance.ShakeCamera(.25f, .2f));

        if (PlayerController.instance.isShieldActive) ShieldController.instance.currentHealth -= damage;
        else
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            StartCoroutine(DieRoutine());
        }
    }

    public void IncreaseHealth(int value)
    {
        currentHealth += value;

        if(currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.SetHealth(currentHealth);

    }

    /// <summary>
    /// Destroys player
    /// </summary>
    /// <returns></returns>
    IEnumerator DieRoutine()
    {
        SoundEffectController.instance.SoundEffect(2);
        rocketFire.SetActive(false);
        anim.SetTrigger("death");
        myCollider.enabled = false;
        yield return new WaitForSeconds(1.5f);
        SoundEffectController.instance.soundEffects[2].Stop();
        SoundEffectController.instance.SoundEffect(3);
        EffectController.instance.CreateEffect(6, gameObject.transform.position, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet")) TakeDamage(1);
        else if (collision.CompareTag("EnemyBombExplosion") || collision.CompareTag("BossMissile")) TakeDamage(2);
        else if (collision.CompareTag("BossBullet")) TakeDamage(3);
        else if (collision.CompareTag("EnemyLaser"))
        {
            TakeDamage(5);
            anim.SetBool("hurtFromLaser", true);
        }
        else if (collision.CompareTag("SupLaser"))
        {
            TakeDamage(3);
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
