using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabPoolTest : MonoBehaviour
{
    float speed = 5f;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.velocity = speed * Vector2.up;
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
