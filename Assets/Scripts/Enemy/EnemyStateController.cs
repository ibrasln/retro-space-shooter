using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    public enum EnemyState
    {
        MOVING,
        ATTACKING
    }

    public EnemyState curState;

    public float speed;
    [SerializeField] GameObject enemyPositions;
    Transform targetPos;
    public int indexOfTarget;
    bool isReached;

    private void Start()
    {
        speed = 2.5f;
        curState = EnemyState.MOVING;
        enemyPositions = GameObject.FindGameObjectWithTag("EnemyPositions");

        for (int i = 0; i < enemyPositions.transform.childCount; i++)
        {
            targetPos = enemyPositions.transform.GetChild(indexOfTarget).transform;
        }
    }

    private void Update()
    {
        if (!isReached
            && Vector2.Distance(transform.position, targetPos.position) <= .1f
            && GameManager.instance.curState == GameManager.GameState.WAVESTARTED)
        {
            curState = EnemyState.ATTACKING;
            isReached = true;
        }
        else if (!isReached) transform.position = Vector2.MoveTowards(transform.position, targetPos.position, speed * Time.deltaTime);
    }

}
