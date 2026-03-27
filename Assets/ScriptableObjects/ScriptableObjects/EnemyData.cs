using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public EnemyState startingState = EnemyState.Patrol;
    public string enemyName;
    public float patrolSpeed;
    public float chaseSpeed;
    public float enemyIdentificationRadius;
    public float enemyChasingRange;
    public float timeOfScanning;    

    public enum EnemyState
    {
        Patrol, Scanning, Chase, Special, Attack
    }
}