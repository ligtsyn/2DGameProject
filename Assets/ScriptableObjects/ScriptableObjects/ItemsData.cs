using UnityEngine;

[CreateAssetMenu(fileName = "ItemsData", menuName = "Scriptable Objects/ItemsData")]
public class ItemsData : ScriptableObject
{
    public string itemname;
    public float maxThrowForce;
    public AnimationCurve throwCurve;
}

public enum ItemEffect{
    Noise,
    EnvyCounter,
    PrideCounter
}