using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BigEnemyController : MonoBehaviour
{

    public enum BigShipStatus { LEFTMOVEMENT, RIGHTMOVEMENT, STOP };

    [Header("Movement")]
    public BigShipStatus currentBigShipStatus;
    [SerializeField] Transform leftTarget, rightTarget;
    public float speed;
    public float cooldownCounter;
    public float fireCounter;

    [Header("Attack")]
    [SerializeField] GameObject laser;
    [SerializeField] Transform laserPos;
    [SerializeField] Transform bulletPos;
    bool isLaserFiring;

    EnemyStateController enemyStateController;
    EnemyHealthController enemyHealthController;

    private void Awake()
    {
        enemyHealthController = GetComponent<EnemyHealthController>();
        enemyStateController = GetComponent<EnemyStateController>();
    }

    private void Start()
    {
        speed = Random.Range(2.5f, 4.5f);
        fireCounter = Random.Range(.5f, 1.5f);
        cooldownCounter = 4.5f;
        Invoke(nameof(SetTargetsNull), 5f);
    }

    private void Update()
    {

        if (enemyStateController.curState == EnemyStateController.EnemyState.MOVING) return;

        if (enemyHealthController.isDead)
        {
            Destroy(leftTarget);
            Destroy(rightTarget);
        }

        if (cooldownCounter > 0)
        {
            Movement();
        }

        #region Changes Ship's Direction
        if (transform.position.x >= rightTarget.position.x)
        {
            transform.position = new Vector2(rightTarget.position.x, transform.position.y);
            ChangeShipDirection(BigShipStatus.LEFTMOVEMENT);
        }
        else if (transform.position.x <= leftTarget.position.x)
        {
            transform.position = new Vector2(leftTarget.position.x, transform.position.y);
            ChangeShipDirection(BigShipStatus.RIGHTMOVEMENT);
        }
        #endregion
    }

    void SetTargetsNull()
    {
        leftTarget.parent = null;
        rightTarget.parent = null;
    }

    /// <summary>
    /// Changes ship's direction
    /// </summary>
    /// <param name="nextStatus"></param>
    public void ChangeShipDirection(BigShipStatus nextStatus)
    {
        currentBigShipStatus = BigShipStatus.STOP;
        cooldownCounter -= Time.deltaTime;

        if(cooldownCounter <= 0)
        {
            currentBigShipStatus = nextStatus;
            isLaserFiring = false;
            cooldownCounter = 4.75f;
        }
        
    }

    /// <summary>
    /// Moves ship, and fire bullets and laser
    /// </summary>
    public void Movement()
    {
        fireCounter -= Time.deltaTime;

        switch (currentBigShipStatus)
        {
            case BigShipStatus.RIGHTMOVEMENT:
                ShipMovement(Vector3.right);
                break;

            case BigShipStatus.LEFTMOVEMENT:
                ShipMovement(Vector3.left);
                break;

            case BigShipStatus.STOP:
                if (!isLaserFiring)
                {
                    StartCoroutine(LaserFire());
                    isLaserFiring = true;
                }
                break;
        }
    }

    public void ShipMovement(Vector3 dir)
    {
        transform.position += speed * Time.deltaTime * dir;
        if(fireCounter < 0)
        {
            StartCoroutine(BulletFire());
            fireCounter = Random.Range(.5f, 1.25f);
        }
    }

    /// <summary>
    /// Creates bullet
    /// </summary>
    IEnumerator BulletFire()
    {
        GameObject bullet = BulletPools.instance.bEnemyBulletPool.PullObjectFromPool();
        bullet.transform.position = bulletPos.position;
        yield return new WaitForSeconds(1.5f);
        BulletPools.instance.bEnemyBulletPool.AddObjectToPool(bullet);
    }

    IEnumerator LaserFire()
    {
        yield return new WaitForSeconds(.5f);
        laserPos.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.25f);
        SoundEffectController.instance.SoundEffect(4);
        laser.SetActive(true);
        laserPos.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        laser.SetActive(false);
    }

}
