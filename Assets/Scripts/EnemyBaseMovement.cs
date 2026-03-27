using Pathfinding;
using System.Collections;
using UnityEngine;

public class EnemyBaseMovement : MonoBehaviour
{
    [SerializeField] private EnemyData _data;
    [SerializeField] private Transform[] navigationPoints;

    private int currentIndex = 0;
    private bool isChasing = false;
    private CircleCollider2D identificationCircle;
    private AILerp pathFinder;
    private Coroutine currentCoroutine;

    private void Awake()
    {
        pathFinder = GetComponent<AILerp>();
        identificationCircle = GetComponent<CircleCollider2D>();
        identificationCircle.radius = _data.enemyIdentificationRadius;
        currentCoroutine = StartCoroutine(PatrolMode());
    }

    private void Start()
    {
        SetCurrentState(_data.startingState,null);
    }

    private void SetCurrentState(EnemyData.EnemyState newState,Transform playerCurrentPosition) 
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        switch (newState)
        {
            case EnemyData.EnemyState.Patrol:
                currentCoroutine = StartCoroutine(PatrolMode());
                break;
            case EnemyData.EnemyState.Scanning:
                currentCoroutine = StartCoroutine(ScanningMode());
                break;
            case EnemyData.EnemyState.Chase:
                currentCoroutine = StartCoroutine(ChasingMode(playerCurrentPosition));
                break;
            case EnemyData.EnemyState.Special:
                currentCoroutine = StartCoroutine(SpecialAttack());
                break;
            case EnemyData.EnemyState.Attack:
                currentCoroutine = StartCoroutine(Attack());
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isChasing)
            {
                isChasing = true;
                SetCurrentState(EnemyData.EnemyState.Chase, other.transform);
            }
        }
    }

    private IEnumerator ChasingMode(Transform playerCurrentPosition)
    {
        pathFinder.speed = _data.chaseSpeed;
        while (isChasing)
        {
            pathFinder.destination = playerCurrentPosition.position;

            float distanceToPlayer = Vector2.Distance(transform.position, playerCurrentPosition.position);

            if (distanceToPlayer > _data.enemyChasingRange)
            {
                isChasing = false;
                SetCurrentState(EnemyData.EnemyState.Patrol, null);
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator PatrolMode()
    {
        pathFinder.speed = _data.patrolSpeed;
        while (!isChasing)
        {
            pathFinder.destination = navigationPoints[currentIndex].transform.position;

            yield return new WaitUntil(() => pathFinder.reachedEndOfPath);
            
            currentIndex++;
            if (currentIndex >= navigationPoints.Length)
                currentIndex = 0;
        }
    }

    private IEnumerator ScanningMode()
    {
        yield return new WaitForSeconds(_data.timeOfScanning);
    }

    //Base attack to kill the player
    private IEnumerator Attack()
    {
        yield return null;
    }

    protected virtual IEnumerator SpecialAttack()
    {
        Debug.Log("Default");
        yield return null;
    }
}