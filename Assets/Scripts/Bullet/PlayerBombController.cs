using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBombController : MonoBehaviour
{
    public float speed;
    Vector2 targetPos;

    private void Start()
    {
        targetPos = ChoosePosition();
        StartCoroutine(BombCountdown());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    public Vector2 ChoosePosition()
    {
        float xPos = Random.Range(-11.5f, 11.5f);
        float yPos = Random.Range(0, 5f);
        return new Vector2(xPos, yPos);
    }

    IEnumerator BombCountdown()
    {
        yield return new WaitForSeconds(30f);
        Explosion();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("EnemyBullet") || collision.CompareTag("BossBullet") || collision.CompareTag("EnemyLaser") || collision.CompareTag("BossShield"))
        {
            Explosion();
        }
    }

    public void Explosion()
    {
        SoundEffectController.instance.SoundEffect(6);
        EffectController.instance.CreateEffect(2, gameObject.transform.position, .5f);
        BulletPools.instance.playerBombPool.AddObjectToPool(gameObject);
    }
}
