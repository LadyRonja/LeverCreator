using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "Unit Data", menuName = "Scriptable Objects/Unit Data", order = 1)]
public class UnitData : ScriptableObject
{
    public string unitName = "UNNAMED UNIT DATA";
    [HideInInspector] public string unitIDforClonedData = "DO NOT FILL";
    public bool controlledByPlayer = true;
}
