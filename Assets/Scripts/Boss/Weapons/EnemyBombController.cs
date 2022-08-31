using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBombController : MonoBehaviour
{

    public float speed;
    Vector2 targetPos;

    private void Start()
    {
        targetPos = ChoosePosition();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    public Vector2 ChoosePosition()
    {
        float xPos = Random.Range(-11.5f, 11.5f);
        float yPos = Random.Range(-5f, -1.5f);
        return new Vector2(xPos, yPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Bullet") || collision.CompareTag("Laser") || collision.CompareTag("PowerfulBullet"))
        {
            SoundEffectController.instance.SoundEffect(6);
            EffectController.instance.CreateEffect(1, gameObject.transform.position, .5f);
            if (gameObject.CompareTag("EnemyBomb")) BulletPools.instance.sEnemyBombPool.AddObjectToPool(gameObject);
            else if (gameObject.CompareTag("BossBomb")) BulletPools.instance.bossBombPool.AddObjectToPool(gameObject);
        }
    }

}
