using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossSupMachineParentController : MonoBehaviour
{

    GameObject targetPositions;
    [SerializeField] float speed;
    float movementDelay;

    // Start is called before the first frame update
    void Start()
    {
        movementDelay = 3.5f;
        targetPositions = GameObject.FindGameObjectWithTag("SupMachinesTargetPos");
    }

    // Update is called once per frame
    private void Update()
    {
        movementDelay -= Time.deltaTime;
        if (movementDelay <= 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).position = Vector2.MoveTowards(transform.GetChild(i).position, targetPositions.transform.GetChild(i).position, speed * Time.deltaTime);
            }
        }

        if (transform.childCount == 0) Destroy(gameObject);

    }
}
