using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackgroundMovement : MonoBehaviour
{

    public float speed;

    private void Update()
    {
        transform.Translate(new Vector2(0f, -speed * Time.deltaTime));
        
        if(transform.position.y < -18f)
        {
            transform.position = new Vector2(transform.position.x, 13f);
        }

    }
}
