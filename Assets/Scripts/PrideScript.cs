using System.Collections;
using UnityEngine;

public class PrideScript : EnemyBaseMovement
{
    protected override IEnumerator SpecialAttack() 
    {
        Debug.Log("Pride");
        yield return null;
    }
}
