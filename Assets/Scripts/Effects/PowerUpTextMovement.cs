using System.Collections;
using UnityEngine;

public class PowerUpTextMovement : MonoBehaviour
{
    private float timer;
    private bool startMove;

    private void Start()
    {
        timer = 1.5f;
    }

    // It moves power-up texts and enemy score effects
    private void Update()
    {
        if (!startMove && PowerUpController.instance.isPowerUpDestroyed) startMove = true;

        if (startMove)
        {
            timer -= Time.deltaTime;
            transform.position += new Vector3(0f, 1f * Time.deltaTime, 0f);
        }

        if (timer <= 0)
        {
            timer = 1.5f;
            gameObject.SetActive(false);
        }
    }

}
