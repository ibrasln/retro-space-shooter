using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemyController : MonoBehaviour
{
    [Header("Movement")]
    public float amplitudeX;
    public float amplitudeY = 4f;
    public float omegaX = .5f;
    public float omegaY = .2f;
    float index;

    [Header("Attack")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletPos;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] Transform leftBombPos, rightBombPos;
    float fireCooldown;

    EnemyStateController enemyStateController;

    private void Start()
    {
        enemyStateController = GetComponent<EnemyStateController>();
        fireCooldown = 1.5f;
        InvokeRepeating(nameof(BombFire), 8f, 15f);
    }

    public void Update()
    {
        if (enemyStateController.curState == EnemyStateController.EnemyState.MOVING) return;

        #region Movement
        index += Time.deltaTime;
        float x = amplitudeX * Mathf.Cos(omegaX * index);
        float y = Mathf.Abs(amplitudeY * Mathf.Sin(omegaY * index));
        transform.localPosition = new Vector3(x, y, 0);
        #endregion

        #region Fire
        fireCooldown -= Time.deltaTime;
        if(fireCooldown <= 0)
        {
            StartCoroutine(BulletFire());
            fireCooldown = Random.Range(.5f, 1.5f);
        }
        #endregion
    }

    /// <summary>
    /// Creates bullet
    /// </summary>
    IEnumerator BulletFire()
    {
        SoundEffectController.instance.BulletSoundEffect(1);
        GameObject bullet = BulletPools.instance.sEnemyBulletPool.PullObjectFromPool();
        bullet.transform.position = bulletPos.position;
        yield return new WaitForSeconds(1.5f);
        BulletPools.instance.sEnemyBulletPool.AddObjectToPool(bullet);
    }

    /// <summary>
    /// Creates bomb
    /// </summary>
    public void BombFire()
    {
        GameObject bomb1 = BulletPools.instance.sEnemyBombPool.PullObjectFromPool();
        GameObject bomb2 = BulletPools.instance.sEnemyBombPool.PullObjectFromPool();
        bomb1.transform.position = leftBombPos.position;
        bomb2.transform.position = rightBombPos.position;
    }

}
