using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{

    public static ShieldController instance;

    public int maxHealth = 10;
    public int currentHealth;

    Animator anim;

    private void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        StartCoroutine(DestroyRoutine());
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            PlayerController.instance.isShieldActive = false;
        }
    }

    void TakeDamage(int value)
    {
        StartCoroutine(CameraController.instance.ShakeCamera(.25f, .2f));
        currentHealth -= value;
    }

    IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(12f);
        anim.SetTrigger("death");
        yield return new WaitForSeconds(3f);
        currentHealth = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet")) TakeDamage(1);
        else if (collision.CompareTag("BossMissile")) TakeDamage(2);
        else if (collision.CompareTag("BossBullet")) TakeDamage(3);
    }
}
