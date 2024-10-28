using UnityEngine;

[CreateAssetMenu(fileName = "Unit Data", menuName = "Scriptable Objects/Unit Data", order = 1)]
public class UnitData : ScriptableObject
{
    public string unitName = "UNNAMED UNIT DATA";
    public bool controlledByPlayer = true;
}
