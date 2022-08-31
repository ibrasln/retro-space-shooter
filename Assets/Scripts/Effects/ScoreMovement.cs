using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMovement : MonoBehaviour
{

    private void Start()
    {
        gameObject.hideFlags = HideFlags.HideInHierarchy;
    }

    private void Update()
    {
        transform.position += new Vector3(0f, .75f * Time.deltaTime, 0f);
    }
}
