using UnityEngine;

[CreateAssetMenu(fileName = "ItemsData", menuName = "Scriptable Objects/ItemsData")]
public class ItemsData : ScriptableObject
{
    public string itemName;
    public float itemMass;
    public AnimationCurve throwCurve;
}