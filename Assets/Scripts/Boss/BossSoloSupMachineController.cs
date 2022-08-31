using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSoloSupMachineController : MonoBehaviour
{

    float fireCounter;
    bool isRight;
    [SerializeField] float speed;
    [SerializeField] Transform bulletPos;
    Vector2 leftTargetPos, rightTargetPos;

    private void Start()
    {
        leftTargetPos = new(-14.25f, transform.position.y);
        rightTargetPos = new(14.25f, transform.position.y);
        fireCounter = Random.Range(5f, 7f);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isPlayerBlocked) return;
        else
        {
            #region Movement
            if (isRight) transform.position = Vector2.MoveTowards(transform.position, rightTargetPos, speed * Time.deltaTime);
            else transform.position = Vector2.MoveTowards(transform.position, leftTargetPos, speed * Time.deltaTime);

            if (transform.position.x <= leftTargetPos.x) isRight = true;
            else if (transform.position.x >= rightTargetPos.x) isRight = false;
            #endregion

            fireCounter -= Time.deltaTime;
            if (fireCounter <= 0)
            {
                StartCoroutine(BulletFire());
                fireCounter = Random.Range(1f, 2f);
            }
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
