using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float speed;
    public Vector2 dir;
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * dir);
    }

}
