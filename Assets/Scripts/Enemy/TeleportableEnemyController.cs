using System.Collections;
using UnityEngine;

public class TeleportableEnemyController : MonoBehaviour
{
    [SerializeField] GameObject teleportStartEffectPrefab, teleportFinishEffectPrefab;
    [SerializeField] Transform bulletPos;
    Vector2 targetPos;
    float fireCooldown, speed;
    PolygonCollider2D enemyCollider;
    bool isReached;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyCollider = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        speed = 3f;
        fireCooldown = 8f;
        targetPos = new(Random.Range(-11.5f, 11.5f), Random.Range(-1.25f, 4f));
    }

    private void Update()
    {
        if (!isReached && Vector2.Distance(transform.position, targetPos) <= .1f) isReached = true;
        if (!isReached)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        }

        fireCooldown -= Time.deltaTime;
        
        if(isReached && fireCooldown <= 0)
        {
            StartCoroutine(BulletFire());
            fireCooldown = Random.Range(1f, 2f);
        }
    }

    IEnumerator Teleport()
    {
        fireCooldown = 2f;
        SoundEffectController.instance.SoundEffect(10);
        anim.SetTrigger("teleport");
        float randX = Random.Range(-12f, 12f);
        float randY = Random.Range(-3f, 4f);
        Vector2 newPos = new(randX, randY);

        yield return new WaitForSeconds(.75f);
        transform.position = newPos;
        enemyCollider.enabled = true;

    }

    /// <summary>
    /// Creates bullet
    /// </summary>
    IEnumerator BulletFire()
    {
        SoundEffectController.instance.BulletSoundEffect(1);
        GameObject bullet = BulletPools.instance.tEnemyBulletPool.PullObjectFromPool();
        bullet.transform.position = bulletPos.position;
        yield return new WaitForSeconds(1.5f);
        BulletPools.instance.tEnemyBulletPool.AddObjectToPool(bullet);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bullet") || collision.CompareTag("Laser") || collision.CompareTag("PowerfulBullet") || collision.CompareTag("BombExplosion") || collision.CompareTag("Missile"))
        {
            enemyCollider.enabled = false;
            StartCoroutine(Teleport());
        }
    }

}
