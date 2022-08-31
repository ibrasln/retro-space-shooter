using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSupMachineController : MonoBehaviour
{

    float fireCounter;
    [SerializeField] Transform bulletPos;

    private void Start()
    {
        fireCounter = Random.Range(5f, 7f);
    }

    // Update is called once per frame
    void Update()
    {
        fireCounter -= Time.deltaTime;
        if (fireCounter <= 0)
        {
            StartCoroutine(BulletFire());
            fireCounter = Random.Range(1f, 2f);
        }
    }

    IEnumerator BulletFire()
    {
        SoundEffectController.instance.BulletSoundEffect(1);
        GameObject bullet = BulletPools.instance.enemySupMachineBulletPool.PullObjectFromPool();
        bullet.transform.position = bulletPos.position;
        yield return new WaitForSeconds(1.5f);
        BulletPools.instance.enemySupMachineBulletPool.AddObjectToPool(bullet);
    }

}
