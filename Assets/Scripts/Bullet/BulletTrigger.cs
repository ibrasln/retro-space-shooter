using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletTrigger : MonoBehaviour
{
    [SerializeField] string targetTag, targetTag2, targetTag3, targetTag4, targetTag5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag) || collision.CompareTag(targetTag2) || collision.CompareTag(targetTag3) || collision.CompareTag(targetTag4) || collision.CompareTag(targetTag5))
        {
            EffectController.instance.CreateEffect(0, gameObject.transform.position, .5f);
            gameObject.SetActive(false);
        }
    }
}
