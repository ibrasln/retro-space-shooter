using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletTimerController : MonoBehaviour
{
    public static BulletTimerController instance;
    Slider slider;
    public float maxVal;
    [SerializeField] Transform parent;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        maxVal = 15f;
        slider = GetComponent<Slider>();
        slider.maxValue = maxVal;
    }

    private void Update()
    {
        if (GameManager.instance.isPlayerBlocked) return;
        slider.value = maxVal;
        maxVal -= Time.deltaTime;
        if(maxVal <= 0)
        {
            parent.gameObject.SetActive(false);
            PlayerController.instance.currentBulletStatus = PlayerController.BulletStatus.normalBullet;
        }

    }

}
