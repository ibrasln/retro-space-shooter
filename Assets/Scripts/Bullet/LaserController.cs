using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{

    public string laserPosTag;

    GameObject laserPos;

    void Start()
    {
        laserPos = GameObject.FindGameObjectWithTag(laserPosTag);
    }

    void Update()
    {
        if (GameManager.instance.isPlayerBlocked || PlayerHealthController.instance.isDead) gameObject.SetActive(false);
        if (laserPos) transform.position = laserPos.transform.position;
    }
}
