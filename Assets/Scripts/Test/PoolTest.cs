using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTest : MonoBehaviour
{
    public GameObject bulletPrefab;
    [SerializeField] Transform bulletPos;
    private ObjectPool bulletPool;
    [SerializeField] Transform bullets;
    void Start()
    {
        bulletPool = new ObjectPool(bulletPrefab, bullets);
        bulletPool.FillPool(5);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump")) StartCoroutine(CreateAndDestroyContinuousObject());
    }

    IEnumerator CreateAndDestroyContinuousObject()
    {
        GameObject go = bulletPool.PullObjectFromPool();
        go.transform.position = bulletPos.position;

        yield return new WaitForSeconds(1f);

        bulletPool.AddObjectToPool(go);
    }

}
