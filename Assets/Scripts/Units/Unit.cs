using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitData data;
    public SpriteRenderer myRenderer;
    public string unitID = "";

    private void Start()
    {
        if (data == null) { Debug.Log("generating new data"); data = ScriptableObject.CreateInstance<UnitData>(); }
        if (myRenderer == null) return;

        if(Random.Range(0, 3) == 1)
        {
            data.controlledByPlayer = false;
        }

        if (data.controlledByPlayer)
            myRenderer.color = Color.green;
        else
            myRenderer.color = Color.red;

    }
}
