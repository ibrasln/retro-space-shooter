using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentDestroyer : MonoBehaviour
{

    // This script is created for destroy support machines' parents.

    private void Start()
    {
        gameObject.hideFlags = HideFlags.HideInHierarchy;
    }

    void Update()
    {
        if (gameObject.transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
