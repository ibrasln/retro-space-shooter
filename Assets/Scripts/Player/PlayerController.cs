using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;
    public enum BulletStatus { normalBullet, doubleBullet, tripleBullet, laser, powerfulBullet }


    [Header("Bullet")]
    public BulletStatus currentBulletStatus;
    [SerializeField] GameObject normalBulletPrefab, doubleBulletPrefab, tripleBulletPrefab, laserPrefab, powerfulBulletPrefab, missilePrefab, bombPrefab;
    GameObject laser;
    [SerializeField] Transform normalBulletPos, doubleBulletPos, tripleBulletPos, laserPos, lasers;
    private ObjectPool normalBulletPool, doubleBulletPool, tripleBulletPool, powerfulBulletPool;
    [SerializeField] Transform bullets;
    [SerializeField] GameObject bulletTimerBar;

    [Header("Common Variables")]
    [SerializeField] GameObject rocketFire;
    [SerializeField] CanvasGroup healthBar, bombUI, missileUI;
    public float speed;
    Vector2 movement;
    public float laserAttackTimer = 0f;

    [Header("Ammo")]
    public int bombAmmo;
    public int missileAmmo;

    [Header("Power Ups")]
    public bool isShieldActive;
    public bool isSupActive;
    [SerializeField] GameObject supportMachine, shield;
    [SerializeField] Transform supportPos;
    [SerializeField] TMP_Text powerUpText;

    [Space]
    [SerializeField] GameObject enemies;

    // Components
    Animator anim;
    Rigidbody2D rb;
    [HideInInspector] public Collider2D col;

    private void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        StartCoroutine(Starting());
    }

    private void Start()
    {
        bombAmmo = 0;
        missileAmmo = 0;
        currentBulletStatus = BulletStatus.normalBullet;

        laser = Instantiate(laserPrefab, laserPos.position, Quaternion.identity);
        laser.SetActive(false);

        #region Pools
        normalBulletPool = new ObjectPool(normalBulletPrefab, bullets);
        normalBulletPool.FillPool(20);
        doubleBulletPool = new ObjectPool(doubleBulletPrefab, bullets);
        doubleBulletPool.FillPool(20);
        tripleBulletPool = new ObjectPool(tripleBulletPrefab, bullets);
        tripleBulletPool.FillPool(20);
        powerfulBulletPool = new ObjectPool(powerfulBulletPrefab, bullets);
        powerfulBulletPool.FillPool(20);
        #endregion

    }

    private void Update()
    {

        if (GameManager.instance.isGameWon)
        {
            transform.position += 5f * Time.deltaTime * Vector3.up;
            return;
        }
        if (GameManager.instance.curState == GameManager.GameState.GAMESTARTING || GameManager.instance.isPlayerBlocked || GameManager.instance.curState == GameManager.GameState.GAMESTARTING || PlayerHealthController.instance.isDead)
        {
            rb.velocity = Vector2.zero;
            laser.SetActive(false);
            CancelInvoke(nameof(Fire)); // We canceled the Fire function, because when the ship is dead, it's continuing to creating bullets.
            return;
        }
        else
        {
            Movement();
            
            #region Laser Attack
            laserAttackTimer -= Time.deltaTime;

            if(currentBulletStatus == BulletStatus.laser && laserAttackTimer <= 0 && Input.GetButtonDown("Jump"))
            {
                StartCoroutine(CreateLaser());
                laserAttackTimer = 3f;
            }
            #endregion

            #region Fire
            if (Input.GetButtonDown("Jump"))
            {
                InvokeRepeating(nameof(Fire), 0.01f, 0.4f);
            }

            if (Input.GetButtonUp("Jump"))
            {
                CancelInvoke(nameof(Fire));
            }
            #endregion

            #region Bomb - Missile Fire
            if (Input.GetKeyDown(KeyCode.V) && missileAmmo > 0 && enemies.transform.childCount > 0)
            {
                SoundEffectController.instance.SoundEffect(9);
                CreateWeapon(BulletPools.instance.playerMissilePool);
                missileAmmo--;
            }
            else if (Input.GetKeyDown(KeyCode.B) && bombAmmo > 0)
            {
                CreateWeapon(BulletPools.instance.playerBombPool);
                bombAmmo--;
            }
            #endregion
        }
    }

    IEnumerator Starting()
    {
        anim.SetTrigger("start");
        rocketFire.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        healthBar.DOFade(1f, 1f);
        yield return new WaitForSeconds(.25f);
        missileUI.DOFade(1f, 1f);
        yield return new WaitForSeconds(.25f);
        bombUI.DOFade(1f, 1f);
    }

    /// <summary>
    /// Player Movement
    /// </summary>
    public void Movement()
    {

        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        rb.velocity = speed * movement;

        anim.SetFloat("horizontal", movement.x);
        anim.SetFloat("vertical", movement.y);
        anim.SetFloat("speed", movement.sqrMagnitude);
    }

    /// <summary>
    /// Creates bullets
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="weaponPos"></param>
    IEnumerator CreateBullet(ObjectPool pool, Transform pos)
    {
        SoundEffectController.instance.BulletSoundEffect(1);
        GameObject bullet = pool.PullObjectFromPool();
        bullet.transform.position = pos.position;
        yield return new WaitForSeconds(2f);
        pool.AddObjectToPool(bullet);
    }

    /// <summary>
    /// Creates laser
    /// </summary>
    IEnumerator CreateLaser()
    {
        SoundEffectController.instance.SoundEffect(4);
        laser.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        laser.SetActive(false);
    }

    public void CreateWeapon(ObjectPool weaponPool)
    {
        GameObject weapon = weaponPool.PullObjectFromPool();
        weapon.transform.position = normalBulletPos.position;
    }

    /// <summary>
    /// Fires bullets
    /// </summary>
    public void Fire()
    {
        switch (currentBulletStatus)
        {
            case BulletStatus.normalBullet:
                StartCoroutine(CreateBullet(normalBulletPool, normalBulletPos));
                break;

            case BulletStatus.doubleBullet:
                StartCoroutine(CreateBullet(doubleBulletPool, doubleBulletPos));
                break;

            case BulletStatus.tripleBullet:
                StartCoroutine(CreateBullet(tripleBulletPool, tripleBulletPos));
                break;

            case BulletStatus.powerfulBullet:
                StartCoroutine(CreateBullet(powerfulBulletPool, normalBulletPos));
                break;
        }
    }

    #region Take Power-Up Functions
    public void TakeWeaponPowerUp(BulletStatus status, string _text)
    {
        bulletTimerBar.SetActive(true);
        BulletTimerController.instance.maxVal = 15f;
        currentBulletStatus = status;
        powerUpText.text = _text;
    }

    public void TakeSupportPowerUp(GameObject prefab, string _text, bool isActive)
    {
        if (isActive)
        {
            ShieldController.instance.currentHealth = 10;
        }
        else
        {
            Instantiate(prefab, supportPos.position, Quaternion.identity, supportPos);
            powerUpText.text = _text;
        }
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        #region Power-Ups
        if (collision.CompareTag("Weapon1"))
        {
            TakeWeaponPowerUp(BulletStatus.doubleBullet, "Double Bullet");
        }
        else if (collision.CompareTag("Weapon2"))
        {
            TakeWeaponPowerUp(BulletStatus.tripleBullet, "Triple Bullet");
        }
        else if (collision.CompareTag("Weapon3"))
        {
            TakeWeaponPowerUp(BulletStatus.powerfulBullet, "Magma Bullet");
        }
        else if (collision.CompareTag("Weapon4"))
        {
            TakeWeaponPowerUp(BulletStatus.laser, "Laser");
        }
        else if (collision.CompareTag("MissilePowerUp"))
        {
            if (missileAmmo < 3)
            {
                missileAmmo++;
                powerUpText.text = "+1 Missile";
            }
            else
            {
                powerUpText.text = "Missile ammo is full";
            }
        }
        else if (collision.CompareTag("BombPowerUp"))
        {
            if (bombAmmo < 3)
            {
                bombAmmo++;
                powerUpText.text = "+1 Bomb";
            }
            else
            {
                powerUpText.text = "Bomb ammo is full";
            }
        }
        else if (collision.CompareTag("ShieldPowerUp"))
        {
            TakeSupportPowerUp(shield, "Shield", isShieldActive);
            isShieldActive = true;
        }
        else if (collision.CompareTag("SupportMachinePowerUp"))
        {
            TakeSupportPowerUp(supportMachine, "Support Machines", isSupActive);
            isSupActive = true;
        }
        else if (collision.CompareTag("HealthPowerUp"))
        {
            PlayerHealthController.instance.IncreaseHealth(5);
            powerUpText.text = "+5 Health";
        }
        #endregion
    }
}
