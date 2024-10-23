using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum ClickMode { TOGGLE, ADD, REMOVE };
public enum GridLayers { TERRAIN, UNIT};

public delegate void EditMode();

public class LevelEditManager : Singleton<LevelEditManager>
{
    public ClickMode editMethod = ClickMode.TOGGLE;
    public GridLayers editLayer = GridLayers.TERRAIN;
    Dictionary<(ClickMode, GridLayers), EditMode> editModes = new();

    protected override void Awake()
    {
        base.Awake();

        editModes.Clear();
        editModes.Add((ClickMode.TOGGLE, GridLayers.TERRAIN), () => ToggleGround());
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            if (editModes.TryGetValue((editMethod, editLayer), out EditMode currentMode))
            {
                currentMode();
            }
        }
    }

    public void ToggleGround()
    {
        Debug.Log("We toggling the ground!");
    }
}
