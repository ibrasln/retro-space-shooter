using UnityEngine;

/// <summary>
/// Bullet pools of gameobjects, except Player.
/// </summary>
public class BulletPools : MonoBehaviour
{

    public static BulletPools instance;

    public ObjectPool bEnemyBulletPool, sEnemyBulletPool, tEnemyBulletPool, bossBulletPool_1, bossBulletPool_2, bossBulletPool_3, bossBulletPool_4;
    public ObjectPool sEnemyBombPool, bossBombPool, bossMissilePool;
    public ObjectPool playerBombPool, playerMissilePool;
    public ObjectPool supMachineBulletPool, enemySupMachineBulletPool;

    [SerializeField] GameObject bEnemyBullet, sEnemyBullet, tEnemyBullet, bossBullet_1, bossBullet_2, bossBullet_3, bossBullet_4;
    [SerializeField] GameObject sEnemyBomb, bossMissile, bossBomb;
    [SerializeField] GameObject playerBomb, playerMissile, playerLaser;
    [SerializeField] GameObject supMachineBullet, enemySupMachineBullet;
    [SerializeField] Transform enemyBullets, enemyBombs, enemyMissiles, enemyLasers, bossBullets; 
    [SerializeField] Transform playerWeapons, playerLasers, playerBullets;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        #region Bullet Pools
        bEnemyBulletPool = new ObjectPool(bEnemyBullet, enemyBullets);
        sEnemyBulletPool = new ObjectPool(sEnemyBullet, enemyBullets);
        tEnemyBulletPool = new ObjectPool(tEnemyBullet, enemyBullets);
        bossBulletPool_1 = new ObjectPool(bossBullet_1, bossBullets);
        bossBulletPool_2 = new ObjectPool(bossBullet_2, bossBullets);
        bossBulletPool_3 = new ObjectPool(bossBullet_3, bossBullets);
        bossBulletPool_4 = new ObjectPool(bossBullet_4, bossBullets);
        supMachineBulletPool = new ObjectPool(supMachineBullet, playerBullets);
        enemySupMachineBulletPool = new ObjectPool(enemySupMachineBullet, bossBullets);

        bEnemyBulletPool.FillPool(10);
        sEnemyBulletPool.FillPool(6);
        tEnemyBulletPool.FillPool(6);
        bossBulletPool_1.FillPool(3);
        bossBulletPool_2.FillPool(3);
        bossBulletPool_3.FillPool(3);
        bossBulletPool_4.FillPool(3);
        supMachineBulletPool.FillPool(6);
        enemySupMachineBulletPool.FillPool(12);
        #endregion

        #region Weapon Pools
        sEnemyBombPool = new ObjectPool(sEnemyBomb, enemyBombs);
        bossBombPool = new ObjectPool(bossBomb, enemyBombs);
        bossMissilePool = new ObjectPool(bossMissile, enemyMissiles);
        playerBombPool = new ObjectPool(playerBomb, playerWeapons);
        playerMissilePool = new ObjectPool(playerMissile, playerWeapons);
        
        sEnemyBombPool.FillPool(12);
        bossBombPool.FillPool(10);
        bossMissilePool.FillPool(12);
        playerBombPool.FillPool(3);
        playerMissilePool.FillPool(3);
        #endregion
    }

}
