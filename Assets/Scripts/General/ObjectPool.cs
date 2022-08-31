using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectPool
{
    private GameObject prefab;
    public Stack<GameObject> pool = new Stack<GameObject>();
    public Transform bullets;

    public ObjectPool(GameObject prefab, Transform bullets)
    {
        this.prefab = prefab;
        this.bullets = bullets;
    }

    public void FillPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject go = Object.Instantiate(prefab, bullets);
            AddObjectToPool(go);
        }
    }

    public GameObject PullObjectFromPool()
    {
        if (pool.Count > 0)
        {
            GameObject go = pool.Pop();
            go.gameObject.SetActive(true);
            if(go.transform.childCount > 0)
            {
                foreach (Transform child in go.transform)
                {
                    if (!child.gameObject.activeInHierarchy) child.gameObject.SetActive(true);
                }
            }
            return go;
        }
        return Object.Instantiate(prefab, bullets);
    }

    public void AddObjectToPool(GameObject go)
    {
        go.gameObject.SetActive(false);
        pool.Push(go);
    }

}
