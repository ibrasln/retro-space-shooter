using System.Collections;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{

    public static PowerUpController instance;

    Vector2 randPos;
    public bool isPowerUpDestroyed;

    Animator anim;

    private void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        isPowerUpDestroyed = false;
        randPos = new Vector2(transform.position.x, Random.Range(-5f, -3f));
        StartCoroutine(DestroyRoutine());
    }

    private void FixedUpdate()
    {
        MoveDown();
    }

    public void MoveDown()
    {
        transform.position = Vector2.LerpUnclamped(transform.position, randPos, Time.fixedDeltaTime);
    }

    IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(12f);
        anim.SetTrigger("destroy");
        yield return new WaitForSeconds(3f);
        DestroyPowerUp();
    }

    public void DestroyPowerUp()
    { 
        isPowerUpDestroyed = true;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundEffectController.instance.SoundEffect(11);
            PowerUpSpawner.instance.powerUpText.gameObject.SetActive(true);
            PowerUpSpawner.instance.powerUpText.transform.position = transform.position;
            DestroyPowerUp();
        }
    }
}
