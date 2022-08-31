using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTestObject2 : MonoBehaviour
{

    [SerializeField] Transform bulletPos;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            GameObject bullet = PoolTest2.instance.GetPooledObject();
            if(bullet != null)
            {
                bullet.transform.SetPositionAndRotation(bulletPos.position, Quaternion.identity);
                bullet.SetActive(true);
            }
        }
    }
}
