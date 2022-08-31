using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSmallEnemyController : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] Transform bulletPos;
    public float fireCounter;
    [SerializeField] GameObject bulletPrefab, bombPrefab;

    private void Start()
    {
        fireCounter = Random.Range(6f, 8f);
        InvokeRepeating(nameof(BombFire), 5f, 6f);
    }

    private void Update()
    {
        fireCounter -= Time.deltaTime;

        if (fireCounter <= 0)
        {
            StartCoroutine(BulletFire());
            fireCounter = Random.Range(1f, 1.5f);
        }

    }

    IEnumerator BulletFire()
    {
        SoundEffectController.instance.BulletSoundEffect(1);
        GameObject bullet = BulletPools.instance.sEnemyBulletPool.PullObjectFromPool();
        bullet.transform.position = bulletPos.position;
        yield return new WaitForSeconds(1.5f);
        BulletPools.instance.sEnemyBulletPool.AddObjectToPool(bullet);
    }

    public void BombFire()
    {
        GameObject bomb = BulletPools.instance.sEnemyBombPool.PullObjectFromPool();
        bomb.transform.position = transform.position;
    }

}
