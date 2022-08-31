using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoubleSupController : MonoBehaviour
{
    [SerializeField] float posX, firstPosX;
    [SerializeField] float speed;
    Vector2 targetPos, firstPos;
    float cooldown = 5f;

    private void Start()
    {
        targetPos = new(posX, transform.position.y);
        firstPos = new(firstPosX, transform.position.y);
    }

    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, targetPos, speed * Time.deltaTime);
        cooldown -= Time.deltaTime;
        if (cooldown <= 0) transform.position = Vector2.Lerp(transform.position, firstPos, speed * Time.deltaTime);
    }
}
