using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BossBigEnemyController : MonoBehaviour
{

    [Header("Attack")]
    [SerializeField] GameObject laser;
    [SerializeField] Transform laserPos, bulletPos;
    public float fireCounter;
    bool isLaserFired;
    
    private void Start()
    {
        fireCounter = 1f;
        StartCoroutine(LaserFire());
    }

    private void Update()
    {
        fireCounter -= Time.deltaTime;
        if (fireCounter <= 0 && !isLaserFired)
        {
            StartCoroutine(BulletFire());
            fireCounter = Random.Range(1f, 2.5f);
        }
    }

    IEnumerator BulletFire()
    {
        GameObject bullet = BulletPools.instance.bEnemyBulletPool.PullObjectFromPool();
        bullet.transform.position = bulletPos.position;
        yield return new WaitForSeconds(1.5f);
        BulletPools.instance.bEnemyBulletPool.AddObjectToPool(bullet);
    }

    IEnumerator LaserFire()
    {
        yield return new WaitForSeconds(7f);
        isLaserFired = true;
        laserPos.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.25f);
        SoundEffectController.instance.SoundEffect(4);
        laser.SetActive(true);
        laserPos.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        laser.SetActive(false);
        isLaserFired = false;
        yield return new WaitForSeconds(.1f);
        StartCoroutine(LaserFire());
    }

}
